using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Person : BaseEntityWithGuid
{
    // [Properties]

    [Sieve(CanFilter = true, CanSort = true)]
    public string? FirstName { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? LastName { get; set; }

    public string FullName => FirstName + " " + LastName;

    // [Navigation Properties]

    public User? User { get; set; }
}