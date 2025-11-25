namespace MusicSynchronizer.Common.Configuration.ExternalMusicApp;

public class AppOptions
{
    public required string Login { get; init; }

    public required string Api { get; init; } //Configuration binding is case-insensitive.

    public required string AccessTokenHeaderName { get; init; }
}
