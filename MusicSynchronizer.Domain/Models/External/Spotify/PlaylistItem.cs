namespace MusicSynchronizer.Domain.Models.External.Spotify;

public class PlaylistItem
{
    public int Limit { get; set; }

    public string? Next { get; set; }

    public int Offset { get; set; }

    public string? Previous { get; set; }

    public int Total { get; set; }

    public required List<PlaylistTrack> Items { get; set; }
}

public class PlaylistTrack
{
    public required Track Track { get; set; }
}

public class Track
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public string? Href { get; set; }

    public required List<Artist> Artists { get; set; }

    public required ExternalUrls ExternalUrls { get; set; }

    public required int DurationMs { get; set; }

    public Album? Album { get; set; }
}

public class Artist
{
    public required string Name { get; set; }

    public string? Href { get; set; }
}

public class ExternalUrls
{
    public required string Spotify { get; set; }
}

public class Album
{
    public required string Name { get; set; }

    public required string Href { get; set; }
}