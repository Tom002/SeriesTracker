using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchingService.Dto;
using WatchingService.Helpers;
using WatchingService.Helpers.Pagination;
using WatchingService.Helpers.RequestContext;
using WatchingService.Interfaces;
using WatchingService.Models;

namespace WatchingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchingController : ControllerBase
    {
        private readonly IWatchingService _watchingService;
        private readonly IRequestContext _requestContext;

        public WatchingController(IWatchingService watchingService,
                                  IRequestContext requestContext)
        {
            _watchingService = watchingService;
            _requestContext = requestContext;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("viewers/{userId}/series/watched")]
        public async Task<ActionResult<PagedList<SeriesWatchedListDto>>> 
            GetSeriesWatched(string userId, [FromQuery] PagingParams pagingParams)
        {
            var viewer = await _watchingService.GetViewer(userId);
            if(viewer is Viewer)
            {
                var seriesWatchedList = await _watchingService.GetSeriesWatchedList(viewer, pagingParams);
                Response.AddPagination(seriesWatchedList.CurrentPage,
                                       seriesWatchedList.PageSize,
                                       seriesWatchedList.TotalCount,
                                       seriesWatchedList.TotalPages);
                return Ok(seriesWatchedList);
            }
            else
            {
                return NotFound();
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("viewers/{userId}/series/liked")]
        public async Task<ActionResult<PagedList<SeriesLikedListDto>>>
            GetSeriesLiked(string userId, [FromQuery] PagingParams pagingParams)
        {
            var viewer = await _watchingService.GetViewer(userId);
            if (viewer is Viewer)
            {
                var seriesLikedList = await _watchingService.GetSeriesLikedList(viewer, pagingParams);
                Response.AddPagination(seriesLikedList.CurrentPage,
                                       seriesLikedList.PageSize,
                                       seriesLikedList.TotalCount,
                                       seriesLikedList.TotalPages);
                return Ok(seriesLikedList);
            }
            else
            {
                return NotFound();
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize]
        [HttpGet("series/{seriesId}/myWatching")]
        public async Task<ActionResult<ViewerSeriesWatchedInfoDto>> GetSeriesWatchedInfo(int seriesId)
        {
            var viewerId = _requestContext.UserId;
            var series = await _watchingService.GetSeries(seriesId);
            if(series is Series)
            {
                var seriesWatchedInfo = await _watchingService.GetSeriesWatchedInfo(series, viewerId);
                return Ok(seriesWatchedInfo);
            }
            else
            {
                return NotFound();
            }
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize]
        [HttpPost("series/{seriesId}/watch")]
        public async Task<ActionResult> WatchSeries(int seriesId)
        {
            var userId = _requestContext.UserId;
            var series = await _watchingService.GetSeries(seriesId);
            if(series is Series)
            {
                var isSeriesWatched = await _watchingService.IsSeriesWatchedByViewer(seriesId, userId);
                if(isSeriesWatched)
                {
                    return BadRequest("You are already watching this series");
                }

                var seriesWatchedCreatedSuccess = await _watchingService.CreateSeriesWatched(series, userId);
                if (seriesWatchedCreatedSuccess)
                    return NoContent();
                return BadRequest("Unexpected error while saving series watched");
            }
            else
            {
                return NotFound();
            }
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize]
        [HttpDelete("series/{seriesId}/watch")]
        public async Task<ActionResult> DeleteSeriesWatched(int seriesId)
        {
            var userId = _requestContext.UserId;
            var series = await _watchingService.GetSeries(seriesId);
            if (series is Series)
            {
                var seriesWatched = await _watchingService.GetSeriesWatched(series, userId);
                if (seriesWatched is SeriesWatched)
                {
                    var seriesWatchedDeletedSuccess = await _watchingService.DeleteSeriesWatched(seriesWatched);
                    if (seriesWatchedDeletedSuccess)
                        return NoContent();
                    return BadRequest("Unexpected error while deleting series watched");
                }
                else
                {
                    return BadRequest("You have not watched this series");
                }
            }
            else
            {
                return NotFound();
            }
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize]
        [HttpPost("episode/{episodeId}/watch")]
        public async Task<ActionResult> WatchEpisode(int episodeId)
        {
            var userId = _requestContext.UserId;
            var episode = await _watchingService.GetEpisode(episodeId);
            if(episode is Episode)
            {
                if(await _watchingService.IsEpisodeWatchedByViewer(episode, userId))
                {
                    return BadRequest("You have already watched this episode");
                }

                var episodeWatchedCreatedSuccess = await _watchingService.CreateEpisodeWatched(episode, userId);
                if (episodeWatchedCreatedSuccess)
                    return NoContent();
                return BadRequest("Unexpected error while saving episode watched");
            }
            else
            {
                return NotFound();
            }

            
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize]
        [HttpDelete("episode/{episodeId}/watch")]
        public async Task<ActionResult> DeleteEpisodeWatched(int episodeId)
        {
            var userId = _requestContext.UserId;
            var episode = await _watchingService.GetEpisode(episodeId);
            if (episode is Episode)
            {
                var episodeWatched = await _watchingService.GetEpisodeWatched(episode, userId);
                if (episodeWatched is EpisodeWatched)
                {
                    var episodeWatchedDeletedSuccess = await _watchingService.DeleteEpisodeWatched(episodeWatched);
                    if (episodeWatchedDeletedSuccess)
                        return NoContent();
                    return BadRequest("Unexpected error while deleting episode watched");
                }
                else
                {
                    return BadRequest("You didn't watch this episode");
                }
            }
            else
            {
                return NotFound();
            }
        }

        
        
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize]
        [HttpPost("series/{seriesId}/like")]
        public async Task<ActionResult> LikeSeries(int seriesId)
        {
            var userId = _requestContext.UserId;
            var series = await _watchingService.GetSeries(seriesId);
            
            if(series is Series)
            {
                if(await _watchingService.IsSeriesLikedByViewer(seriesId, userId))
                {
                    return BadRequest("You have already liked this series");
                }

                var seriesLikedCreatedSucces = await _watchingService.CreateSeriesLiked(series, userId);
                if (seriesLikedCreatedSucces)
                    return NoContent();
                return BadRequest("Unexpected error while saving series liked");

            }
            else
            {
                return NotFound();
            }
        }

        
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize]
        [HttpDelete("series/{seriesId}/like")]
        public async Task<ActionResult> DeleteSeriesLiked(int seriesId)
        {
            var userId = _requestContext.UserId;
            var series = await _watchingService.GetSeries(seriesId);

            if (series is Series)
            {
                var seriesLiked = await _watchingService.GetSeriesLiked(series, userId);
                if (seriesLiked is SeriesLiked)
                {
                    var seriesLikedDeletedSuccess = await _watchingService.DeleteSeriesLiked(seriesLiked);
                    if (seriesLikedDeletedSuccess)
                        return NoContent();
                    return BadRequest("Unexpected error while deleting liked series");
                }
                else
                {
                    return BadRequest("You didnt like this series");
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}