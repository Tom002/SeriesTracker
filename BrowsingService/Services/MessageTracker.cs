using BrowsingService.Data;
using BrowsingService.Models;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BrowsingService.Services
{
    public class MessageTracker : IMessageTracker
    {
        private readonly BrowsingDbContext _context;

        public MessageTracker(BrowsingDbContext context)
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
