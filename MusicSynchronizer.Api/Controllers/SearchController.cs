using Microsoft.AspNetCore.Mvc;
using MusicSynchronizer.Domain.Interfaces.YouTube;
using MusicSynchronizer.Domain.Models.External.YouTube;

namespace MusicSynchronizer.Api.Controllers;

public class SearchController : BaseApiController
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    //[HttpGet]
    //public async Task<IActionResult> SearchInYouTube(string query, uint maxResults)
    //{
    //    var criteria = new SearchCriteria(query, maxResults);

    //    var awaited = await _searchService.SearchVideoAsync(criteria);

    //    return Ok(awaited);
    //}
}
