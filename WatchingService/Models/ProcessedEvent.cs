using System;
using System.ComponentModel.DataAnnotations;

namespace WatchingService.Models
{
    public class ProcessedEvent
    {
        [Key]
        public Guid EventId { get; set; }
    }
}
