using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Events
{
    public abstract class BaseEvent
    {
        [Required]
        public Guid EventId { get; set; }
    }
}
