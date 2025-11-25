using MusicSynchronizer.Domain.Models;
using MusicSynchronizer.Domain.Models.External.YouTube;

namespace MusicSynchronizer.Domain.Interfaces.YouTube;

public interface IVideoService
{
    Task<ServiceResult<List<Video>>> ListVideosByIdAsync(params IEnumerable<string> ids);
}
