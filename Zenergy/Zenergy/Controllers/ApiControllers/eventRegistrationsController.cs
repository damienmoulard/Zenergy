using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Zenergy.Models;
using System.Web.Http.Results;

using Zenergy.Services;

namespace Zenergy.Controllers.ApiControllers
{
   // [RoutePrefix("api/events")]
    public class eventRegistrationsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();


        // GET: api/events
        [HttpGet]
        [Route("api/events")]
        [Authorize(Roles = "Admin, Manager")]
        public IQueryable<@event> GetAllEventRegistrations()
        {
            return db.@event.Where(e => e.user.Count != 0);
        }


        //GET: api/events/GetRegistrationsByEvent?eventId=1
        [HttpGet]
        [ResponseType(typeof(EventRegistrationModel))]
        [Authorize(Roles = "Admin, Manager")]
        [Route("api/eventsregistration/{eventId}")]
        public async Task<IHttpActionResult> GetRegistrationsByEvent(int eventId)
        {
            var myEvent = db.@event.Where(e => e.eventId == eventId && e.user.Count != 0);
            if (!myEvent.Any())
            {
                return NotFound();
            }
            var registeredUsers = myEvent.FirstAsync().Result.user.ToList();
            return Ok(new EventRegistrationByEventModel() { eventId = eventId, registeredUsers = registeredUsers });
        }


        [HttpGet]
        [ResponseType(typeof(@event))]
        [Route("api/events/byactivity")]
        [Authorize(Roles = "Manager, Member")]
        public IOrderedQueryable<@event> SortEventsByActivity()
        {
            return db.@event.OrderBy(a => a.activity.activityName);
        }


        [HttpPost]
        [ResponseType(typeof(List<EventModel>))]
        [Authorize(Roles = "Manager, Member")]
        [Route("api/events/bydate")]
        public async Task<IHttpActionResult> SortEventsByDate(EventDateModel filter)
        {
            var datefilter = filter.eventdate;
            var sortedEvents = new List<EventModel>();
            //Sorting punctual events;
            var punctualEvents = await db.ponctualEvent.Where(pe => pe.eventDate == datefilter).ToListAsync();
            if (punctualEvents.Any())
            {
                foreach (ponctualEvent pe in punctualEvents)
                {
                    //creating EventModel from punctaul events
                    var eventmodel = new EventModel()
                    {
                        eventId = pe.eventId,
                        activityId = pe.@event.activityId,
                        eventDate = pe.eventDate.Value,
                        eventDescription = pe.@event.eventDescription,
                        eventDurationHours = pe.@event.eventDurationHours,
                        eventMaxPeople = pe.@event.eventMaxPeople,
                        eventName = pe.@event.eventName,
                        eventPrice = pe.@event.eventPrice,
                        roomId = pe.@event.roomId
                    };
                    sortedEvents.Add(eventmodel);
                }
            }

            //sorting regular events by day
            var regularEvents = new List<regularEvent>();
            if (datefilter.DayOfWeek.Equals(DayOfWeek.Monday))
            {
                regularEvents = await db.regularEvent.Where(re => re.dateDay.Equals("Monday")).ToListAsync();
            }
            else if (datefilter.Day.Equals(DayOfWeek.Tuesday))
            {
                regularEvents = await db.regularEvent.Where(re => re.dateDay.Equals("Tuesday")).ToListAsync();
            }
            else if (datefilter.Day.Equals(DayOfWeek.Wednesday))
            {
                regularEvents = await db.regularEvent.Where(re => re.dateDay.Equals("Wednesday")).ToListAsync();
            }
            else if (datefilter.Day.Equals(DayOfWeek.Thursday))
            {
                regularEvents = await db.regularEvent.Where(re => re.dateDay.Equals("Thursday")).ToListAsync();
            }
            else if (datefilter.Day.Equals(DayOfWeek.Friday))
            {
                regularEvents = await db.regularEvent.Where(re => re.dateDay.Equals("Friday")).ToListAsync();
            }
            else if (datefilter.Day.Equals(DayOfWeek.Saturday))
            {
                regularEvents = await db.regularEvent.Where(re => re.dateDay.Equals("Saturday")).ToListAsync();
            }

            if (regularEvents.Any())
            {
                foreach (regularEvent re in regularEvents)
                {
                    //creating EventModel from punctaul events
                    var eventmodel = new EventModel();
                    {
                        eventmodel.eventDate = datefilter;
                        eventmodel.eventId = re.eventId;
                        eventmodel.activityId = re.@event.activityId;
                        eventmodel.eventDescription = re.@event.eventDescription;
                        eventmodel.eventDurationHours = re.@event.eventDurationHours;
                        eventmodel.eventMaxPeople = re.@event.eventMaxPeople;
                        eventmodel.eventName = re.@event.eventName;
                        eventmodel.eventPrice = re.@event.eventPrice;
                        eventmodel.roomId = re.@event.roomId;
                    }
                    sortedEvents.Add(eventmodel);
                }
            }

            return Ok(sortedEvents);
        }



