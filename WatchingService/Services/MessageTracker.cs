using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Common.Interfaces;
using WatchingService.Data;
using WatchingService.Models;

namespace WatchingService.Services
{
    public class MessageTracker : IMessageTracker
    {
        private readonly WatchingDbContext _context;

        public MessageTracker(WatchingDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasProcessed(Guid eventId)
        {
            return await _context.ProcessedEvents.AnyAsync(e => e.EventId == eventId);
        }

        public async Task MarkAsProcessed(Guid eventId)
        {
            var processedEvent = new ProcessedEvent
            {
                EventId = eventId
            };
            _context.ProcessedEvents.Add(processedEvent);
            await _context.SaveChangesAsync();
        }
    }
}
