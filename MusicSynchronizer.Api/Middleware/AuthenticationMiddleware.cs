using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using System.Net;
using System.Net.Mime;

namespace MusicSynchronizer.Api.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SpotifyOptions _spotifyOptions;
    private readonly YouTubeOptions _youtubeOptions;

    public AuthenticationMiddleware(RequestDelegate next, IOptions<SpotifyOptions> spotifyOptions, IOptions<YouTubeOptions> youtubeOptions)
    {
        _next = next;
        _spotifyOptions = spotifyOptions.Value;
        _youtubeOptions = youtubeOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string youtubeToken = context.Request.Headers[_youtubeOptions.AccessTokenHeaderName].ToString();

        if (string.IsNullOrEmpty(youtubeToken))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = MediaTypeNames.Text.Plain;

            await context.Response.WriteAsync($"YouTube access token could not be obtained. Make sure you set the header \"{_youtubeOptions.AccessTokenHeaderName}\".");

            return;
        }

        await _next(context);
    }
}
