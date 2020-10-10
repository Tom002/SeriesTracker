using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReviewService.Data;
using ReviewService.Models;
using System;
using System.Threading.Tasks;

namespace ReviewService.Services
{
    public class MessageTracker : IMessageTracker
    {
        private readonly ReviewDbContext _context;

        public MessageTracker(ReviewDbContext context)
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
