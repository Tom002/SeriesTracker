using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class EpisodeWatchedEvent : BaseEvent
    {
        public int EpisodeId { get; set; }

        public string ViewerId { get; set; }
    }
}
