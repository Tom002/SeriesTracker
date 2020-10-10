using Common.Events;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Helpers
{
    public static class MessageBusExtensions
    {
        public static async Task SendEvent(this ICapPublisher capPublisher, string eventName, BaseEvent eventObject)
        {
            eventObject.EventId = Guid.NewGuid();
            await capPublisher.PublishAsync(eventName, eventObject);
        }
    }
}
