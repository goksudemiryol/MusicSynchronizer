using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using MusicSynchronizer.Service.Integration;
using System.Text.Json;

namespace MusicSynchronizer.Service.YouTube.Integration;

public class IntegrationService : BaseIntegrationService
{
    public IntegrationService(IHttpContextAccessor httpContextAccessor, IOptions<YouTubeOptions> youTubeOptions)
        : base(httpContextAccessor, youTubeOptions)
    {
        SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }
}
