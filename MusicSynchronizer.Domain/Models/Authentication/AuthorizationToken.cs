namespace MusicSynchronizer.Domain.Models.Authentication;

public class AuthorizationToken
{
    public required string AccessToken { get; set; }

    public required string TokenType { get; set; }

    public required int ExpiresIn { get; set; }
}
