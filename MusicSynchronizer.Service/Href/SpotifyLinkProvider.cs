using Microsoft.Extensions.Options;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Domain.Interfaces.Spotify;

namespace MusicSynchronizer.Service.Href;

public class SpotifyLinkProvider : ISpotifyLinkProvider
{
    private readonly IOptions<SpotifyOptions> _options;

    private static string Track(string trackId) => $"track/{trackId}";
    private static string Playlist(string playlistId) => $"playlist/{playlistId}";

    public SpotifyLinkProvider(IOptions<SpotifyOptions> options)
    {
        _options = options;
    }

    public Uri GetTrackLink(string trackId) => new(_options.Value.HomePage.ToUri(), Track(trackId));

    public Uri GetPlaylistLink(string playlistId) => new(_options.Value.HomePage.ToUri(), Playlist(playlistId));
}
