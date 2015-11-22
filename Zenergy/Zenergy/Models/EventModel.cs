using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zenergy.Models
{
    public class EventModel
    {
        public int eventId { get; set; }
        public int roomId { get; set; }
        public int activityId { get; set; }
        public string eventName { get; set; }
        public Nullable<decimal> eventPrice { get; set; }
        public Nullable<decimal> eventDurationHours { get; set; }
        public Nullable<int> eventMaxPeople { get; set; }
        public string eventDescription { get; set; }
        public Nullable<System.TimeSpan> timeBegin { get; set; }

        public DateTime eventDate { get; set; }

                      
    }

    
}