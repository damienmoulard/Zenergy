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
    public class usersController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();
        private UserServices userServices;

        public usersController()
        {
            userServices = new UserServices(db);
        }


        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        // GET: api/users
        [Authorize]
        public IQueryable<user> Getuser()
        {
            return db.user;
        }


        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/users/5
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


        /// <summary>
        /// Return all the users of a role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        // GET: api/users/findByRole
        [Route("api/users/findByRole")]
        [HttpGet]
        [ResponseType(typeof(user[]))]
        public async Task<IHttpActionResult> FindByRole(string role) {
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


        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        // POST: api/users
        [ResponseType(typeof(user))]
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


        /// <summary>
        /// Delete user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/users/5
        [ResponseType(typeof(user))]
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