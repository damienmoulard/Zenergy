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
    [Authorize]
    public class usersController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();
        private UserServices userServices;

        public usersController()
        {
            userServices = new UserServices(db);
        }

        // GET: api/users
        [Authorize(Roles = "Admin")]
        public IQueryable<user> Getuser()
        {
            return db.user;
        }

        // GET: api/users/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(user))]
        public async Task<IHttpActionResult> Getuser(int id)
        {
            user user = await db.user.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("api/users/findByMail")]
        [HttpGet]
        [ResponseType(typeof(user))]
        public async Task<IHttpActionResult> findByMail(string userMail)
        {
            user user = await userServices.findByMail(userMail);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/users/findByRole
        // Return all the users of a role
        [Route("api/users/findByRole")]
        [HttpGet]
        [ResponseType(typeof(user[]))]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IHttpActionResult> findByRole(string role) {
            user[] users = null;

            if (role == "Administrator")
            {
                users = await userServices.getAdmins();
            }
            else if (role == "Manager")
            {
                users = await userServices.getManagers();
            }
            else if (role == "Contributor")
            {
                users = await userServices.getManagers();
            }
            else if (role == "Member")
            {
                users = await userServices.getMembers();
            }
            else
            {
                return BadRequest();
            }

            return Ok(users);
        }

        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putuser(int id, user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.userId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
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

        // POST: api/users
        [ResponseType(typeof(user))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Postuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.user.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.userId }, user);
        }

        // DELETE: api/users/5
        [ResponseType(typeof(user))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Deleteuser(int id)
        {
            user user = await db.user.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.user.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(int id)
        {
            return db.user.Count(e => e.userId == id) > 0;
        }
    }
}