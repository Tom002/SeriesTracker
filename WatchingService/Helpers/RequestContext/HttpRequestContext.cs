using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Helpers.RequestContext
{
    public class HttpRequestContext : IRequestContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string UserId { get; set; }

        public HttpRequestContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                UserId = _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
            }
        }
    }
}
