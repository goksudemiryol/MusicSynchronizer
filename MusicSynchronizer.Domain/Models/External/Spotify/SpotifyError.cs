namespace MusicSynchronizer.Domain.Models.External.Spotify;

public class SpotifyError
{
    public required Error Error { get; set; }
}

public class Error
{
    public required int Status { get; set; }

    public required string Message { get; set; }
}
