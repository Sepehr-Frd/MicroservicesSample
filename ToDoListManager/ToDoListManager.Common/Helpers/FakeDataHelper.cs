using Bogus;
using ToDoListManager.Model.Entities;
using ToDoListManager.Model.Enums;
using Person = ToDoListManager.Model.Entities.Person;

namespace ToDoListManager.Common.Helpers;

public static class FakeDataHelper
{
    public static readonly Person FakeAdminPerson = new()
    {
        Guid = Guid.NewGuid(),
        FirstName = "Sepehr",
        LastName = "Foroughi Rad"
    };

    public static readonly Person FakeUserPerson = new()
    {
        Guid = Guid.NewGuid(),
        FirstName = "Bernard",
        LastName = "Cool"
    };

    public static List<Person> GetFakePeople(int countToGenerate)
    {
        var personFaker = new Faker<Person>()
            .RuleFor(person => person.FirstName, faker => faker.Name.FirstName())
            .RuleFor(person => person.LastName, faker => faker.Name.LastName());

        var fakePeople = new List<Person>
        {
            FakeAdminPerson,
            FakeUserPerson
        };

        var currentPeopleCount = fakePeople.Count;

        fakePeople.AddRange(personFaker.Generate(countToGenerate - currentPeopleCount));

        return fakePeople;
    }

    public static List<User> GetFakeUsers(List<Person> fakePeople, int countToGenerate)
    {
        var fakeUsers = new List<User>();

        fakeUsers.AddRange(new List<User>
        {
            new()
            {
                Username = "sepehr_frd",
                PasswordHash = "Sfr1376.".GetHashStringAsync().Result,
                Email = "sepfrd@outlook.com",
                PersonId = fakePeople.First(person => person.Guid == FakeAdminPerson.Guid).Id
            },
            new()
            {
                Username = "bernard_cool",
                PasswordHash = "BernardCool1997.".GetHashStringAsync().Result,
                Email = "bercool@gmail.com",
                PersonId = fakePeople.First(person => person.Guid == FakeUserPerson.Guid).Id
            }
        });

        var currentUsersCount = fakeUsers.Count;

        var fakePeopleIndex = currentUsersCount;

        var userFaker = new Faker<User>()
            .RuleFor(user => user.PersonId, _ => fakePeople[fakePeopleIndex++].Id)
            .RuleFor(user => user.Username, faker => faker.Internet.UserName())
            .RuleFor(user => user.PasswordHash, faker => faker.Internet.Password().GetHashStringAsync().Result)
            .RuleFor(user => user.Email, faker => faker.Internet.Email());

        fakeUsers.AddRange(userFaker.Generate(fakePeople.Count - currentUsersCount));

        return fakeUsers;
    }

    public static List<ToDoItem> GetFakeToDoItems(List<ToDoList> fakeToDoLists, List<Category> fakeCategories, int countToGenerate)
    {
        var toDoItemFaker = new Faker<ToDoItem>()
            .RuleFor(toDoItem => toDoItem.ToDoListId, faker => faker.PickRandom(fakeToDoLists).Id)
            .RuleFor(toDoItem => toDoItem.CategoryId, faker => faker.PickRandom(fakeCategories).Id)
            .RuleFor(toDoItem => toDoItem.IsCompleted, faker => faker.Random.Bool())
            .RuleFor(toDoItem => toDoItem.Title, faker => faker.Name.JobTitle())
            .RuleFor(toDoItem => toDoItem.Description, faker => faker.Name.JobDescriptor())
            .RuleFor(toDoItem => toDoItem.Priority, faker => faker.PickRandom<Priority>())
            .RuleFor(toDoItem => toDoItem.DueDate, faker => faker.Date.Between(DateTime.Today.AddDays(-100), DateTime.Today.AddDays(100)));

        var fakeToDoItems = toDoItemFaker.Generate(countToGenerate);

        return fakeToDoItems;
    }

    public static List<ToDoList> GetFakeToDoLists(List<User> fakeUsers, int countToGenerate)
    {
        var toDoListFaker = new Faker<ToDoList>()
            .RuleFor(toDoList => toDoList.UserId, faker => faker.PickRandom(fakeUsers).Id)
            .RuleFor(toDoList => toDoList.Name, faker => faker.Commerce.ProductName())
            .RuleFor(toDoList => toDoList.Description, faker => faker.Name.JobDescriptor());

        var fakeToDoLists = toDoListFaker.Generate(countToGenerate);

        return fakeToDoLists;
    }

    public static List<Category> GetFakeCategories(List<User> fakeUsers, int countToGenerate)
    {
        var categoryFaker = new Faker<Category>()
            .RuleFor(category => category.UserId, faker => faker.PickRandom(fakeUsers).Id)
            .RuleFor(category => category.Name, faker => faker.Commerce.ProductName());

        var fakeCategories = categoryFaker.Generate(countToGenerate);

        return fakeCategories;
    }
}