using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Zenergy.Models;

namespace Zenergy.Controllers.ApiControllers
{
    public class accessoriesController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();

        // GET: api/accessories
        public IQueryable<accessory> Getaccessory()
        {
            return db.accessory;
        }

        // GET: api/accessories/5
        [ResponseType(typeof(accessory))]
        public async Task<IHttpActionResult> Getaccessory(int id)
        {
            accessory accessory = await db.accessory.FindAsync(id);
            if (accessory == null)
            {
                return NotFound();
            }

            return Ok(accessory);
        }

        // PUT: api/accessories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putaccessory(int id, accessory accessory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accessory.accessoryId)
            {
                return BadRequest();
            }

            db.Entry(accessory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!accessoryExists(id))
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

        // POST: api/accessories
        [ResponseType(typeof(accessory))]
        public async Task<IHttpActionResult> Postaccessory()
        {
            var context = HttpContext.Current;
            var data = HttpContext.Current.Request.Form;
            string postData = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();
            accessory accessory = JsonConvert.DeserializeObject<accessory>(postData);
            db.accessory.Add(accessory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = accessory.accessoryId }, accessory);
        }

        // DELETE: api/accessories/5
        [ResponseType(typeof(accessory))]
        public async Task<IHttpActionResult> Deleteaccessory(int id)
        {
            accessory accessory = await db.accessory.FindAsync(id);
            if (accessory == null)
            {
                return NotFound();
            }

            db.accessory.Remove(accessory);
            await db.SaveChangesAsync();

            return Ok(accessory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool accessoryExists(int id)
        {
            return db.accessory.Count(e => e.accessoryId == id) > 0;
        }
    }
}