﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class EpisodeCreatedEvent : BaseEvent
    {
        public int EpisodeId { get; set; }

        public int SeriesId { get; set; }

        public string EpisodeTitle { get; set; }

        public string Description { get; set; }

        public int Season { get; set; }

        public int EpisodeNumber { get; set; }

        public int LengthInMinutes { get; set; }

        public DateTime Release { get; set; }

        public bool IsReleased { get; set; }

        public string CoverImageUrl { get; set; }
    }
}
