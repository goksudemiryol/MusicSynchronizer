using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Common.Utilities;
using MusicSynchronizer.Domain.Interfaces.YouTube;
using MusicSynchronizer.Domain.Models;
using MusicSynchronizer.Domain.Models.External.YouTube;
using MusicSynchronizer.Service.Endpoints;
using YTVideo = MusicSynchronizer.Domain.Models.External.YouTube.Video;

namespace MusicSynchronizer.Service.YouTube.Video;

public class VideoService : IVideoService
{
    private readonly IIntegrationServiceYouTube _integrationService;

    public VideoService(IIntegrationServiceYouTube integrationService)
    {
        _integrationService = integrationService;
    }

    public async Task<ServiceResult<List<YTVideo>>> ListVideosByIdAsync(params IEnumerable<string> ids)
    {
        ServiceResult<List<YTVideo>> serviceResult = new();

        if (!ids.Any())
        {
            serviceResult.Success = false;
            serviceResult.StatusCode = 400;
            serviceResult.ErrorMessage = "No id provided.";

            return serviceResult;
        }

        string? path = YouTubeApiEndpoints.Video;

        UriHelpers.AppendQueryParameter(ref path, "id", string.Join(',', ids));
        UriHelpers.AppendQueryParameter(ref path, "part", $"{PartValue.Snippet.ToCamelCaseString()},{PartValue.ContentDetails.ToCamelCaseString()}");

        var callResult = await _integrationService.GetAsync<SearchResponse<YTVideo>>(path);

        if (callResult.Success && callResult.Result is not null)
        {
            serviceResult.Data = callResult.Result.Items;
        }
        else if (callResult.StatusCode.IsClientError())
        {
            serviceResult.Success = false;
            serviceResult.StatusCode = callResult.StatusCode;
            serviceResult.ErrorMessage = callResult.ErrorMessage;
        }
        else
        {
            serviceResult.Success = false;
            serviceResult.StatusCode = 500;
            throw new Exception("unexpected error occurred");
        }

        return serviceResult;
    }
}
