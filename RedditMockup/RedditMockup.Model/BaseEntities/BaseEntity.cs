using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedditMockup.Model.BaseEntities;

public class BaseEntity
{
    // [Constructor]

    protected BaseEntity() => CreationDate = LastUpdated = DateTime.Now;

    

    // [Properties]

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Sieve(CanSort = true)]
    public int Id { get; init; }

    [Sieve(CanSort = true)]
    public DateTime CreationDate { get; }

    [Sieve(CanSort = true)]
    public DateTime LastUpdated { get; set; }

    
}