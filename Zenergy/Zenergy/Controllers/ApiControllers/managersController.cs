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
    public class managersController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();

        // GET: api/managers/5
        [ResponseType(typeof(manager))]
        public async Task<IHttpActionResult> Getmanager(int id)
        {
            manager manager = await db.manager.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            return Ok(manager);
        }
        
        // POST: api/managers
        [ResponseType(typeof(manager))]
        public async Task<IHttpActionResult> Postmanager(manager manager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.manager.Add(manager);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (managerExists(manager.userId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = manager.userId }, manager);
        }

        // DELETE: api/managers/5
        [ResponseType(typeof(manager))]
        public async Task<IHttpActionResult> Deletemanager(int id)
        {
            manager manager = await db.manager.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            db.manager.Remove(manager);
            await db.SaveChangesAsync();

            return Ok(manager);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool managerExists(int id)
        {
            return db.manager.Count(e => e.userId == id) > 0;
        }
    }
}