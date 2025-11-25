using MusicSynchronizer.Domain.Models;
using ExternalSpoNS = MusicSynchronizer.Domain.Models.External.Spotify;

namespace MusicSynchronizer.Domain.Interfaces.Spotify;

public interface IPlaylistService
{
    Task<ServiceResult<ExternalSpoNS.Playlist>> GetPlaylistAsync(string playlistId);

    Task<ServiceResult<List<ExternalSpoNS.Track>>> GetPlaylistItemsAsync(string playlistId);
}
