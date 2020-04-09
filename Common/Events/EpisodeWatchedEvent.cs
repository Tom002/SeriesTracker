using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class EpisodeWatchedEvent
    {
        public int EpisodeId { get; set; }

        public string ViewerId { get; set; }

        public DateTime WatchingDate { get; set; }

        public bool IsInDiary { get; set; }
    }
}
