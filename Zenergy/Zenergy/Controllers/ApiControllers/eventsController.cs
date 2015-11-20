﻿using System;
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
    [RoutePrefix("api/events")]
    public class eventsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();


        // GET: api/events/GetAllEventRegistrations
        [HttpGet]
        [ActionName("GetAllEventRegistration")]
        [Route("GetAllEventRegistration")]
        public IQueryable<@event> GetAllEventRegistrations()
        {
            return db.@event.Where(e => e.user.Count != 0);
        }


        //GET: api/events/GetRegistrationsByEvent?eventId=1
        [HttpGet]
        [ResponseType(typeof(EventRegistrationModel))]
        [ActionName("GetRegistrationByEvent")]

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
        [ActionName("SortByActivity")]
        [Route("SortByActivity")]
        public IOrderedQueryable<@event> SortEventsByActivity()
        {
            return db.@event.OrderBy(a => a.activity.activityName);
        }


        [HttpGet]
        [ResponseType(typeof(@event))]
        [Route("GetMyEvents")]
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



        /// <summary>
        /// Register user to the event.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/events/PostRegisterToEvent
        [HttpPost]
        [ResponseType(typeof(EventRegistrationModel))]
        public async Task<IHttpActionResult> PostRegisterToEvent(EventRegistrationModel model)
        {
            if (EventExists(model.eventId))
            {
                if (!UserAlreadyRegisteredToEvent(model.userId, model.eventId))
                {
                    var myUser = await db.user.Where(u => u.userId == model.userId).FirstAsync();
                    var myEvent = await db.@event.Where(e => e.eventId == model.eventId).FirstAsync();

                    try
                    {
                        myEvent.user.Add(myUser);
                        db.Entry(myEvent).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return CreatedAtRoute("DefaultApi", new { id = model.userId }, model);
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