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

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users
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
                    Name = user.Name,
                    ProfileImageUrl = user.ProfileImageUrl
                };

                return Ok(profile);
            }
        }
    }
}