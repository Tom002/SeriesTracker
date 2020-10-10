using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Events
{
    public class SeriesReviewCreatedEvent : BaseEvent
    {
        public int SeriesId { get; set; }
        public string ReviewerId { get; set; }
        public string ReviewTitle { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewText { get; set; }
        public int? Rating { get; set; }
    }
}
