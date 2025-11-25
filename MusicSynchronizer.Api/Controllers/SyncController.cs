using Microsoft.AspNetCore.Mvc;
using MusicSynchronizer.Api.ViewModels;
using MusicSynchronizer.Domain.Interfaces.Sync;

namespace MusicSynchronizer.Api.Controllers;

public class SyncController : BaseApiController
{
    private readonly ISyncService _syncService;

    public SyncController(ISyncService syncService)
    {
        _syncService = syncService;
    }

    [HttpPost]
    public async Task<IActionResult> SyncSpotifyPlaylist([FromBody] PlaylistViewModel playlist)
    {
        if (playlist is null)
        {
            return BadRequest(playlist);
        }

        var result = await _syncService.CreateYouTubePlaylistFromSpotifyAsync(playlist.Id);

        return StatusCode(result.StatusCode, result.Success ? (SyncResponseViewModel)result.Data : result.ErrorMessage);





        //Validations, error models...

        //return NotFound(new ProblemDetails
        //{
        //    Title = "Spotify Playlist not Found",
        //    Detail = $"No Spotify playlist found with the given ID \"{spotifyPlaylistId}\"."
        //});
    }
}
