namespace RedditMockup.Common.Dtos;

public class LoginDto
{
    public string? Username { get; init; }

    public string? Password { get; init; }

    public bool RememberMe { get; init; }

}