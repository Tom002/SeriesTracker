using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Dto;
using BrowsingService.Helpers;
using BrowsingService.Helpers.Pagination;
using BrowsingService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrowsingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly BrowsingDbContext _context;

        private readonly IMapper _mapper;

        public ArtistController(BrowsingDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Artist>>> GetArtists([FromQuery]ArtistParams artistParams)
        {
            var artistQuery = _context.Artists
                .Include(a => a.AppearedIn)
                .Include(a => a.WriterOf)
                .AsQueryable();

            if(!String.IsNullOrEmpty(artistParams.NameFilter))
            {
                artistQuery = artistQuery.Where(a => a.Name.Contains(artistParams.NameFilter));
            }
            switch (artistParams.Sort)
            {
                case ArtistParams.SortBy.Alphabetical:
                    artistQuery = artistQuery.OrderBy(a => a.Name);
                    break;
                case ArtistParams.SortBy.AlphabeticalDescending:
                    artistQuery = artistQuery.OrderByDescending(a => a.Name);
                    break;
            }

            switch (artistParams.Occupation)
            {
                case ArtistParams.ActorWriter.Actor:
                    artistQuery = artistQuery.Where(a => a.AppearedIn.Any());
                    break;
                case ArtistParams.ActorWriter.Writer:
                    artistQuery = artistQuery.Where(a => a.WriterOf.Any());
                    break;
                case ArtistParams.ActorWriter.Both:
                    artistQuery = artistQuery.Where(a => a.WriterOf.Any() && a.AppearedIn.Any());
                    break;
                case ArtistParams.ActorWriter.Any:
                    break;
                default:
                    break;
            }

            var artistToList = artistQuery
                .Select(a => new ArtistForListDto 
                {
                    ArtistId = a.ArtistId,
                    BirthDate = a.BirthDate,
                    DeathDate = a.DeathDate,
                    AppearedInCount = a.AppearedIn.Count(),
                    WriterOfCount = a.WriterOf.Count(),
                    City = a.City,
                    Name = a.Name,
                    ImageUrl = a.ImageUrl
                });
            var artists = await PagedList<ArtistForListDto>.CreateAsync(artistToList, artistParams.PageNumber, artistParams.PageSize);
            Response.AddPagination(artistParams.PageNumber, artistParams.PageSize, artists.TotalCount, artists.TotalPages);
            return Ok(artists);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Artist>> GetSingleArtist(int id)
        {
            var artist = await _context.Artists
                .Include(a => a.AppearedIn)
                .Include(a => a.WriterOf)
                .Where(a => a.ArtistId == id)
                .FirstOrDefaultAsync();

            if (artist == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(artist);
            }
        }
    }
}