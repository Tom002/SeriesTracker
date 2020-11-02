using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class UserCreatedEvent : BaseEvent
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
