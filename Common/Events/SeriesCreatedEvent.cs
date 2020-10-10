using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class SeriesCreatedEvent : BaseEvent
    {
        public int SeriesId { get; set; }

        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }
    }
}
