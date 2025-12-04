using MusicSynchronizer.Common.Extensions;
using MusicSynchronizer.Domain.Interfaces.Sync;
using MusicSynchronizer.Domain.Models;
using MusicSynchronizer.Domain.Models.External.YouTube;
using SpotifyInterfacesNS = MusicSynchronizer.Domain.Interfaces.Spotify;
using SpotifyModelsNS = MusicSynchronizer.Domain.Models.External.Spotify;
using YouTubeInterfacesNS = MusicSynchronizer.Domain.Interfaces.YouTube;
using YouTubeModelsNS = MusicSynchronizer.Domain.Models.External.YouTube;

namespace MusicSynchronizer.Service.Sync;

public class SyncService : ISyncService
{
    private readonly SpotifyInterfacesNS.IPlaylistService _playlistServiceSpotify;
    private readonly YouTubeInterfacesNS.IPlaylistService _playlistServiceYoutube;
    private readonly YouTubeInterfacesNS.ISearchService _searchServiceYoutube;
    private readonly YouTubeInterfacesNS.IVideoService _videoServiceYoutube;
    private readonly YouTubeInterfacesNS.IYouTubeLinkProvider _youtubeLinkProvider;
    private readonly SpotifyInterfacesNS.ISpotifyLinkProvider _spotifyLinkProvider;

    public SyncService(
        SpotifyInterfacesNS.IPlaylistService playlistServiceSpotify,
        YouTubeInterfacesNS.IPlaylistService playlistServiceYoutube,
        YouTubeInterfacesNS.ISearchService searchServiceYoutube,
        YouTubeInterfacesNS.IVideoService videoServiceYoutube,
        YouTubeInterfacesNS.IYouTubeLinkProvider youtubeLinkProvider,
        SpotifyInterfacesNS.ISpotifyLinkProvider spotifyLinkProvider)
    {
        _playlistServiceSpotify = playlistServiceSpotify;
        _playlistServiceYoutube = playlistServiceYoutube;
        _searchServiceYoutube = searchServiceYoutube;
        _videoServiceYoutube = videoServiceYoutube;
        _youtubeLinkProvider = youtubeLinkProvider;
        _spotifyLinkProvider = spotifyLinkProvider;
    }

