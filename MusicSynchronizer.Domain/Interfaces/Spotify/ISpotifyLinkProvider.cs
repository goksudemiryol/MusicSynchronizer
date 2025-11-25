namespace MusicSynchronizer.Domain.Interfaces.Spotify;

public interface ISpotifyLinkProvider
{
    Uri GetTrackLink(string trackId);

    Uri GetPlaylistLink(string playlistId);
}
