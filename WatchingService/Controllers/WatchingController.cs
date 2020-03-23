using System.Threading.Tasks;
using Common.Events;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchingService.Data;
using WatchingService.Dto;
using WatchingService.Models;

namespace WatchingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchingController : ControllerBase
    {
        private readonly WatchingDbContext _context;

        public WatchingController(WatchingDbContext context)
        {
            _context = context;
        }

        [CapSubscribe("identityservice.user.created")]
        public async Task ReceiveUserCreated(UserCreatedEvent userEvent)
        {
            if(!await _context.Viewer.AnyAsync(viewer => viewer.ViewerId == userEvent.UserId))
            {
                _context.Viewer.Add(new Viewer { ViewerId = userEvent.UserId });
                await _context.SaveChangesAsync();
            }
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPost("watch/series")]
        public async Task<ActionResult> WatchSeries(WatchSeriesDto watchSeriesDto)
        {
            var signedInUserId = User.FindFirst("sub").Value;
            if (watchSeriesDto.ViewerId != signedInUserId)
            {
                return Unauthorized();
            }

            var user = await _context.Viewer.FirstOrDefaultAsync(viewer => viewer.ViewerId == watchSeriesDto.ViewerId);
            var series = await _context.Series.FirstOrDefaultAsync(viewer => viewer.SeriesId == watchSeriesDto.SeriesId);
            if (user == null || series == null)
            {
                return NotFound();
            }
            if (await _context.SeriesWatched.AnyAsync(sw => 
                sw.SeriesId == watchSeriesDto.SeriesId && 
                sw.ViewerId == watchSeriesDto.ViewerId))
            {
                return BadRequest("You are already watching this series");
            }

            var seriesWatched = new SeriesWatched { SeriesId = watchSeriesDto.SeriesId, ViewerId = watchSeriesDto.ViewerId };
            _context.SeriesWatched.Add(seriesWatched);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Error while saving watched series");
            }
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPost("watch/episode")]
        public async Task<ActionResult> WatchEpisode([FromBody] WatchEpisodeDto watchEpisodeDto)
        {
            var signedInUserId = User.FindFirst("sub").Value;
            if (watchEpisodeDto.ViewerId != signedInUserId)
            {
                return Unauthorized();
            }
            var user = await _context.Viewer.FirstOrDefaultAsync(viewer => viewer.ViewerId == watchEpisodeDto.ViewerId);
            var episode = await _context.Episode.FirstOrDefaultAsync(episode => episode.EpisodeId == watchEpisodeDto.EpisodeId);
            if (user == null || episode == null)
            {
                return NotFound();
            }
            if (await _context.EpisodeWatched.AnyAsync(ew => 
                ew.ViewerId == watchEpisodeDto.ViewerId && 
                ew.EpisodeId == watchEpisodeDto.EpisodeId))
            {
                return BadRequest("You have already watched this episode");
            }

            var episodeWatched = new EpisodeWatched
            {
                EpisodeId = watchEpisodeDto.EpisodeId,
                ViewerId = watchEpisodeDto.ViewerId,
                WatchingDate = watchEpisodeDto.WatchingDate,
                IsInDiary = watchEpisodeDto.AddToDiary
            };
            _context.EpisodeWatched.Add(episodeWatched);

            if (!await _context.SeriesWatched.AnyAsync(sw => 
                sw.SeriesId == episode.SeriesId &&
                sw.ViewerId == watchEpisodeDto.ViewerId))
            {
                var seriesWatched = new SeriesWatched { SeriesId = episode.SeriesId, ViewerId = watchEpisodeDto.ViewerId };
                _context.SeriesWatched.Add(seriesWatched);
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Error while saving watched episode");
            }
        }

        [HttpPost("like/series")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> LikeSeries(LikeSeriesDto likeSeriesDto)
        {
            var signedInUserId = User.FindFirst("sub").Value;
            if (likeSeriesDto.ViewerId != signedInUserId)
            {
                return Unauthorized();
            }

            var user = await _context.Viewer.FirstOrDefaultAsync(viewer => viewer.ViewerId == likeSeriesDto.ViewerId);
            var series = await _context.Series.FirstOrDefaultAsync(series => series.SeriesId == likeSeriesDto.SeriesId);
            if (user == null || series == null)
            {
                return NotFound();
            }
            if (await _context.SeriesLiked.AnyAsync(sl => 
                sl.SeriesId == likeSeriesDto.SeriesId &&
                sl.ViewerId == likeSeriesDto.ViewerId))
            {
                return BadRequest("You have already liked this series");
            }

            var seriesLiked = new SeriesLiked { SeriesId = likeSeriesDto.SeriesId, ViewerId = likeSeriesDto.ViewerId };
            _context.SeriesLiked.Add(seriesLiked);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Could not like the series");
            }
        }
    }
}