namespace MusicSynchronizer.Common.Configuration.ExternalMusicApp;

public class SpotifyOptions : AppOptions
{
    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public required string HomePage { get; init; }
}
