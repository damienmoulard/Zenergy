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

namespace Zenergy.Controllers
{
    public class productsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();

        // GET: api/products
        [HttpGet]
        [Route("api/product")]
        [Authorize(Roles = "User")]
        public IQueryable<product> Getproduct()
        {
            return db.product;
        }

        // GET: api/products/5
        [HttpGet]
        [Route("api/products/{productId}")]
        [Authorize(Roles = "User")]
        [ResponseType(typeof(product))]
        public async Task<IHttpActionResult> Getproduct(int productId)
        {
            product product = await db.product.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/products/5
        [HttpPut]
        [Route("api//products/{productId}")]
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putproduct(int productId, product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productId != product.productId)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productExists(productId))
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

        // POST: api/products
        [HttpPost]
        [Route("api/product")]
        [ResponseType(typeof(product))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Postproduct(product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.product.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.productId }, product);
        }

        // DELETE: api/products/5
        [HttpDelete]
        [Route("api/products/{productId}")]
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(product))]
        public async Task<IHttpActionResult> Deleteproduct(int productId)
        {
            product product = await db.product.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            db.product.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool productExists(int id)
        {
            return db.product.Count(e => e.productId == id) > 0;
        }
    }
}