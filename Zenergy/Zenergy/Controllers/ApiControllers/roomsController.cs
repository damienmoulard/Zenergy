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
    public class roomsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();

        // GET: api/rooms
        public IQueryable<room> Getroom()
        {
            return db.room;
        }

        // GET: api/rooms/5
        [ResponseType(typeof(room))]
        public async Task<IHttpActionResult> Getroom(int id)
        {
            room room = await db.room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // PUT: api/rooms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putroom(int id, room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.roomId)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!roomExists(id))
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

        // POST: api/rooms
        [ResponseType(typeof(room))]
        public async Task<IHttpActionResult> Postroom(room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.room.Add(room);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = room.roomId }, room);
        }

        // DELETE: api/rooms/5
        [ResponseType(typeof(room))]
        public async Task<IHttpActionResult> Deleteroom(int id)
        {
            room room = await db.room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            db.room.Remove(room);
            await db.SaveChangesAsync();

            return Ok(room);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool roomExists(int id)
        {
            return db.room.Count(e => e.roomId == id) > 0;
        }
    }
}