    public async Task<ServiceResult<SyncResponse>> CreateYouTubePlaylistFromSpotifyAsync(string spotifyPlaylistId)
    {
        ServiceResult<SyncResponse> serviceResult = new();

        var srGetSpotifyPlaylist = await _playlistServiceSpotify.GetPlaylistAsync(spotifyPlaylistId);

        if (srGetSpotifyPlaylist.StatusCode.IsClientError() || srGetSpotifyPlaylist.Data is null)
        {
            serviceResult.Success = false;
            serviceResult.StatusCode = srGetSpotifyPlaylist.StatusCode;
            serviceResult.ErrorMessage = srGetSpotifyPlaylist.ErrorMessage;

            return serviceResult;
        }

        var srGetSpotifyPlaylistItems = await _playlistServiceSpotify.GetPlaylistItemsAsync(spotifyPlaylistId);

        if (!srGetSpotifyPlaylistItems.Success || srGetSpotifyPlaylistItems.Data is null)
        {
            serviceResult.Success = srGetSpotifyPlaylistItems.Success;
            serviceResult.StatusCode = srGetSpotifyPlaylistItems.StatusCode;
            serviceResult.ErrorMessage = srGetSpotifyPlaylistItems.ErrorMessage;

            return serviceResult;
        }

        YouTubeModelsNS.Playlist youTubePlaylist = new()
        {
            Snippet = new()
            {
                Title = srGetSpotifyPlaylist.Data.Name
            },
            //Uncomment this and try passing "snippet,status" for the part in the playlist service, the documentation does it for public.
            //Status = new()
            //{
            //    PrivacyStatus = "private"
            //}
        };

        var srCreateYouTubePlaylist = await _playlistServiceYoutube.CreatePlaylistAsync(youTubePlaylist);

        if (srCreateYouTubePlaylist.StatusCode.IsClientError() || srCreateYouTubePlaylist.Data is null)
        {
            serviceResult.Success = false;
            serviceResult.StatusCode = srCreateYouTubePlaylist.StatusCode;
            serviceResult.ErrorMessage = srCreateYouTubePlaylist.ErrorMessage;

            return serviceResult;
        }

        serviceResult.Data = new SyncResponse(new Domain.Models.Playlist(srGetSpotifyPlaylist.Data.Id, _spotifyLinkProvider.GetPlaylistLink(srGetSpotifyPlaylist.Data.Id)))
        {
            CreatedPlaylist = new Domain.Models.Playlist(srCreateYouTubePlaylist.Data.Id, _youtubeLinkProvider.GetPlaylistLink(srCreateYouTubePlaylist.Data.Id))
        };

        YouTubeModelsNS.SearchCriteria criteria = new(string.Empty, 50)
        {
            //I don't know, these parameters might be the reason of the wrong results.
            //SafeSearch = SafeSearchValue.None,
            //VideoCategoryId = VideoCategoryValue.Music,
            //VideoLicense = VideoLicenseValue.Youtube
        };

        YouTubeModelsNS.PlaylistItem youtubePlaylistItem = new()
        {
            Snippet = new()
            {
                PlaylistId = srCreateYouTubePlaylist.Data.Id,
                ResourceId = new()
                {
                    Kind = "youtube#video",
                    VideoId = string.Empty
                }
            }
        };

        foreach (var track in srGetSpotifyPlaylistItems.Data)
        {
            criteria.Q = CreateSearchQuery(track);
            var srSearchYouTubeVideo = await _searchServiceYoutube.SearchVideoAsync(criteria);

            if (srSearchYouTubeVideo.StatusCode.IsClientError())
            {
                //serviceResult.Success = false;
                //serviceResult.StatusCode = srSearchYouTubeVideo.StatusCode;
                //serviceResult.ErrorMessage = srSearchYouTubeVideo.ErrorMessage;
                serviceResult.Data.Failed.Add(new FailedSync(new Media(track.Id, _spotifyLinkProvider.GetTrackLink(track.Id), track.Name), srSearchYouTubeVideo.ErrorMessage));

                //return serviceResult;
                continue;
            }

            var srListYouTubeVideos = await _videoServiceYoutube.ListVideosByIdAsync(srSearchYouTubeVideo.Data.Select(item => item.Id.VideoId));

            if (srListYouTubeVideos.StatusCode.IsClientError())
            {
                //serviceResult.Success = false;
                //serviceResult.StatusCode = srListYouTubeVideos.StatusCode;
                //serviceResult.ErrorMessage = srListYouTubeVideos.ErrorMessage;
                serviceResult.Data.Failed.Add(new FailedSync(new Media(track.Id, _spotifyLinkProvider.GetTrackLink(track.Id), track.Name), srListYouTubeVideos.ErrorMessage));

                //return serviceResult;
                continue;
            }

            var matched = PickMatchedVideo(srListYouTubeVideos.Data, track);
            eşleşme bulunmazsa geçici bir listeye alınıp daha sonra yeni bir foreach ile tekrar dönülebilir... ama en iyisi ayrı bir servise al bu işleri
            if (matched.video is null)
            {
                //serviceResult.Success = false;
                //serviceResult.StatusCode = 404;
                //serviceResult.ErrorMessage = $"No matched video for track: {track.Name} | Id: {track.Id}. {matched.error}";
                serviceResult.Data.Failed.Add(new FailedSync(new Media(track.Id, _spotifyLinkProvider.GetTrackLink(track.Id), track.Name), $"No matched video for track: {track.Name} | Id: {track.Id}. {matched.error}"));

                //return serviceResult;
                continue;
            }

            youtubePlaylistItem.Snippet.ResourceId.VideoId = matched.video.Id;
            var srAddItemToYouTubePlaylist = await _playlistServiceYoutube.AddItemToPlaylistAsync(youtubePlaylistItem);

            if (srAddItemToYouTubePlaylist.StatusCode.IsClientError())
            {
                //serviceResult.Success = false;
                //serviceResult.StatusCode = srAddItemToYouTubePlaylist.StatusCode;
                //serviceResult.ErrorMessage = srAddItemToYouTubePlaylist.ErrorMessage;
                serviceResult.Data.Failed.Add(new FailedSync(new Media(track.Id, _spotifyLinkProvider.GetTrackLink(track.Id), track.Name), srAddItemToYouTubePlaylist.ErrorMessage));

                //return serviceResult;
                continue;
            }

            var addedItem = srAddItemToYouTubePlaylist.Data;
            var addedId = addedItem.Snippet.ResourceId.VideoId;

            serviceResult.Data.Successful.Add(new SuccessfulSync(
                new Media(track.Id, _spotifyLinkProvider.GetTrackLink(track.Id), track.Name),
                new Media(addedId, _youtubeLinkProvider.GetPlaylistItemLink(addedItem.Snippet.PlaylistId, addedId), addedItem.Snippet.Title)));
        }

        //Here, the status code does not have to be an error code...
        serviceResult.Success = true;
        serviceResult.StatusCode = 200;

        return serviceResult;
    }

