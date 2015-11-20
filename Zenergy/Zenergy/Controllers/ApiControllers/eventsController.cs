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
    public class eventsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();


        // POST: api/regularEvents/RegisterToEvent
        [ActionName("Register")]
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