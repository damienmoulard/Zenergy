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
    public class CartContentsController : ApiController
    {
        private ZenergyContext db = new ZenergyContext();


        // GET: api/users/{userId}/basket
        [HttpGet]
        [Route("api/users/{userId}/basket")]
        [Authorize]
        [ResponseType(typeof(IQueryable<CartContent>))]
        public  IHttpActionResult GetCartContent(int userId)
        {
            //verify the identity of the user
            var currentUserId = db.user.Where(u => u.mail.Equals(this.User.Identity.Name)).FirstAsync().Result.userId;
            if (!(currentUserId == userId))
            {
                return BadRequest("You are not authorized to access this user's cart content!");
            }
            IQueryable<CartContent> cartContent = db.CartContent.Where(cc => cc.userId == (userId));
            if (!cartContent.Any())
            {
                return NotFound();
            }
           

            return Ok(cartContent);
        }

        // PUT: api/CartContents/5
        [HttpPut]
        [Route("api/users/{userId}/basket")]
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCartContent(int userId, CartContentModel cartContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId != cartContent.userId)
            {
                return BadRequest("You are not authorized to access to this user's basket");
            }

            if (!CartContentExists(userId, cartContent.productId))
            {
                return NotFound();
            }

            //Verify if the quantity in the cartContent if inferior to the available quantity for the product
            int enoughProductInStock = db.product.Where(p => p.productId == cartContent.productId).FirstAsync().Result.availableQty.Value;
            if (cartContent.productQuantity > enoughProductInStock)
            {
                return BadRequest(string.Format("There are only {0} products left!", enoughProductInStock));
            }
            db.Entry(new CartContent() { userId = cartContent.userId, productId = cartContent.productId, productQuantity = cartContent.productQuantity }).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                 throw;
              
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/users/basket
        [HttpPost]
        [Route("api/users/{userId}/basket")]
        [Authorize]
        [ResponseType(typeof(CartContentModel))]
        public async Task<IHttpActionResult> PostCartContent(int userId,CartContentModel cartContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId != cartContent.userId)    //verify that we are creating a cartcontent into the rigth user's basket
            {
                return BadRequest();
            }
            if (!CartContentExists(userId, cartContent.productId))
            {
                //Verify if the quantity in the cartContent if inferior to the available quantity for the product
                int enoughProductInStock = db.product.Where(p => p.productId == cartContent.productId).FirstAsync().Result.availableQty.Value;
                if (cartContent.productQuantity > enoughProductInStock)
                {
                    return BadRequest(string.Format("There are only {0} products left!", enoughProductInStock));
                }
                db.CartContent.Add(new CartContent() { userId = cartContent.userId, productId = cartContent.productId, productQuantity = cartContent.productQuantity });
            }
            else return BadRequest("This product is already in your cart");
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                
                throw;
            }
            
            return Created("api/users/basket", cartContent);
        }


        /// <summary>
        /// Validate the basket, clear it and create a purchase.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cartContent"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/users/{userId}/basket/validate")]
        [Authorize]
        [ResponseType(typeof(purchase))]
        public async Task<IHttpActionResult> ValidateBasket(int userId, CartContent cartContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!VerifyIdentity(userId))    //verify that we are creating a cartcontent into the rigth user's basket
            {
                return BadRequest("You are not authorized to access to this user's cart!");
            }
            
            var basket = db.CartContent.Where(cc => cc.userId == userId);
            if (!basket.Any())
            {
                return BadRequest("Your cart is empty!");
            }
            var purchaseContents = new List<purchaseContent>();
            
            foreach (CartContent item in basket)
            {
                var purchaseContent = new purchaseContent();
                purchaseContent.productId = item.productId;
                purchaseContent.product = item.product;
                purchaseContent.productQuantity = item.productQuantity;
                purchaseContents.Add(purchaseContent);

            var purchase = db.purchase.Add(new purchase() { userId = cartContent.userId, purchaseDate = DateTime.Today, user = cartContent.user, purchaseContent = purchaseContents });

            try
            {
                //Clearing the basket
               await ClearBasket(basket.ToListAsync().Result); 
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Created("api/users/{userId}/basket/validate", purchase);
        }

        // DELETE: api/users/{userId}/basket
        [HttpDelete]
        [Route("api/users/{userId}/basket/{productId}")]
        [Authorize]
        [ResponseType(typeof(CartContentModel))]
        public async Task<IHttpActionResult> DeleteCartContent(int userId, int productId)
        {
            if (!VerifyIdentity(userId))
            {
                return BadRequest("You are not authorized to access to this user's basket");
            }
            var cartContent = db.CartContent.Where(cc => cc.userId == userId && cc.productId == productId);
            if (!cartContent.Any())
            {
                return NotFound();
            }

            if (userId != cartContent.FirstAsync().Result.userId)
            {
                return BadRequest("You are not authorized to access to this user's basket");
            }

            db.CartContent.Remove(cartContent.FirstAsync().Result);
            await db.SaveChangesAsync();
            return Ok(cartContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartContentExists(int userId, int productId)
        {
            return db.CartContent.Count(e => e.userId == userId && e.productId == productId) > 0;
        }

        private async Task<int> ClearBasket(List<CartContent> basket)
        {
            db.CartContent.RemoveRange(basket);
            return await db.SaveChangesAsync();
        }

        public bool VerifyIdentity(int userId)
        {
            return db.user.Where(cc => cc.mail.Equals(this.User.Identity.Name)).FirstAsync().Result.userId == userId;
        }
    }
}