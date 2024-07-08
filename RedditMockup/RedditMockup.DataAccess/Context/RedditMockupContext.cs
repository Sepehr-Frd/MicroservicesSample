using Bogus;
using Microsoft.EntityFrameworkCore;
using RedditMockup.Common.Constants;
using RedditMockup.Common.Helpers;
using RedditMockup.Model.Entities;
using Person = RedditMockup.Model.Entities.Person;

namespace RedditMockup.DataAccess.Context;

public class RedditMockupContext : DbContext
{
    // [Constructor]

    public RedditMockupContext(DbContextOptions options) : base(options)
    {
    }

    // [Properties]

    public DbSet<Answer>? Answers { get; set; }

    public DbSet<Person>? Persons { get; set; }

    public DbSet<Profile>? Profiles { get; set; }

    public DbSet<Question>? Questions { get; set; }

    public DbSet<Role>? Roles { get; set; }

    public DbSet<User>? Users { get; set; }

    public DbSet<UserRole>? UserRoles { get; set; }

    public DbSet<AnswerVote>? AnswerVotes { get; set; }

    public DbSet<QuestionVote>? QuestionVotes { get; set; }

    public DbSet<Bookmark>? Bookmarks { get; set; }

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
            .RuleFor(user => user.Password, faker => faker.Internet.Password())
            .RuleFor(user => user.PersonId, _ => id++)
            .RuleFor(user => user.Score, faker => faker.Random.Number(50));

        var fakeUsers = new List<User>();

        fakeUsers.AddRange(new List<User>
        {
            new()
            {
                Id = 1,
                Username = "sepehr_frd",
                Password = "sfr1376".GetHashStringAsync().Result,
                PersonId = 1
            },
            new()
            {
                Id = 2,
                Username = "abbas_booazaar",
                Password = "abbasabbas".GetHashStringAsync().Result,
                PersonId = 2
            }
        });

        for (var i = 0; i < 100; i++)
        {
            fakeUsers.Add(userFaker.Generate());
        }

