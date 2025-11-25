namespace MusicSynchronizer.Domain.Models.External.YouTube;

public class YouTubeError
{
    public required Error Error { get; set; }
}

public class Error
{
    public required int Code { get; set; }

    public required string Message { get; set; }

    public required List<ErrorDetail> Errors { get; set; }

    public required string Status { get; set; }
}

public class ErrorDetail
{
    public required string Message { get; set; }

    public required string Domain { get; set; }

    public required string Reason { get; set; }

    public required string Location { get; set; }

    public required string LocationType { get; set; }
}
