using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class SeriesCreatedEvent
    {
        public int SeriesId { get; set; }

        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        public string Description { get; set; }
    }
}
