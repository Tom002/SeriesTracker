using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Dto;
using ProfileService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IMapper _mapper;
        private readonly UserDbContext _context;

        public ProfileService(IMapper mapper, UserDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserProfileDto> GetUserProfile(string userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
