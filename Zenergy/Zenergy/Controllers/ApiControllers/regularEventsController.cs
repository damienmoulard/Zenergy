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

namespace Zenergy.Controllers.ApiControllers
{
    public class regularEventsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();

        // GET: api/regularEvents
        public IQueryable<regularEvent> GetregularEvent()
        {
            return db.regularEvent;
        }

        // GET: api/regularEvents/5
        [ResponseType(typeof(regularEvent))]
        public async Task<IHttpActionResult> GetregularEvent(int id)
        {
            regularEvent regularEvent = await db.regularEvent.FindAsync(id);
            if (regularEvent == null)
            {
                return NotFound();
            }

            return Ok(regularEvent);
        }

        // PUT: api/regularEvents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutregularEvent(int id, regularEvent regularEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != regularEvent.eventId)
            {
                return BadRequest();
            }

            db.Entry(regularEvent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!regularEventExists(id))
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

        // POST: api/regularEvents
        [ResponseType(typeof(regularEvent))]
        public async Task<IHttpActionResult> PostregularEvent(regularEvent regularEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.regularEvent.Add(regularEvent);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (regularEventExists(regularEvent.eventId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = regularEvent.eventId }, regularEvent);
        }

        // DELETE: api/regularEvents/5
        [ResponseType(typeof(regularEvent))]
        public async Task<IHttpActionResult> DeleteregularEvent(int id)
        {
            regularEvent regularEvent = await db.regularEvent.FindAsync(id);
            if (regularEvent == null)
            {
                return NotFound();
            }

            db.regularEvent.Remove(regularEvent);
            await db.SaveChangesAsync();

            return Ok(regularEvent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool regularEventExists(int id)
        {
            return db.regularEvent.Count(e => e.eventId == id) > 0;
        }
    }
}