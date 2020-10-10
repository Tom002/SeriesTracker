using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Helpers.RequestContext
{
    public interface IRequestContext
    {
        public string UserId { get; set; }
    }
}
