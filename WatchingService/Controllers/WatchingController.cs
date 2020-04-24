using System;
using System.Linq;
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

        private readonly ICapPublisher _capBus;

        public WatchingController(WatchingDbContext context, ICapPublisher capBus)
        {
            _context = context;
            _capBus = capBus;
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

        [CapSubscribe("browsingservice.series.created")]
        public async Task ReceiveSeriesCreated(SeriesCreatedEvent seriesEvent)
        {
            if (!await _context.Series.AnyAsync(series => series.SeriesId == seriesEvent.SeriesId))
            {
                _context.Series.Add(new Series { SeriesId = seriesEvent.SeriesId });
                await _context.SaveChangesAsync();
            }
        }

        [CapSubscribe("browsingservice.episode.created")]
        public async Task ReceiveEpisodeCreated(EpisodeCreatedEvent episodeEvent)
        {
            if (!await _context.Episode.AnyAsync(episode => episode.EpisodeId == episodeEvent.EpisodeId))
            {
                _context.Episode.Add(new Episode { EpisodeId = episodeEvent.EpisodeId, SeriesId = episodeEvent.SeriesId });
                await _context.SaveChangesAsync();
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{userId}/series/watched")]
        public async Task<ActionResult<SeriesWatchedListDto>> GetSeriesWatched(string userId)
        {
            if(!await _context.Viewer.AnyAsync(v => v.ViewerId == userId))
            {
                return NotFound("Searched user does not exist");
            }

            var seriesWatched = await _context.SeriesWatched.Where(s => s.ViewerId == userId).ToListAsync();
            var watchedListDto = new SeriesWatchedListDto
            {
                ViewerId = userId,
                SeriesWatchedIds = seriesWatched.Select(s => s.SeriesId).ToList()
            };
            return Ok(watchedListDto);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{userId}/series/{seriesId}/episodes")]
        public async Task<ActionResult<SeriesEpisodesWatchedListDto>> GetSeriesEpisodesWatched(string userId, int seriesId)
        {
            if (!await _context.Viewer.AnyAsync(v => v.ViewerId == userId))
            {
                return NotFound("Requested user does not exist");
            }

            if(!await _context.Series.AnyAsync(s => s.SeriesId == seriesId))
            {
                return NotFound("Requested series does not exist");
            }

            var episodesWatched = await _context.EpisodeWatched
                .Where(e => e.SeriesId == seriesId
                        &&
                       e.ViewerId == userId)
                .ToListAsync();

            var watchedListDto = new SeriesEpisodesWatchedListDto
            {
                ViewerId = userId,
                SeriesId = seriesId,
                EpisodesWatchedIds = episodesWatched.Select(e => e.EpisodeId).ToList()
            };
            return Ok(watchedListDto);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{userId}/series/liked")]
        public async Task<ActionResult<SeriesLikedListDto>> GetSeriesLiked(string userId)
        {
            if (!await _context.Viewer.AnyAsync(v => v.ViewerId == userId))
            {
                return NotFound("Searched user does not exist");
            }

            var seriesLiked = await _context.SeriesLiked.Where(s => s.ViewerId == userId).ToListAsync();
            var likedListDto = new SeriesLikedListDto
            {
                ViewerId = userId,
                SeriesLikedIds = seriesLiked.Select(s => s.SeriesId).ToList()
            };
            return Ok(likedListDto);
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPost("series")]
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

            using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
            {
                try
                {
                    _context.SeriesWatched.Add(seriesWatched);
                    await _context.SaveChangesAsync();

                    var seriesWatchedEvent = new SeriesWatchedEvent
                    {
                        SeriesId = seriesWatched.SeriesId,
                        ViewerId = seriesWatched.ViewerId
                    };
                    await _capBus.PublishAsync("watchingService.seriesWatched.created", seriesWatchedEvent);
                    await trans.CommitAsync();
                    return NoContent();
                }
                catch (System.Exception)
                {
                    await trans.RollbackAsync();
                    return BadRequest("Error while saving watched series");
                }
            }
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPost("episode")]
        public async Task<ActionResult> WatchEpisode([FromBody] WatchEpisodeDto watchEpisodeDto)
        {
            var signedInUserId = User.FindFirst("sub").Value;
            if (watchEpisodeDto.ViewerId != signedInUserId)
            {
                return Unauthorized();
            }
            var user = await _context.Viewer.FirstOrDefaultAsync(viewer => viewer.ViewerId == watchEpisodeDto.ViewerId);
            var episode = await _context.Episode
                .FirstOrDefaultAsync(episode => episode.EpisodeId == watchEpisodeDto.EpisodeId
                                    &&
                                    episode.SeriesId == watchEpisodeDto.SeriesId);
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
                IsInDiary = watchEpisodeDto.AddToDiary,
                SeriesId = watchEpisodeDto.SeriesId
            };

            using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
            {
                try
                {
                    _context.EpisodeWatched.Add(episodeWatched);

                    if (!await _context.SeriesWatched.AnyAsync(sw =>
                        sw.SeriesId == episode.SeriesId &&
                        sw.ViewerId == watchEpisodeDto.ViewerId))
                    {
                        var seriesWatched = new SeriesWatched { SeriesId = episode.SeriesId, ViewerId = watchEpisodeDto.ViewerId };
                        _context.SeriesWatched.Add(seriesWatched);
                        var seriesWatchedEvent = new SeriesWatchedEvent
                        {
                            SeriesId = seriesWatched.SeriesId,
                            ViewerId = seriesWatched.ViewerId
                        };
                        await _capBus.PublishAsync("watchingService.seriesWatched.created", seriesWatchedEvent);
                    }

                    var episodeWatchedEvent = new EpisodeWatchedEvent
                    {
                        ViewerId = episodeWatched.ViewerId,
                        EpisodeId = episodeWatched.EpisodeId,
                        WatchingDate = episodeWatched.WatchingDate,
                        IsInDiary = episodeWatched.IsInDiary
                    };

                    if(await _context.SaveChangesAsync() > 0)
                    {
                        await _capBus.PublishAsync("watchingService.episodeWatched", episodeWatchedEvent);
                        await trans.CommitAsync();
                        return NoContent();
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    return BadRequest("Error while saving watched episode");
                }
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