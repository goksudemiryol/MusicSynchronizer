namespace MusicSynchronizer.Domain.Models.External.YouTube;

public class SearchCriteria
{
    public SearchCriteria(string query, uint maxResults)
    {
        Q = query;
        MaxResults = maxResults;
        Part = PartValue.Snippet;
        SafeSearch = SafeSearchValue.Moderate;
        Type = TypeValue.Video;
        VideoCategoryId = VideoCategoryValue.Default;
        VideoLicense = VideoLicenseValue.Any;
    }

    public PartValue Part { get; set; }

    public bool ForDeveloper { get; set; }

    public uint MaxResults { get; set; }

    public string? Order { get; set; }

    public string? PageToken { get; set; }

    public string Q { get; set; }

    public SafeSearchValue SafeSearch { get; set; }

    public TypeValue Type { get; set; }

    public VideoCategoryValue VideoCategoryId { get; set; }

    public string? VideoDuration { get; set; }

    public VideoLicenseValue VideoLicense { get; set; }
}
