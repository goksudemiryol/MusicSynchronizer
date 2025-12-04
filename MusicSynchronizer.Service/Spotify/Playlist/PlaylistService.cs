using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Common.Utilities;
using MusicSynchronizer.Domain.Interfaces.Spotify;
using MusicSynchronizer.Domain.Models;
using MusicSynchronizer.Domain.Models.External.Spotify;
using MusicSynchronizer.Service.Endpoints;
//Namespace and type aliases.
//using ExtNS = MusicSynchronizer.Domain.Models.External.Spotify;
using ExtPlaylist = MusicSynchronizer.Domain.Models.External.Spotify.Playlist;

namespace MusicSynchronizer.Service.Spotify.Playlist;

public class PlaylistService : IPlaylistService
{
    private readonly IIntegrationServiceSpotify _integrationService;

    public PlaylistService(IIntegrationServiceSpotify integrationService)
    {
        _integrationService = integrationService;
    }

    public async Task<ServiceResult<ExtPlaylist>> GetPlaylistAsync(string playlistId)
    {
        ServiceResult<ExtPlaylist> serviceResult = new();

        string path = SpotifyApiEndpoints.GetPlaylist(playlistId);
        UriHelpers.AppendQueryParameter(ref path, "fields", "id,name,description");

        var callResult = await _integrationService.GetAsync<ExtPlaylist>(path);

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

    public async Task<ServiceResult<List<Track>>> GetPlaylistItemsAsync(string playlistId)
    {
        ServiceResult<List<Track>> serviceResult = new();

        int limit = 50, offset = 0;
        List<PlaylistTrack> tracks = new();

        string? path = SpotifyApiEndpoints.GetPlaylistItems(playlistId);

        //UriHelpers.AppendQueryParameter(ref uri, "fields", "limit,offset,next,total,items(track(id,name,duration_ms,href,artists(name),external_ids(isrc),external_urls(spotify)))");
        UriHelpers.AppendQueryParameter(ref path, "fields", "next,total,items(track(id,name,duration_ms,artists(name),external_ids(isrc),external_urls(spotify)))");
        UriHelpers.AppendQueryParameter(ref path, "limit", limit);
        UriHelpers.AppendQueryParameter(ref path, "offset", offset);

        do
        {
            Uri uri = path.ToUri();

            //Aslında integration servisine absolute uri de gönderilebiliyor, sadece HttpClient'ın BaseAddress property'si ile HttpRequestMessage'ın RequestUri property'sindeki absolute kısım farklı olursa şöyle oluyor: HttpRequestMessage'ın RequestUri property'si HttpClient'ın BaseAddress property'sini eziyor, yani değerler farklı ise direkt RequestUri property'sine bakıyor, BaseAddress property'sini kaale almıyor... Integration servisine gelen path eğer absolute ise  bir kontrol ile HttpClient'ın BaseAddress property'sini boş geçebiliriz.
            if (uri.IsAbsoluteUri)
            {
                path = uri.PathAndQuery;
            }

            var callResult = await _integrationService.GetAsync<PlaylistItem>(path);

            if (callResult.Success && callResult.Result is not null)
            {
                tracks.AddRange(callResult.Result.Items);
                path = callResult.Result.Next;

                continue;
            }
            else if (callResult.StatusCode.IsClientError())
            {
                serviceResult.Success = false;
                serviceResult.StatusCode = callResult.StatusCode;
                serviceResult.ErrorMessage = callResult.ErrorMessage;

                break;
            }
            else
            {
                serviceResult.Success = false;
                serviceResult.StatusCode = 500;
                serviceResult.ErrorMessage = callResult.ErrorMessage;

                //If we won't throw an exception...
                //serviceResult.Data = tracks.Select(t => t.Track).ToList();

                throw new Exception("unexpected error occurred");
            }
        }
        while (path is not null && path.IsUriString());

        serviceResult.Data = tracks.Select(t => t.Track).ToList();

        return serviceResult;
    }
}
