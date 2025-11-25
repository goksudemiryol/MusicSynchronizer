namespace MusicSynchronizer.Domain.Models.External.YouTube;

public class Video : BaseEntity<string>
{
    public required ContentDetails ContentDetails { get; set; }
}

public class ContentDetails
{
    /// <summary>
    /// <see href="https://developers.google.com/youtube/v3/docs/videos#contentDetails.duration">Videos  |  YouTube Data API  |  Google for Developers</see>
    /// <br></br><br></br>
    /// The length of the video. The property value is an ISO 8601 duration. For example, for a video that is at least one minute long and less than one hour long, the duration is in the format PT#M#S, in which the letters PT indicate that the value specifies a period of time, and the letters M and S refer to length in minutes and seconds, respectively. The # characters preceding the M and S letters are both integers that specify the number of minutes (or seconds) of the video. For example, a value of PT15M33S indicates that the video is 15 minutes and 33 seconds long.
    /// <br></br><br></br>
    /// If the video is at least one hour long, the duration is in the format PT#H#M#S, in which the # preceding the letter H specifies the length of the video in hours and all of the other details are the same as described above. If the video is at least one day long, the letters P and T are separated, and the value's format is P#DT#H#M#S. Please refer to the ISO 8601 specification for complete details.
    /// </summary>
    public required string Duration { get; set; }
}
