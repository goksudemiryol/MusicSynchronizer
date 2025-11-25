using MusicSynchronizer.Domain.Models;
using ExternalYtNS = MusicSynchronizer.Domain.Models.External.YouTube;

namespace MusicSynchronizer.Domain.Interfaces.YouTube;

public interface IPlaylistService
{
    Task<ServiceResult<ExternalYtNS.Playlist>> CreatePlaylistAsync(ExternalYtNS.Playlist properties);

    Task<ServiceResult<ExternalYtNS.PlaylistItem>> AddItemToPlaylistAsync(ExternalYtNS.PlaylistItem playlistItem);
}
