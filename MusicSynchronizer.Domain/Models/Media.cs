namespace MusicSynchronizer.Domain.Models;

public class Media : BaseEntity
{
    public string Title { get; set; }

    //public TimeSpan Duration { get; set; }

    public Media(string id, Uri href, string title) : base(id, href)
    {
        Title = title;
    }

    //public static explicit operator Media(SearchItem searchItem)
    //{
    //    return new Media()
    //    {
    //        Id = searchItem.Id.VideoId,
    //        Title = searchItem.Snippet.Title,
    //        Duration = TimeSpan.MinValue
    //    };
    //}

    //public static explicit operator Media(Video searchItem)
    //{
    //    return new Media()
    //    {
    //        Id = searchItem.Id,
    //        Title = searchItem.Snippet.Title,
    //        Duration = TimeSpan.MinValue
    //        //Duration = searchItem.ContentDetails.Duration.ConvertIso8601ToMilliSeconds()
    //    };
    //}
}