        [HttpGet]
        [ResponseType(typeof(@event))]
        [Route("api/users/{userId}/events")]
        [Authorize(Roles = "Member")]
        public async Task<IHttpActionResult> GetMyEvent(int userId)
        {
            var myUser = await db.user.FindAsync(userId);
            if(myUser == null)
            {
                return NotFound();
            }
            if (!myUser.@event.Any())
            {
                return BadRequest("User has not registered to event yet.");
            }
            return Ok(myUser.@event);
        }

        [HttpGet]
        [ResponseType(typeof(@event))]
        [Route("SortUserEventByActivity")]
        [Authorize(Roles = "Member")]
        public async Task<IHttpActionResult> SortUserEventByActivity(int userId)
        {
            var user = await db.user.FindAsync(userId);
            if(user == null)
            {
                return BadRequest("This user does not exist.");
            }
            if (!user.@event.Any())
            {
                return BadRequest("User has not registered to event yet.");
            }
            return Ok(user.@event.OrderBy(e => e.activity.activityName));
        }


        /// <summary>
        /// Register user to the event.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/events/PostRegisterToEvent
        [HttpPost]
        [ResponseType(typeof(EventRegistrationModel))]
        [Authorize(Roles = "Member")]
        [Route("api/users/{userId}/events/{eventId}/registration")]
        public async Task<IHttpActionResult> PostRegisterToEvent(int userId, int eventId)
        {
            if (EventExists(eventId))
            {
                if (!UserAlreadyRegisteredToEvent(userId, eventId))
                {
                    var myUser = await db.user.Where(u => u.userId == userId).FirstAsync();
                    var myEvent = await db.@event.Where(e => e.eventId == eventId).FirstAsync();

                    try
                    {
                        myEvent.user.Add(myUser);
                        db.Entry(myEvent).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return Created("api/users/{userId}/events/{eventId}/registration", new EventRegistrationModel() {userId = userId, eventId = eventId});
                    }
                    catch (DbUpdateException)
                    {
                        return BadRequest("Sorry an error occured. Please try again.");
                    }
                }
                else return BadRequest("You are already registered to this event");
            }
            else return BadRequest("This event does not exist.");

        }


        /// <summary>
        /// Unsubscribe a user to an event.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        // DELETE: api/events/DeleteRegistration
        [HttpDelete]
        [ResponseType(typeof(EventRegistrationModel))]
        [Authorize(Roles = "Member")]
        [Route("api/delete/users/{userId}/events/{eventId}/registration")]
        public async Task<IHttpActionResult> DeleteRegistration(int eventId, int userId)
        {
            if (EventExists(eventId))
            {
                if(UserAlreadyRegisteredToEvent(userId, eventId))
                {
                    var myEvent = await db.@event.FindAsync(eventId);
                    var myUser = await db.user.FindAsync(userId);
                    myEvent.user.Remove(myUser);
                    await db.SaveChangesAsync();
                    return Ok(new EventRegistrationModel() { eventId = eventId, userId = userId });
                }
                return BadRequest("You are not registered to this event.");
            }
            return BadRequest("The event does not exist.");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool EventExists(int eventId)
        {
            return db.@event.Where(e => e.eventId == eventId).Any();
        }

        public bool UserAlreadyRegisteredToEvent(int userId, int eventId)
        {
            return db.@event.Where(e => e.eventId == eventId).First().user.Where(u => u.userId == userId).Any(); 
        }


    }
}