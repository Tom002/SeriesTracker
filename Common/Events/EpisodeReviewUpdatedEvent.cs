﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class EpisodeReviewUpdatedEvent : BaseEvent
    {
        public int EpisodeId { get; set; }
        public string ReviewerId { get; set; }
        public int Rating { get; set; }
    }
}
