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
    public class roomContentsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();

        // GET: api/roomContents
        public IQueryable<roomContent> GetroomContent()
        {
            return db.roomContent;
        }

        // GET: api/roomContents/5
        [ResponseType(typeof(roomContent))]
        public async Task<IHttpActionResult> GetroomContent(int id)
        {
            roomContent roomContent = await db.roomContent.FindAsync(id);
            if (roomContent == null)
            {
                return NotFound();
            }

            return Ok(roomContent);
        }

        // PUT: api/roomContents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutroomContent(int roomId,int accessoryId, roomContent roomContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (roomId != roomContent.roomId || accessoryId != roomContent.accessoryId)
            {
                return BadRequest();
            }

            db.Entry(roomContent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!roomContentExists(roomId, accessoryId))
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

        // POST: api/roomContents
        [ResponseType(typeof(roomContent))]
        public async Task<IHttpActionResult> PostroomContent(roomContent roomContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.roomContent.Add(roomContent);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (roomContentExists(roomContent.roomId, roomContent.accessoryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = roomContent.roomId }, roomContent);
        }

        // DELETE: api/roomContents?roomId=2&accessoryId=1
        [ResponseType(typeof(roomContent))]
        public async Task<IHttpActionResult> DeleteroomContent(int roomId, int accessoryId)
        {
            roomContent roomContent = await db.roomContent.Where(rc => rc.roomId == roomId & rc.accessoryId == accessoryId).FirstAsync();
            if (roomContent == null)
            {
                return NotFound();
            }

            db.roomContent.Remove(roomContent);
            await db.SaveChangesAsync();

            return Ok(roomContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool roomContentExists(int roomId, int accessoryId)
        {
            return db.roomContent.Count(e => e.roomId == roomId && e.accessoryId == accessoryId) > 0;
        }
    }
}