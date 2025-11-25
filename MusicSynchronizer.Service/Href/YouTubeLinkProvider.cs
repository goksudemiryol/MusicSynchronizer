using Microsoft.Extensions.Options;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Domain.Interfaces.YouTube;

namespace MusicSynchronizer.Service.Href;

public class YouTubeLinkProvider : IYouTubeLinkProvider
{
    private readonly IOptions<YouTubeOptions> _options;

    private static string Playlist(string playlistId) => $"playlist?list={playlistId}";
    private static string PlaylistItem(string playlistId, string trackId) => $"watch?v={trackId}&list={playlistId}";

    public YouTubeLinkProvider(IOptions<YouTubeOptions> options)
    {
        _options = options;
    }

    public Uri GetPlaylistLink(string playlistId) => new(_options.Value.MusicHomePage.ToUri(), Playlist(playlistId));

    public Uri GetPlaylistItemLink(string playlistId, string trackId) => new(_options.Value.MusicHomePage.ToUri(), PlaylistItem(playlistId, trackId));
}
