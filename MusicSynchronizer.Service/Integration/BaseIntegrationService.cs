using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Domain.Interfaces.Integration;
using MusicSynchronizer.Domain.Interfaces.Spotify;
using MusicSynchronizer.Domain.Interfaces.YouTube;
using MusicSynchronizer.Domain.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace MusicSynchronizer.Service.Integration;

public abstract class BaseIntegrationService : IIntegrationService, IIntegrationServiceSpotify, IIntegrationServiceYouTube
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Uri _baseUri;
    private readonly string _accessTokenHeaderName;

    private HttpClient? _httpClient;

    protected JsonSerializerOptions SerializerOptions { get; }

    //public AuthenticationMethod AuthenticationMethod { get; set; }

    public BaseIntegrationService(IHttpContextAccessor httpContextAccessor, IOptions<AppOptions> appOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _baseUri = appOptions.Value.Api.ToUri();
        _accessTokenHeaderName = appOptions.Value.AccessTokenHeaderName;

        SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        //AuthenticationMethod = authenticationMethod;
        //Client = GetHttpClient();
    }

    //Idk???
    ~BaseIntegrationService() => _httpClient?.Dispose();


    public virtual async Task<CallResult<TResponseModel>> GetAsync<TResponseModel>(string path)
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, path);

        return await Send<TResponseModel>(requestMessage);
    }

    public virtual async Task<CallResult<TResponseModel>> PostAsync<TRequestModel, TResponseModel>(string path, TRequestModel? body)
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, path);

        if (body is not null)
        {
            requestMessage.Content = GetJsonStringContent(body);
        }

        return await Send<TResponseModel>(requestMessage);
    }

    private async Task<CallResult<TResponseModel>> Send<TResponseModel>(HttpRequestMessage requestMessage)
    {
        _httpClient ??= GetHttpClient();

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        CallResult<TResponseModel> callResult = new()
        {
            StatusCode = (int)responseMessage.StatusCode,
            Success = responseMessage.IsSuccessStatusCode
        };

        if (!responseMessage.IsSuccessStatusCode)
        {
            callResult.Result = default;
            callResult.ErrorMessage = await responseMessage.Content.ReadAsStringAsync();

            return callResult;
        }

        TResponseModel? result = await responseMessage.Content.ReadFromJsonAsync<TResponseModel>(SerializerOptions);

        if (result is null)
        {
            throw new Exception($"{nameof(result)} is null.");
        }

        callResult.Result = result;

        return callResult;
    }

    private HttpClient GetHttpClient()
    {
        HttpClientHandler handler = new();

        HttpClient client = new(handler, true) { BaseAddress = _baseUri };

        //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());

        _httpClient = client;
        return _httpClient;
    }

    protected virtual string GetAccessToken()
    {
        return _httpContextAccessor.HttpContext.Request.Headers[_accessTokenHeaderName].ToString();

        //switch (AuthenticationMethod)
        //{
        //    case AuthenticationMethod.AuthorizationCode:
        //        return _httpContextAccessor.HttpContext.Request.Headers[_accessTokenHeaderName].ToString();

        //    case AuthenticationMethod.ClientCredentials:
        //        return _tokenServiceSpotify.GetToken();

        //    default:
        //        return string.Empty;
        //}
    }

    private StringContent GetJsonStringContent<TModel>(TModel body)
    {
        return new StringContent(
            content: JsonSerializer.Serialize(body, SerializerOptions),
            encoding: Encoding.UTF8,
            mediaType: MediaTypeNames.Application.Json);
    }
}
