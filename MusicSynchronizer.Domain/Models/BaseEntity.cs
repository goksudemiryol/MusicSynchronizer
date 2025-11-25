namespace MusicSynchronizer.Domain.Models;

public abstract class BaseEntity
{
    public string Id { get; set; }

    public Uri Href { get; set; }

    protected BaseEntity(string id, Uri href)
    {
        Id = id;
        Href = href;
    }
}
