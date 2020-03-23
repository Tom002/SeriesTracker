using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Models;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<List<Artist>>> GetArtists()
        {
            var artists = await _context.Artists.ToListAsync();
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