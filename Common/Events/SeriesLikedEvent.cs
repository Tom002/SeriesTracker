﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class SeriesLikedEvent : BaseEvent
    {
        public int SeriesId { get; set; }

        public string ViewerId { get; set; }
    }
}
