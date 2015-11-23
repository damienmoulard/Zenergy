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
    public class activitiesController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();
        private ActivityService activityServices;

        public activitiesController()
        {
            activityServices = new ActivityService(db);
        }

        // GET: api/activities
        public IQueryable<activity> Getactivity()
        {
            return db.activity;
        }

        // GET: api/activities/5
        [ResponseType(typeof(activity))]
        public IHttpActionResult Getactivity(int id)
        {
            activity activity = db.activity.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        // GET: api/activities/findByManagerId/5
        [Route("api/activities/findByManagerId/{managerId}")]
        [HttpGet]
        [ResponseType(typeof(activity[]))]
        public async Task<IHttpActionResult> findActivitiesByManagerId(int managerId)
        {
            activity[] activities = await activityServices.findActivitiesByManagerId(managerId);
            if (activities == null)
            {
                return NotFound();
            }

            return Ok(activities);
        }

        // PUT: api/activities/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult Putactivity(int id, activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activity.activityId)
            {
                return BadRequest();
            }

            db.Entry(activity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!activityExists(id))
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

        // POST: api/activities
        [ResponseType(typeof(activity))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult Postactivity(activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.activity.Add(activity);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = activity.activityId }, activity);
        }

        // DELETE: api/activities/5
        [ResponseType(typeof(activity))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult Deleteactivity(int id)
        {
            activity activity = db.activity.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            db.activity.Remove(activity);
            db.SaveChanges();

            return Ok(activity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool activityExists(int id)
        {
            return db.activity.Count(e => e.activityId == id) > 0;
        }
    }
}