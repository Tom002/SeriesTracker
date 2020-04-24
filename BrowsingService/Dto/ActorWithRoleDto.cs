using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Dto
{
    public class ActorWithRoleDto
    {
        public int ArtistId { get; set; }

        public string Name { get; set; }

        public string RoleName { get; set; }

        public int Order { get; set; }

        public string ImageUrl { get; set; }
    }
}
