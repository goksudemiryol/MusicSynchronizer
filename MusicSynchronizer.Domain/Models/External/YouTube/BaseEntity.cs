namespace MusicSynchronizer.Domain.Models.External.YouTube;

public abstract class BaseEntity<TIdType>
{
    public string? Kind { get; set; }

    public string? Etag { get; set; }

    public TIdType? Id { get; set; }

    public Snippet? Snippet { get; set; }

    public Status? Status { get; set; }
}

public class Snippet
{
    public DateTime PublishedAt { get; set; }

    public string? ChannelId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public Thumbnails? Thumbnails { get; set; }

    public string? ChannelTitle { get; set; }

    public string? LiveBroadcastContent { get; set; }

    public DateTime? PublishTime { get; set; }

    public string? PlaylistId { get; set; }

    public ResourceId? ResourceId { get; set; }
}

public class Thumbnails
{
    public required ThumbnailInfo Default { get; set; }

    public required ThumbnailInfo Medium { get; set; }

    public required ThumbnailInfo High { get; set; }
}

public class ThumbnailInfo
{
    public required string Url { get; set; }

    public uint Width { get; set; }

    public uint Height { get; set; }
}

public class Status
{
    public required string PrivacyStatus { get; set; }
}

public class Id
{
    public required string Kind { get; set; }

    public string? VideoId { get; set; }

    public string? ChannelId { get; set; }

    public string? PlaylistId { get; set; }
}

public class ResourceId
{
    public required string Kind { get; set; }

    public required string VideoId { get; set; }
}
