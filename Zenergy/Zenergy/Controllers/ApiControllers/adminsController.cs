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
    [Authorize]
    public class adminsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();

        // GET: api/admins/5
        [ResponseType(typeof(admin))]
        public async Task<IHttpActionResult> Getadmin(int id)
        {
            admin admin = await db.admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            return Ok(admin);
        }

        // POST: api/admins
        [ResponseType(typeof(admin))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Postadmin(admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.admin.Add(admin);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (adminExists(admin.userId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = admin.userId }, admin);
        }

        // DELETE: api/admins/5
        [ResponseType(typeof(admin))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Deleteadmin(int id)
        {
            admin admin = await db.admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            db.admin.Remove(admin);
            await db.SaveChangesAsync();

            return Ok(admin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool adminExists(int id)
        {
            return db.admin.Count(e => e.userId == id) > 0;
        }
    }
}