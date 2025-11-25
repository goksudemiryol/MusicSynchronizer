namespace MusicSynchronizer.Domain.Models;

public class SyncResponse
{
    public SyncResponse(Playlist sourcePlaylist)
    {
        SourcePlaylist = sourcePlaylist;
    }

    public Playlist SourcePlaylist { get; set; }

    public List<SuccessfulSync> Successful { get; set; } = [];

    public List<FailedSync> Failed { get; set; } = [];

    public int SuccessfulCount { get => Successful.Count; }

    public int FailedCount { get => Failed.Count; }
}

public class SuccessfulSync
{
    public SuccessfulSync(Media sourceTrack, Media addedTrack)
    {
        SourceTrack = sourceTrack;
        AddedTrack = addedTrack;
    }

    public Media SourceTrack { get; set; }

    public Media AddedTrack { get; set; }
}

public class FailedSync
{
    public FailedSync(Media sourceTrack, string errorMessage)
    {
        SourceTrack = sourceTrack;
        ErrorMessage = errorMessage;
    }

    public Media SourceTrack { get; set; }

    public string ErrorMessage { get; set; }
}
