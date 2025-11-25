using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Domain.Interfaces.Spotify;
using MusicSynchronizer.Domain.Models.Authentication;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MusicSynchronizer.Infrastructure.Spotify.Authentication;

public class TokenService : ITokenServiceSpotify
{
    private readonly IMemoryCache _cache;
    private readonly SpotifyOptions _spotifyOptions;
    private readonly JsonSerializerOptions _serializerOptions;

    private const string _accessTokenCacheKey = "SpotifyAccessToken";

    public TokenService(IMemoryCache cache, IOptions<SpotifyOptions> spotifyOptions)
    {
        _cache = cache;
        _spotifyOptions = spotifyOptions.Value;

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }

    public string GetToken()
    {
        if (_cache.TryGetValue(_accessTokenCacheKey, out string? token) && token is not null)
        {
            return token;
        }

        var newToken = RequestNewToken();

        _cache.Set(_accessTokenCacheKey, newToken.AccessToken, TimeSpan.FromSeconds(newToken.ExpiresIn - 10));

        return newToken.AccessToken;
    }

    private AuthorizationToken RequestNewToken()
    {
        using HttpClientHandler handler = new();

        using HttpClient client = new(handler, true) { BaseAddress = _spotifyOptions.Login.ToUri() };

        using HttpRequestMessage requestMessage = new(HttpMethod.Post, "api/token");

        requestMessage.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" }
        });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedCredentials(_spotifyOptions.ClientId, _spotifyOptions.ClientSecret));

        using HttpResponseMessage responseMessage = client.Send(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        var token = JsonSerializer.Deserialize<AuthorizationToken>(responseMessage.Content.ReadAsString(), _serializerOptions);

        if (token is null)
        {
            throw new Exception("Spotify access token could not be get using the client credentials.");
        }

        return token;
    }

    private static string GetEncodedCredentials(string clientId, string clientSecret)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret));
    }
}
