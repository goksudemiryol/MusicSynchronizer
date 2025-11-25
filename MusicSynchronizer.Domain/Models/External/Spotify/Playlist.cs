namespace MusicSynchronizer.Domain.Models.External.Spotify;

public class Playlist
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
