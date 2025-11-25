using Microsoft.Extensions.Options;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Common.Utilities;
using MusicSynchronizer.Domain.Interfaces.YouTube;
using MusicSynchronizer.Domain.Models;
using MusicSynchronizer.Domain.Models.External.YouTube;
using MusicSynchronizer.Service.Endpoints;
//Namespace and type aliases.
//using ExtNS = MusicSynchronizer.Domain.Models.External.YouTube;
using ExtPlaylist = MusicSynchronizer.Domain.Models.External.YouTube.Playlist;

namespace MusicSynchronizer.Service.YouTube.Playlist;

public class PlaylistService : IPlaylistService
{
    private readonly IIntegrationServiceYouTube _integrationService;
    private readonly YouTubeOptions _youtubeOptions;

    public PlaylistService(IIntegrationServiceYouTube integrationService, IOptions<YouTubeOptions> youtubeOptions)
    {
        _integrationService = integrationService;
        _youtubeOptions = youtubeOptions.Value;
    }

    public async Task<ServiceResult<ExtPlaylist>> CreatePlaylistAsync(ExtPlaylist playlistProperties)
    {
        ServiceResult<ExtPlaylist> serviceResult = new();

        string path = YouTubeApiEndpoints.Playlist;
        UriHelpers.AppendQueryParameter(ref path, "part", PartValue.Snippet.ToCamelCaseString());

        var callResult = await _integrationService.PostAsync<ExtPlaylist, ExtPlaylist>(path, playlistProperties);

        if (callResult.Success)
        {
            serviceResult.Data = callResult.Result;
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

    public async Task<ServiceResult<PlaylistItem>> AddItemToPlaylistAsync(PlaylistItem playlistItem)
    {
        ServiceResult<PlaylistItem> serviceResult = new();

        string path = YouTubeApiEndpoints.PlaylistItems;
        UriHelpers.AppendQueryParameter(ref path, "part", PartValue.Snippet.ToCamelCaseString());

        var callResult = await _integrationService.PostAsync<PlaylistItem, PlaylistItem>(path, playlistItem);

        if (callResult.Success)
        {
            serviceResult.Data = callResult.Result;
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
