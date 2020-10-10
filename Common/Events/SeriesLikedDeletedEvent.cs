using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class SeriesLikedDeletedEvent : BaseEvent
    {
        public int SeriesId { get; set; }

        public string ViewerId { get; set; }
    }
}
