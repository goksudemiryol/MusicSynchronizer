using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using MusicSynchronizer.Domain.Interfaces.Spotify;
using MusicSynchronizer.Service.Integration;
using System.Text.Json;

namespace MusicSynchronizer.Service.Spotify.Integration;

public class IntegrationService : BaseIntegrationService
{
    private readonly ITokenServiceSpotify _tokenService;

    public IntegrationService(IHttpContextAccessor httpContextAccessor, ITokenServiceSpotify tokenService, IOptions<SpotifyOptions> spotifyOptions)
        : base(httpContextAccessor, spotifyOptions)
    {
        _tokenService = tokenService;
        SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;

        //Client = GetHttpClient();
    }

    protected override string GetAccessToken()
    {
        return _tokenService.GetToken();
    }
}
