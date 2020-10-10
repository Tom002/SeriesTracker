using Common.Events;
using DotNetCore.CAP;
using System;
using System.Threading.Tasks;

namespace BrowsingService.Helpers
{
    public static class MessageBusExtensions
    {
        public static async Task SendEventAsync(this ICapPublisher capPublisher, string eventName, BaseEvent eventObject)
        {
            eventObject.EventId = Guid.NewGuid();
            await capPublisher.PublishAsync(eventName, eventObject);
        }

        public static void SendEvent(this ICapPublisher capPublisher, string eventName, BaseEvent eventObject)
        {
            eventObject.EventId = Guid.NewGuid();
            capPublisher.Publish(eventName, eventObject);
        }
    }
}
