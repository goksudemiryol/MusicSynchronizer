using MusicSynchronizer.Domain.Models;

namespace MusicSynchronizer.Api.ViewModels;

public class SyncResponseViewModel
{
    public SyncResponseViewModel(Playlist sourcePlaylist)
    {
        SourcePlaylist = sourcePlaylist;
    }

    public Playlist SourcePlaylist { get; set; }

    public List<SuccessfulSync> Successful { get; set; } = [];

    public List<FailedSync> Failed { get; set; } = [];

    public int SuccessfulCount { get => Successful.Count; }

    public int FailedCount { get => Failed.Count; }


    public static explicit operator SyncResponseViewModel(SyncResponse syncResponse)
    {
        return new SyncResponseViewModel(syncResponse.SourcePlaylist)
        {
            Successful = syncResponse.Successful,
            Failed = syncResponse.Failed
        };
    }
}
