using System;
using System.ComponentModel.DataAnnotations;

namespace BrowsingService.Models
{
    public class ProcessedEvent
    {
        [Key]
        public Guid EventId { get; set; }
    }
}
