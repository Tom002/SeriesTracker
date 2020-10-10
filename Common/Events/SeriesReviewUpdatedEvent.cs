using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class SeriesReviewUpdatedEvent : BaseEvent
    {
        public int SeriesId { get; set; }
        public string ReviewerId { get; set; }
        public string ReviewTitle { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewText { get; set; }
        public int? Rating { get; set; }
    }
}
