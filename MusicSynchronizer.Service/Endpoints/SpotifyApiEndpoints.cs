namespace MusicSynchronizer.Service.Endpoints;

public static class SpotifyApiEndpoints
{
    public static string GetPlaylist(string playlistId) => $"v1/playlists/{playlistId}";

    public static string GetPlaylistItems(string playlistId) => $"v1/playlists/{playlistId}/tracks";
}
