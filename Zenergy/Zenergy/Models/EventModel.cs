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

        public DateTime GetRegularEventDate(string day)
        {
            var today = DateTime.Today;
            var dayofweek = today.DayOfWeek;
            var eventdate = new DateTime();

            if (dayofweek.Equals(DayOfWeek.Monday))
            {
                switch (day)
                {
                    case "Monday":
                        eventdate = new DateTime(today.Year, today.Month, today.Day);
                        break;
                    case "Tuesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(1).Day);
                        break;
                    case "Wednesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(2).Day);
                        break;
                    case "Thursday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(3).Day);
                        break;
                    case "Friday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(4).Day);
                        break;
                    case "Saturday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(5).Day);
                        break;
                }
            }
            else if (dayofweek.Equals(DayOfWeek.Tuesday))
            {
                switch (day)
                {
                    case "Monday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-1).Day);
                        break;
                    case "Tuesday":
                        eventdate = new DateTime(today.Year, today.Month, today.Day);
                        break;
                    case "Wednesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(1).Day);
                        break;
                    case "Thursday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(2).Day);
                        break;
                    case "Friday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(3).Day);
                        break;
                    case "Saturday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(4).Day);
                        break;
                }
            }
            else if (dayofweek.Equals(DayOfWeek.Wednesday))
            {
                switch (day)
                {
                    case "Monday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-2).Day);
                        break;
                    case "Tuesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-1).Day);
                        break;
                    case "Wednesday":
                        eventdate = new DateTime(today.Year, today.Month, today.Day);
                        break;
                    case "Thursday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(1).Day);
                        break;
                    case "Friday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(2).Day);
                        break;
                    case "Saturday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(3).Day);
                        break;
                }
            }
            else if (dayofweek.Equals(DayOfWeek.Thursday))
            {
                switch (day)
                {
                    case "Monday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-3).Day);
                        break;
                    case "Tuesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-2).Day);
                        break;
                    case "Wednesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-1).Day);
                        break;
                    case "Thursday":
                        eventdate = new DateTime(today.Year, today.Month, today.Day);
                        break;
                    case "Friday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(1).Day);
                        break;
                    case "Saturday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(2).Day);
                        break;
                }
            }
            else if (dayofweek.Equals(DayOfWeek.Friday))
            {
                switch (day)
                {
                    case "Monday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-4).Day);
                        break;
                    case "Tuesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-3).Day);
                        break;
                    case "Wednesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-2).Day);
                        break;
                    case "Thursday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-1).Day);
                        break;
                    case "Friday":
                        eventdate = new DateTime(today.Year, today.Month, today.Day);
                        break;
                    case "Saturday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(1).Day);
                        break;
                }
            }
            else
            {
                switch (day)
                {
                    case "Monday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-5).Day);
                        break;
                    case "Tuesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-4).Day);
                        break;
                    case "Wednesday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-3).Day);
                        break;
                    case "Thursday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-2).Day);
                        break;
                    case "Friday":
                        eventdate = new DateTime(today.Year, today.Month, today.AddDays(-1).Day);
                        break;
                    case "Saturday":
                        eventdate = new DateTime(today.Year, today.Month, today.Day);
                        break;
                }
            }
            return eventdate;
        }
                
    }

    
}