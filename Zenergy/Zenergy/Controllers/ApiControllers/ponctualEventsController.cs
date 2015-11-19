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

        // PUT: api/ponctualEvents/5
        [ResponseType(typeof(void))]
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
        [ResponseType(typeof(ponctualEvent))]
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