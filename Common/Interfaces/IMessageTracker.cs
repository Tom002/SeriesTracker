using System;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IMessageTracker
    {
        public Task<bool> HasProcessed(Guid eventId);

        public Task MarkAsProcessed(Guid eventId);
    }
}
