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
                        return CreatedAtRoute("DefaultApi", new { id = userId }, new EventRegistrationModel() {userId = userId, eventId = eventId});
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