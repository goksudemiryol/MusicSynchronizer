namespace MusicSynchronizer.Domain.Models.External.YouTube;

public class SearchResponse<TItemType>
{
    public required string Kind { get; set; }

    public required string Etag { get; set; }

    public string? NextPageToken { get; set; }

    public string? PrevPageToken { get; set; }

    public string? RegionCode { get; set; }

    public required PageInfo PageInfo { get; set; }

    public required List<TItemType> Items { get; set; }
}

public class PageInfo
{
    public int TotalResults { get; set; }

    public int ResultsPerPage { get; set; }
}
