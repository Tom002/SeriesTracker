using Common.Events;
using DotNetCore.CAP;
using System;
using System.Threading.Tasks;

namespace WatchingService.Helpers
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
