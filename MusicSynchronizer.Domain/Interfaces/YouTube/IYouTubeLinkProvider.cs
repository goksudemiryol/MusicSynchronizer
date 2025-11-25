namespace MusicSynchronizer.Domain.Interfaces.YouTube;

public interface IYouTubeLinkProvider
{
    Uri GetPlaylistLink(string playlistId);

    Uri GetPlaylistItemLink(string playlistId, string trackId);
}
