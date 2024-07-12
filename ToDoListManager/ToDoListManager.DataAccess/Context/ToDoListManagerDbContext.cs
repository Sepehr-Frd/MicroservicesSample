using Bogus;
using Microsoft.EntityFrameworkCore;
using ToDoListManager.Common.Helpers;
using ToDoListManager.Model.Entities;
using ToDoListManager.Model.Enums;
using Person = ToDoListManager.Model.Entities.Person;

namespace ToDoListManager.DataAccess.Context;

public class ToDoListManagerDbContext : DbContext
{
    public ToDoListManagerDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category>? Categories { get; init; }

    public DbSet<Person>? Persons { get; init; }

    public DbSet<ToDoItem>? ToDoItems { get; init; }

    public DbSet<ToDoList>? ToDoLists { get; init; }

    public DbSet<User>? Users { get; init; }

    // [Methods]

    private static List<Person> GetFakePeople()
    {
        var id = 3;

        var personFaker = new Faker<Person>()
            .RuleFor(person => person.Id, _ => id++)
            .RuleFor(person => person.FirstName, faker => faker.Name.FirstName())
            .RuleFor(person => person.LastName, faker => faker.Name.LastName());

        var fakePeople = new List<Person>();

        fakePeople.AddRange(new List<Person>
        {
            new()
            {
                Id = 1,
                FirstName = "Sepehr",
                LastName = "Foroughi Rad"
            },
            new()
            {
                Id = 2,
                FirstName = "Abbas",
                LastName = "BooAzaar"
            }
        });

        for (var i = 0; i < 100; i++)
        {
            fakePeople.Add(personFaker.Generate());
        }

        return fakePeople;
    }

    private static List<User> GetFakeUsers()
    {
        var id = 3;

        var userFaker = new Faker<User>()
            .RuleFor(user => user.Id, _ => id)
            .RuleFor(user => user.Username, faker => faker.Internet.UserName())
            .RuleFor(user => user.PasswordHash, faker => faker.Internet.Password().GetHashStringAsync().Result)
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.PersonId, _ => id++);

        var fakeUsers = new List<User>();

        fakeUsers.AddRange(new List<User>
        {
            new()
            {
                Id = 1,
                Username = "sepehr_frd",
                PasswordHash = "sfr1376".GetHashStringAsync().Result,
                PersonId = 1,
                Email = "sepfrd@outlook.com"
            },
            new()
            {
                Id = 2,
                Username = "abbas_booazaar",
                PasswordHash = "abbasabbas".GetHashStringAsync().Result,
                PersonId = 2,
                Email = "abbas_booazaar@outlook.com"
            }
        });

        for (var i = 0; i < 100; i++)
        {
            fakeUsers.Add(userFaker.Generate());
        }

        return fakeUsers;
    }

    private static List<ToDoItem> GetFakeToDoItems()
    {
        var id = 1;

        var toDoItemFaker = new Faker<ToDoItem>()
            .RuleFor(toDoItem => toDoItem.Id, _ => id)
            .RuleFor(toDoItem => toDoItem.CategoryId, _ => id)
            .RuleFor(toDoItem => toDoItem.ToDoListId, _ => id++)
            .RuleFor(toDoItem => toDoItem.IsCompleted, faker => faker.Random.Bool())
            .RuleFor(toDoItem => toDoItem.Title, faker => faker.Name.JobTitle())
            .RuleFor(toDoItem => toDoItem.Description, faker => faker.Name.JobDescriptor())
            .RuleFor(toDoItem => toDoItem.Priority, faker => faker.PickRandom<Priority>())
            .RuleFor(toDoItem => toDoItem.DueDate, faker => faker.Date.Between(DateTime.Today.AddDays(-100), DateTime.Today.AddDays(100)));

        var fakeToDoItems = new List<ToDoItem>();

        for (var i = 0; i < 100; i++)
        {
            fakeToDoItems.Add(toDoItemFaker.Generate());
        }

        return fakeToDoItems;
    }

    private static List<ToDoList> GetFakeToDoLists()
    {
        var id = 1;

        var toDoListFaker = new Faker<ToDoList>()
            .RuleFor(toDoList => toDoList.Id, _ => id)
            .RuleFor(toDoList => toDoList.Name, faker => faker.Commerce.ProductName())
            .RuleFor(toDoList => toDoList.Description, faker => faker.Name.JobDescriptor())
            .RuleFor(toDoList => toDoList.UserId, _ => id++);

        var fakeToDoLists = new List<ToDoList>();

        for (var i = 0; i < 100; i++)
        {
            fakeToDoLists.Add(toDoListFaker.Generate());
        }

        return fakeToDoLists;
    }

    private static List<Category> GetFakeCategories()
    {
        var id = 1;

        var categoryFaker = new Faker<Category>()
            .RuleFor(category => category.Id, _ => id)
            .RuleFor(category => category.Name, faker => faker.Commerce.ProductName())
            .RuleFor(category => category.UserId, _ => id++);

        var fakeCategories = new List<Category>();

        for (var i = 0; i < 100; i++)
        {
            fakeCategories.Add(categoryFaker.Generate());
        }

        return fakeCategories;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasKey(category => category.Id);
        modelBuilder.Entity<User>().HasKey(user => user.Id);
        modelBuilder.Entity<Person>().HasKey(person => person.Id);
        modelBuilder.Entity<ToDoItem>().HasKey(toDoItem => toDoItem.Id);
        modelBuilder.Entity<ToDoList>().HasKey(toDoList => toDoList.Id);

        // [Index Configuration]

        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();

        // [Relationship Configuration]

        modelBuilder
            .Entity<User>()
            .HasOne(user => user.Person)
            .WithOne(person => person.User)
            .HasForeignKey<User>(user => user.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<ToDoList>()
            .HasOne(toDoList => toDoList.User)
            .WithMany(user => user.ToDoLists)
            .HasForeignKey(toDoList => toDoList.UserId)
            .HasPrincipalKey(user => user.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<ToDoItem>()
            .HasOne(toDoItem => toDoItem.ToDoList)
            .WithMany(toDoList => toDoList.ToDoItems)
            .HasForeignKey(toDoItem => toDoItem.ToDoListId)
            .HasPrincipalKey(toDoList => toDoList.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<ToDoItem>()
            .HasOne(toDoItem => toDoItem.Category)
            .WithMany(category => category.ToDoItems)
            .HasForeignKey(toDoItem => toDoItem.CategoryId)
            .HasPrincipalKey(category => category.Id)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Category>()
            .HasOne(category => category.User)
            .WithMany(user => user.Categories)
            .HasForeignKey(category => category.UserId)
            .HasPrincipalKey(user => user.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // [Seed Data]

        modelBuilder.Entity<Category>().HasData(GetFakeCategories());

        modelBuilder.Entity<Person>().HasData(GetFakePeople());

        modelBuilder.Entity<User>().HasData(GetFakeUsers());

        modelBuilder.Entity<ToDoItem>().HasData(GetFakeToDoItems());

        modelBuilder.Entity<ToDoList>().HasData(GetFakeToDoLists());
    }

}