using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchingService.Dto.Email;

namespace WatchingService.Interfaces
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(Message message);
    }
}
