
using System;
using System.ComponentModel.DataAnnotations;

namespace ReviewService.Models
{
    public class ProcessedEvent
    {
        [Key]
        public Guid EventId { get; set; }
    }
}
