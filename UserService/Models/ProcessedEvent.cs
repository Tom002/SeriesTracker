using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class ProcessedEvent
    {
        [Key]
        public Guid EventId { get; set; }
    }
}
