using MusicSynchronizer.Domain.Models;
using MusicSynchronizer.Domain.Models.External.YouTube;

namespace MusicSynchronizer.Domain.Interfaces.YouTube;

public interface ISearchService
{
    Task<ServiceResult<List<SearchItem>>> SearchVideoAsync(SearchCriteria criteria);

    Task<ServiceResult<SearchItem>> SearchSpecificVideoAsync(SearchCriteria criteria);
}
