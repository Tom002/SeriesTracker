using ProfileService.Dto;
using System.Threading.Tasks;

namespace ProfileService.Interfaces
{
    public interface IProfileService
    {
        public Task<UserProfileDto> GetUserProfile(string userId);
    }

}
