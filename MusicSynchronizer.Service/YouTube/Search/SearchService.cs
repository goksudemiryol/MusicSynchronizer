using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Common.Utilities;
using MusicSynchronizer.Domain.Interfaces.YouTube;
using MusicSynchronizer.Domain.Models;
using MusicSynchronizer.Domain.Models.External.YouTube;
using MusicSynchronizer.Service.Endpoints;

namespace MusicSynchronizer.Service.YouTube.Search;

public class SearchService : ISearchService
{
    private readonly IIntegrationServiceYouTube _integrationService;

    public SearchService(IIntegrationServiceYouTube integrationService)
    {
        _integrationService = integrationService;
    }

    public async Task<ServiceResult<List<SearchItem>>> SearchVideoAsync(SearchCriteria criteria)
    {
        ServiceResult<List<SearchItem>> serviceResult = new();

        string? pathNoPageToken = YouTubeApiEndpoints.Search;

        //We didn't pass the "snippet" in the part parameter because we'll be querying it later with contentDetails in the list videos method. But let's not forget that the reason we added contentDetails there is the duration information. If we won't need the duration anymore, we can use the snippet here directly without going the list videos method.
        //UriHelpers.AppendQueryParameter(ref pathWithoutPageToken, "part", Part.Snippet.ToLowercaseString());
        UriHelpers.AppendQueryParameter(ref pathNoPageToken, "part", PartValue.Id.ToCamelCaseString());
        UriHelpers.AppendQueryParameter(ref pathNoPageToken, "maxResults",
            criteria.MaxResults > 0 && criteria.MaxResults <= 50
                ? criteria.MaxResults
                : 5);
        UriHelpers.AppendQueryParameter(ref pathNoPageToken, "q", criteria.Q);

        //Also I'm not sure if this is necessary, I think it is.
        UriHelpers.AppendQueryParameter(ref pathNoPageToken, "type", TypeValue.Video.ToCamelCaseString());

        if (!string.IsNullOrEmpty(criteria.Order))
        {
            UriHelpers.AppendQueryParameter(ref pathNoPageToken, "order", criteria.Order);
        }
        if (criteria.SafeSearch != default)
        {
            UriHelpers.AppendQueryParameter(ref pathNoPageToken, "safeSearch", criteria.SafeSearch.ToCamelCaseString());
        }
        if (criteria.VideoCategoryId != VideoCategoryValue.Default)
        {
            UriHelpers.AppendQueryParameter(ref pathNoPageToken, "videoCategoryId", (int)criteria.VideoCategoryId);
        }
        if (criteria.VideoLicense != default)
        {
            UriHelpers.AppendQueryParameter(ref pathNoPageToken, "videoLicense", criteria.VideoLicense.ToCamelCaseString());
        }
        if (!string.IsNullOrEmpty(criteria.VideoDuration))
        {
            UriHelpers.AppendQueryParameter(ref pathNoPageToken, "videoDuration", criteria.VideoDuration);
        }

        do
        {
            string path = pathNoPageToken;

            if (!string.IsNullOrEmpty(criteria.PageToken))
            {
                UriHelpers.AppendQueryParameter(ref path, "pageToken", criteria.PageToken);
            }

            var callResult = await _integrationService.GetAsync<SearchResponse<SearchItem>>(path);

            if (callResult.Success && callResult.Result is not null)
            {
                serviceResult.Data = callResult.Result.Items;
                criteria.PageToken = callResult.Result.NextPageToken;
            }
            else if (callResult.StatusCode.IsClientError())
            {
                serviceResult.Success = false;
                serviceResult.StatusCode = callResult.StatusCode;
                serviceResult.ErrorMessage = callResult.ErrorMessage;

                return serviceResult;
            }
            else
            {
                serviceResult.Success = false;
                serviceResult.StatusCode = 500;
                serviceResult.ErrorMessage = callResult.ErrorMessage;

                throw new Exception("unexpected error occurred");
            }

            //We do not need to search any further for right now...
            break;
        }
        while (criteria.PageToken is not null);

        return serviceResult;
    }

    public async Task<ServiceResult<SearchItem>> SearchSpecificVideoAsync(SearchCriteria criteria)
    {
        throw new NotImplementedException();
    }
}
