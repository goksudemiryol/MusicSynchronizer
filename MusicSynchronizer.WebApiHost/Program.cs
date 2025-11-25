using MusicSynchronizer.Api.Middleware;
using MusicSynchronizer.Common.Configuration.ExternalMusicApp;
using MusicSynchronizer.Domain.Interfaces.Spotify;
using MusicSynchronizer.Domain.Interfaces.Sync;
using MusicSynchronizer.Domain.Interfaces.YouTube;
using MusicSynchronizer.Service.Href;
using MusicSynchronizer.Service.Sync;
using AuthenticationSpoNS = MusicSynchronizer.Infrastructure.Spotify.Authentication;
using InterfacesSpoNS = MusicSynchronizer.Domain.Interfaces.Spotify;
using InterfacesYtNS = MusicSynchronizer.Domain.Interfaces.YouTube;
using ServiceSpoNS = MusicSynchronizer.Service.Spotify;
using ServiceYtNS = MusicSynchronizer.Service.YouTube;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//////////////////////////////////////////////////////////////////////////////////////////////////

builder.Services.Configure<SpotifyOptions>(builder.Configuration.GetSection("ExternalApp:Spotify"));
builder.Services.Configure<YouTubeOptions>(builder.Configuration.GetSection("ExternalApp:YouTube"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
//Http Client getirmek için kullanılabilir.
//builder.Services.AddHttpClient();

builder.Services
    .AddScoped<IIntegrationServiceSpotify, ServiceSpoNS.Integration.IntegrationService>()
    .AddScoped<IIntegrationServiceYouTube, ServiceYtNS.Integration.IntegrationService>()
    .AddScoped<ISyncService, SyncService>()
    .AddScoped<InterfacesSpoNS.IPlaylistService, ServiceSpoNS.Playlist.PlaylistService>()
    .AddScoped<InterfacesYtNS.IPlaylistService, ServiceYtNS.Playlist.PlaylistService>()
    .AddScoped<InterfacesYtNS.ISearchService, ServiceYtNS.Search.SearchService>()
    .AddScoped<InterfacesYtNS.IVideoService, ServiceYtNS.Video.VideoService>()
    .AddSingleton<InterfacesSpoNS.ITokenServiceSpotify, AuthenticationSpoNS.TokenService>()
    .AddScoped<InterfacesSpoNS.ISpotifyLinkProvider, SpotifyLinkProvider>()
    .AddScoped<InterfacesYtNS.IYouTubeLinkProvider, YouTubeLinkProvider>();

//////////////////////////////////////////////////////////////////////////////////////////////////
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
