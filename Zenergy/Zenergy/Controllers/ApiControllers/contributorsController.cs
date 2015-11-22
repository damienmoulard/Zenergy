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
    public class contributorsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();
        
        // GET: api/contributors/5
        [ResponseType(typeof(contributor))]
        public async Task<IHttpActionResult> Getcontributor(int id)
        {
            contributor contributor = await db.contributor.FindAsync(id);
            if (contributor == null)
            {
                return NotFound();
            }

            return Ok(contributor);
        }
        
        // POST: api/contributors
        [ResponseType(typeof(contributor))]
        public async Task<IHttpActionResult> Postcontributor(contributor contributor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.contributor.Add(contributor);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (contributorExists(contributor.userId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = contributor.userId }, contributor);
        }

        // DELETE: api/contributors/5
        [ResponseType(typeof(contributor))]
        public async Task<IHttpActionResult> Deletecontributor(int id)
        {
            contributor contributor = await db.contributor.FindAsync(id);
            if (contributor == null)
            {
                return NotFound();
            }

            db.contributor.Remove(contributor);
            await db.SaveChangesAsync();

            return Ok(contributor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool contributorExists(int id)
        {
            return db.contributor.Count(e => e.userId == id) > 0;
        }
    }
}