namespace RedditMockup.Common.Dtos;

public class UserDto : BaseDto
{
    public string? Username { get; set; }

    public string? Password { get; set; }

    public int Score { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    public ProfileDto? Profile { get; set; }
}