        return fakeUsers;
    }

    private static List<Profile> GetFakeProfiles()
    {
        var profilesList = new List<Profile>();

        profilesList.AddRange(new List<Profile>
        {
            new()
            {
                Id = 1,
                UserId = 1
            },
            new()
            {
                Id = 2,
                UserId = 2
            }
        });

        for (var i = 3; i < 103; i++)
        {
            profilesList.Add(
                new Profile
                {
                    Id = i,
                    UserId = i
                });
        }

        return profilesList;
    }

    private static List<UserRole> GetFakeUserRoles()
    {
        var userRolesList = new List<UserRole>();

        userRolesList.AddRange(new List<UserRole>
        {
            new()
            {
                Id = 1,
                UserId = 1,
                RoleId = 1
            },
            new()
            {
                Id = 2,
                UserId = 2,
                RoleId = 2
            }
        });

        for (var i = 3; i < 102; i++)
        {
            userRolesList.Add(
                new UserRole
                {
                    Id = i,
                    UserId = i,
                    RoleId = 2
                });
        }

        return userRolesList;
    }

    private static List<Question> GetFakeQuestions()
    {
        var id = 1;

        var questionFaker = new Faker<Question>()
            .RuleFor(question => question.Id, _ => id++)
            .RuleFor(question => question.Title, faker => faker.Lorem.Sentence(5))
            .RuleFor(question => question.Description, faker => faker.Lorem.Paragraph())
            .RuleFor(question => question.UserId, faker => faker.Random.Number(1, 2));

        var fakeQuestions = new List<Question>();

        for (var i = 0; i < 100; i++)
        {
            fakeQuestions.Add(questionFaker.Generate());
        }

        return fakeQuestions;
    }

    private static List<Answer> GetFakeAnswers()
    {
        var id = 1;

        var answerFaker = new Faker<Answer>()
            .RuleFor(answer => answer.Id, _ => id++)
            .RuleFor(answer => answer.Title, faker => faker.Lorem.Sentence(5))
            .RuleFor(answer => answer.Description, faker => faker.Lorem.Paragraph())
            .RuleFor(answer => answer.UserId, faker => faker.Random.Number(1, 2))
            .RuleFor(answer => answer.QuestionId, 1);

        var fakeAnswers = new List<Answer>();

        for (var i = 0; i < 100; i++)
        {
            fakeAnswers.Add(answerFaker.Generate());
        }

        return fakeAnswers;
    }

    private static List<AnswerVote> GetFakeAnswerVotes()
    {
        var id = 1;

        var answerVoteFaker = new Faker<AnswerVote>()
            .RuleFor(answerVote => answerVote.Id, _ => id)
            .RuleFor(answerVote => answerVote.AnswerId, _ => id++)
            .RuleFor(answerVote => answerVote.Kind, true);

        var fakeAnswerVotes = new List<AnswerVote>();

        for (var i = 0; i < 100; i++)
        {
            fakeAnswerVotes.Add(answerVoteFaker.Generate());
        }

        return fakeAnswerVotes;
    }

    private static List<QuestionVote> GetFakeQuestionVotes()
    {
        var id = 1;

        var questionVoteFaker = new Faker<QuestionVote>()
            .RuleFor(questionVote => questionVote.Id, _ => id)
            .RuleFor(questionVote => questionVote.QuestionId, _ => id++)
            .RuleFor(questionVote => questionVote.Kind, true);

        var fakeQuestionVotes = new List<QuestionVote>();

        for (var i = 0; i < 100; i++)
        {
            fakeQuestionVotes.Add(questionVoteFaker.Generate());
        }

        return fakeQuestionVotes;
    }

    private static List<Bookmark> GetFakeBookmarks()
    {
        var id = 1;

        var bookmarkFaker = new Faker<Bookmark>()
            .RuleFor(bookmark => bookmark.Id, _ => id)
            .RuleFor(bookmark => bookmark.UserId, _ => id)
            .RuleFor(bookmark => bookmark.QuestionId, _ => id++)
            .RuleFor(bookmark => bookmark.IsBookmarked, true);

        var fakeBookmarks = new List<Bookmark>();

        for (var i = 0; i < 50; i++)
        {
            fakeBookmarks.Add(bookmarkFaker.Generate());
        }

        return fakeBookmarks;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // [Index Configuration]

        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();


        // [Relationship Configuration]

        // [Person Relationships]

        modelBuilder.Entity<User>()
            .HasOne<Person>(user => user.Person)
            .WithOne(person => person.User)
            .HasForeignKey<User>(user => user.PersonId);


        // [User Relationships]

        modelBuilder.Entity<Profile>()
            .HasOne<User>(profile => profile.User)
            .WithOne(user => user.Profile)
            .HasForeignKey<Profile>(profile => profile.UserId);

        modelBuilder.Entity<Question>()
            .HasOne<User>(question => question.User)
            .WithMany(user => user.Questions)
            .HasForeignKey(question => question.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Answer>()
            .HasOne<User>(answer => answer.User)
            .WithMany(user => user.Answers)
            .HasForeignKey(answer => answer.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
            .HasOne<User>(userRole => userRole.User)
            .WithMany(user => user.UserRoles)
            .HasForeignKey(userRole => userRole.UserId);

        modelBuilder.Entity<Bookmark>()
            .HasOne<User>(bookmark => bookmark.User)
            .WithMany(user => user.Bookmarks)
            .HasForeignKey(bookmark => bookmark.UserId);


        // [Question Relationships]

        modelBuilder.Entity<Bookmark>()
            .HasOne<Question>(bookmark => bookmark.Question)
            .WithMany(question => question.Bookmarks)
            .HasForeignKey(bookmark => bookmark.QuestionId);

        modelBuilder.Entity<Answer>()
            .HasOne<Question>(answer => answer.Question)
            .WithMany(question => question.Answers)
            .HasForeignKey(answer => answer.QuestionId);

        modelBuilder.Entity<QuestionVote>()
            .HasOne<Question>(vote => vote.Question)
            .WithMany(question => question.Votes)
            .HasForeignKey(vote => vote.QuestionId);


        // [Answer Relationships]

        modelBuilder.Entity<AnswerVote>()
            .HasOne<Answer>(vote => vote.Answer)
            .WithMany(answer => answer.Votes)
            .HasForeignKey(vote => vote.AnswerId);


        // [Role Relationships]

        modelBuilder.Entity<UserRole>()
            .HasOne<Role>(userRole => userRole.Role)
            .WithMany(role => role.UserRoles)
            .HasForeignKey(userRole => userRole.RoleId);


        // [Seed Data]

        modelBuilder.Entity<Role>().HasData(new List<Role>
        {
            new()
            {
                Id = 1,
                Title = RoleConstants.Admin
            },
            new()
            {
                Id = 2,
                Title = RoleConstants.User
            }
        });

        modelBuilder.Entity<Person>().HasData(GetFakePeople());

        modelBuilder.Entity<User>().HasData(GetFakeUsers());

        modelBuilder.Entity<Profile>().HasData(GetFakeProfiles());

        modelBuilder.Entity<UserRole>().HasData(GetFakeUserRoles());

        modelBuilder.Entity<AnswerVote>().HasData(GetFakeAnswerVotes());

        modelBuilder.Entity<QuestionVote>().HasData(GetFakeQuestionVotes());

        modelBuilder.Entity<Question>().HasData(GetFakeQuestions());

        modelBuilder.Entity<Answer>().HasData(GetFakeAnswers());

        modelBuilder.Entity<Bookmark>().HasData(GetFakeBookmarks());
    }

}