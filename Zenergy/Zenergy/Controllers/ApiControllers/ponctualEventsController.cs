using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenergy.Models;
using Zenergy.Services;

namespace Zenergy.Controllers.ApiControllers
{
    public class ponctualEventsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();
        private EventService eventServices;

        public ponctualEventsController()
        {
            eventServices = new EventService(db);
        }

        // GET: api/ponctualEvents
        public IQueryable<ponctualEvent> GetponctualEvent()
        {
            return db.ponctualEvent;
        }

        // GET: api/ponctualEvents/5
        [ResponseType(typeof(ponctualEvent))]
        public async Task<IHttpActionResult> GetponctualEvent(int id)
        {
            ponctualEvent ponctualEvent = await db.ponctualEvent.FindAsync(id);
            if (ponctualEvent == null)
            {
                return NotFound();
            }

            return Ok(ponctualEvent);
        }

        // GET: api/ponctualEvents/findByManagerId/5
        [Route("api/ponctualEvents/findByManagerId/{managerId}")]
        [HttpGet]
        [ResponseType(typeof(ponctualEvent[]))]
        public async Task<IHttpActionResult> findPonctualEventsByManagerId(int managerId)
        {
            ponctualEvent[] ponctualEvents = await eventServices.findPonctualEventsByManagerId(managerId);
            if (ponctualEvents == null)
            {
                return NotFound();
            }

            return Ok(ponctualEvents);
        }

        // PUT: api/ponctualEvents/5
        [HttpPut]
        [Route("api/ponctualEvents/{id}")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Manager")]
        public async Task<IHttpActionResult> PutponctualEvent(int id, ponctualEvent ponctualEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ponctualEvent.eventId)
            {
                return BadRequest();
            }

            //db.Entry(ponctualEvent.@event).State = EntityState.Modified;     
            db.Entry(ponctualEvent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ponctualEventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ponctualEvents
        [Authorize(Roles = "Manager")]
        [ResponseType(typeof(ponctualEvent))]
        public async Task<IHttpActionResult> PostponctualEvent(ponctualEvent ponctualEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ponctualEvent.Add(ponctualEvent);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ponctualEventExists(ponctualEvent.eventId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = ponctualEvent.eventId }, ponctualEvent);
        }

        // DELETE: api/ponctualEvents/5
        [HttpDelete]
        [Route("api/ponctualEvents/{id}")]
        [ResponseType(typeof(ponctualEvent))]
        [Authorize(Roles = "Manager")]
        public async Task<IHttpActionResult> DeleteponctualEvent(int id)
        {
            ponctualEvent ponctualEvent = await db.ponctualEvent.FindAsync(id);
            if (ponctualEvent == null)
            {
                return NotFound();
            }

            db.ponctualEvent.Remove(ponctualEvent);
            await db.SaveChangesAsync();

            return Ok(ponctualEvent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ponctualEventExists(int id)
        {
            return db.ponctualEvent.Count(e => e.eventId == id) > 0;
        }
    }
}