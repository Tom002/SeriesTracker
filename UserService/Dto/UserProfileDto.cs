using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Dto
{
    public class UserProfileDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
        public string About { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
