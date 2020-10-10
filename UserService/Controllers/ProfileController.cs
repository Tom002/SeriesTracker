using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Dto;
using ProfileService.Interfaces;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(string id)
        {
            var userProfile = await _profileService.GetUserProfile(id);
            if(userProfile is UserProfileDto)
            {
                return Ok(userProfile);
            }
            return NotFound();
        }
    }
}