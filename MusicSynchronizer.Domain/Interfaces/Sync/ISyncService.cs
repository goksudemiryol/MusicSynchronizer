using MusicSynchronizer.Domain.Models;

namespace MusicSynchronizer.Domain.Interfaces.Sync;

public interface ISyncService
{
    Task<ServiceResult<SyncResponse>> CreateYouTubePlaylistFromSpotifyAsync(string playlistId);
}