    private static string CreateSearchQuery(SpotifyModelsNS.Track spotifyTrack)
    {
        //return $"{spotifyTrack.Name} {string.Join(' ', spotifyTrack.Artists.Take(3).Select(a => a.Name))}";
        return spotifyTrack.ExternalIds.Isrc;
    }

    private static (Video? video, string error) PickMatchedVideo(List<Video> videos, SpotifyModelsNS.Track track)
    {
        if (videos.Count == 0)
        {
            //sebepleri de dönülecek...
            return (null, "No videos were found in the YouTube search results.");
        }

        var filtered = videos.Where(v => Math.Abs(v.ContentDetails.Duration.ConvertIso8601ToMilliSeconds() - track.DurationMs) < 1500);

        if (!filtered.Any())
        {
            //sebepleri de dönülecek...
            return (null, "The duration of all YouTube videos found in the search result deviates from the duration of the Spotify track by more than 1.5 seconds.");
        }

        filtered = track.Name.Length < 8
            ? filtered.Where(v => string.Equals(v.Snippet.Title, track.Name, StringComparison.InvariantCultureIgnoreCase))
            : filtered.Where(v => v.Snippet.Title.ContainsNormalized(track.Name));

        if (!filtered.Any())
        {
            //sebepleri de dönülecek...
            return (null, "The normalized title of any YouTube video found in the search result does not include the normalized title of the Spotify track.");
        }

        filtered = filtered.Where(v => v.Snippet.Description.StartsWith("Provided to YouTube"));

        if (!filtered.Any())
        {
            //sebepleri de dönülecek...
            return (null, "None of the YouTube videos found in the search result have a description begin with the text \"Provided to YouTube\".");
        }

        filtered = filtered
            .Where(v => track.Artists
                .Any(artist => v.Snippet.Description.ContainsNormalized(artist.Name)
                    || v.Snippet.ChannelTitle.ContainsNormalized(artist.Name)));

        if (!filtered.Any())
        {
            //sebepleri de dönülecek...
            return (null, "The normalized description or channel title of any YouTube video found in the search result does not include any of the normalized name of the artists of the Spotify track.");
        }

        //Bring those with the closest duration to the top.
        filtered = filtered
            .OrderBy(v => Math.Abs(v.ContentDetails.Duration.ConvertIso8601ToMilliSeconds() - track.DurationMs))
            .ThenBy(v => v.ContentDetails.Duration.ConvertIso8601ToMilliSeconds());

        return (filtered.First(), string.Empty);
    }
}
