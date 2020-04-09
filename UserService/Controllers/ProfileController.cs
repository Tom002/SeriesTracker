using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Common.Events;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Dto;
using ProfileService.Models;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserDbContext _context;

        private readonly IMapper _mapper;

        public ProfileController(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        
        [CapSubscribe("watchingService.episodeWatched")]
        public async Task ReceiveEpisodeWatched(EpisodeWatchedEvent watchedEvent)
        {
            if(! await _context.DiaryEpisodes.AnyAsync(e => e.EpisodeId == watchedEvent.EpisodeId && e.UserId == watchedEvent.ViewerId))
            {
                if(watchedEvent.IsInDiary)
                {
                    var diaryEpisode = new EpisodeDiary
                    {
                        EpisodeId = watchedEvent.EpisodeId,
                        UserId = watchedEvent.ViewerId,
                        WatchingDate = watchedEvent.WatchingDate
                    };
                    _context.DiaryEpisodes.Add(diaryEpisode);
                    await _context.SaveChangesAsync();
                }
            }
        }


        [CapSubscribe("watchingService.seriesWatched.created")]
        public async Task ReceiveSeriesWatched(SeriesWatchedEvent watchedEvent)
        {
            if (!await _context.SeriesWatched.AnyAsync(sw => sw.UserId == watchedEvent.ViewerId && sw.SeriesId == watchedEvent.SeriesId))
            {
                var seriesWatched = new SeriesWatched
                {
                    SeriesId = watchedEvent.SeriesId,
                    UserId = watchedEvent.ViewerId
                };
                _context.SeriesWatched.Add(seriesWatched);
                await _context.SaveChangesAsync();
            }
        }


        [CapSubscribe("identityservice.user.created")]
        public async Task ReceiveUserCreated(UserCreatedEvent userEvent)
        {
            if(!await _context.Users.AnyAsync(user => user.UserId == userEvent.UserId))
            {
                var user = _mapper.Map<User>(userEvent);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        [CapSubscribe("series.created")]
        public async Task ReceiveSeriesCreated(SeriesCreatedEvent seriesEvent)
        {
            if (!await _context.Series.AnyAsync(series => series.SeriesId == seriesEvent.SeriesId))
            {
                _context.Series.Add(new Series { SeriesId = seriesEvent.SeriesId });
                await _context.SaveChangesAsync();
            }
        }

        [CapSubscribe("episode.created")]
        public async Task ReceiveEpisodeCreated(EpisodeCreatedEvent episodeEvent)
        {
            if (!await _context.Episodes.AnyAsync(episode => episode.EpisodeId == episodeEvent.EpisodeId))
            {
                _context.Episodes.Add(new Episode { EpisodeId = episodeEvent.EpisodeId, SeriesId = episodeEvent.SeriesId });
                await _context.SaveChangesAsync();
            }
        }


        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users
                .Include(user => user.SeriesWatched)
                .Include(user => user.SeriesLiked)
                .Include(user => user.EpisodeDiary)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if(user == null)
            {
                return NotFound();
            }
            else
            {
                var profile = new UserProfileDto
                {
                    UserId = user.UserId,
                    BirthDate = user.BirthDate,
                    City = user.City,
                    SeriesWatchedCount = user.SeriesWatched.Count(),
                    SeriesLikedCount = user.SeriesLiked.Count(),
                    EpisodeWatchedCount = user.EpisodeDiary.Count(),
                    Name = user.Name,
                    ProfileImageUrl = user.ProfileImageUrl
                };

                return Ok(profile);
            }
        }

        [HttpGet("{id}/seriesWatched")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Series>>> GetSeriesWatched(string id)
        {
            var user = await _context.Users
                .Include(user => user.SeriesWatched).ThenInclude(sw => sw.Series)
                .FirstOrDefaultAsync(user => user.UserId == id);

            if(user == null)
            {
                return NotFound();
            }
            else
            {
                var seriesWatched = user.SeriesWatched.ToList();
                return Ok(seriesWatched);
            }
        }

        [HttpGet("{id}/diary")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Series>>> GetUserDiary(string id)
        {
            var user = await _context.Users
                .Include(user => user.EpisodeDiary).ThenInclude(ew => ew.Episode)
                .FirstOrDefaultAsync(user => user.UserId == id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var episodesWatched = user.EpisodeDiary.ToList();
                return Ok(episodesWatched);
            }
        }

        [HttpGet("{id}/seriesLiked")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Series>>> GetSeriesLiked(string id)
        {
            var user = await _context.Users
                .Include(user => user.SeriesLiked).ThenInclude(sw => sw.Series)
                .FirstOrDefaultAsync(user => user.UserId == id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var seriesLiked = user.SeriesLiked.ToList();
                return Ok(seriesLiked);
            }
        }
    }
}