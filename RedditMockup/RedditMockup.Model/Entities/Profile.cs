using RedditMockup.Model.BaseEntities;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Profile : BaseEntity
{
    #region [Properties]

    public string? Bio { get; set; } = string.Empty;

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Email { get; set; } = string.Empty;
    
    #endregion

    #region [Navigation Properties]
    
    public int UserId { get; set; }
    
    public User? User { get; set; }

    #endregion
}