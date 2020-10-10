using Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Interfaces
{
    public interface ISubscriberService
    {
        public Task ReceiveUserCreated(UserCreatedEvent userEvent);
    }
}
