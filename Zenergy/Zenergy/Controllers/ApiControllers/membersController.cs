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
    public class membersController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();

        // GET: api/members/5
        [ResponseType(typeof(member))]
        public async Task<IHttpActionResult> Getmember(int id)
        {
            member member = await db.member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // POST: api/members
        [ResponseType(typeof(member))]
        public async Task<IHttpActionResult> Postmember(member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.member.Add(member);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (memberExists(member.userId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = member.userId }, member);
        }

        // DELETE: api/members/5
        [ResponseType(typeof(member))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Deletemember(int id)
        {
            member member = await db.member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            db.member.Remove(member);
            await db.SaveChangesAsync();

            return Ok(member);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool memberExists(int id)
        {
            return db.member.Count(e => e.userId == id) > 0;
        }
    }
}