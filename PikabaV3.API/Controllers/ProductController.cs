using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MongoDB.Bson;
using PikabaV3.API.Controllers;
using PikabaV3.API.Filters;
using PikabaV3.GeneralProvider;
using PikabaV3.Logic;
using PikabaV3.Models;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories;

namespace PikabaV3.Api.Controllers
{
    [RoutePrefix("api")]
    public class ProductController : BaseApiController
    {
        public ProductController(IModelFactory modelFactory, IPikabaV3Service service, IValidator validator)
            : base(modelFactory, service, validator) { }
        
        [Route("products")]
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            return Service.Products.GetAll();
        }

        [Route("productsSeller/{sellerId}")]
        [HttpGet]
        public IHttpActionResult GetProductsSeller(string sellerId)
        {
            try
            {
                // check valid seller seller id
                if (Validator.CheckValidId(sellerId))
                {
                    //Get all products created some seller by him id
                    var sellerProducts = Service.Products.GetSellerProducts(ObjectId.Parse(sellerId)).ToList();
                    if (sellerProducts.Any())
                    {
                        return Ok(sellerProducts);
                    }
                    return NotFound();
                }
                return BadRequest("The value " + sellerId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("product/{productId}")]
        [HttpGet]
        public IHttpActionResult GetProduct(string productId)
        {
            try
            {
                // check valid product Id
                if (Validator.CheckValidId(productId))
                {
                    // Get one product by id
                    var product = Service.Products.Get(ObjectId.Parse(productId));
                    if (product != null)
                    {
                        return Ok(product);
                    }
                    return NotFound();
                }
                return BadRequest("The value " + productId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize(UserRole.Seller, UserRole.Admin)]
        [Route("product/{cookieUuid}")]
        [HttpPost]
        public IHttpActionResult PostProduct(ProductModel productModel, string cookieUuid)
        {
            try
            {
                // get user session
                var session = Service.Sessions.Get(cookieUuid);
                // get seller
                var seller = Service.Sellers.Get(session.User_Id);
                // check exceeded limit creation products and active user
                if (!Validator.IsExceededLimitCreationProducts(session.User_Id) && seller.IsActive)
                {
                    // provide product by model
                    var product = ModelFactory.Create(seller, productModel);
                    // create product
                    if (Service.Products.Add(product))
                    {
                        return StatusCode(HttpStatusCode.Created);
                    }
                    return BadRequest("Product not created");
                }
                return StatusCode(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize(UserRole.Seller, UserRole.Admin)]
        [Route("product/{productId}/{cookieUuid}")]
        [HttpPut]
        public IHttpActionResult PutProduct(ProductModel productModel, string productId, string cookieUuid)
        {
            try
            {
                // check valid product Id
                if (Validator.CheckValidId(productId))
                {
                    // Get user session
                    UserSession session = Service.Sessions.Get(cookieUuid);
                    // Get old product to identify owner
                    Product productOld = Service.Products.Get(ObjectId.Parse(productId));
                    // Identification owner a product 
                    if (session.User_Id == productOld.Owner.User_Id)
                    {
                        // Provide product by model
                        Product product = ModelFactory.Create(productModel);
                        // Update product
                        if (Service.Products.Update(ObjectId.Parse(productId), product))
                        {
                            return StatusCode(HttpStatusCode.Created);
                        }
                        return BadRequest("Product not updated");
                    }
                    return StatusCode(HttpStatusCode.Forbidden);
                }
                return BadRequest("The value " + productId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [CustomAuthorize(UserRole.Seller, UserRole.Admin)]
        [Route("product/{productId}/{cookieUuid}")]
        [HttpDelete]
        public IHttpActionResult DeleteProduct(string productId, string cookieUuid)
        {
            try
            {
                // check valid product id
                if (Validator.CheckValidId(productId))
                {
                    // Get session by cookie
                    var session = Service.Sessions.Get(cookieUuid);
                    // Get product to identify owner
                    var productToDelete = Service.Products.Get(ObjectId.Parse(productId));
                    // Identify owner product
                    if (session.User_Id == productToDelete.Owner.User_Id)
                    {
                        // remove product from db
                        if (Service.Products.Remove(ObjectId.Parse(productId)))
                        {
                            return Ok();
                        }
                        return BadRequest("Product not deleted");
                    }
                    return StatusCode(HttpStatusCode.Forbidden);
                }
                return BadRequest("The value " + productId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize]
        [Route("product/comment/{productId}/{cookieUuid}")]
        [HttpPost]
        public IHttpActionResult AddComment(string productId, string cookieUuid, CommentModel commentModel)
        {
            try
            {
                // check valid product id
                if (Validator.CheckValidId(productId))
                {
                    // get user session 
                    var session = Service.Sessions.Get(cookieUuid);
                    // get seller
                    var seller = Service.Sellers.Get(session.User_Id);
                    if (seller != null && seller.IsActive)
                    {
                        // provide comment and owner
                        var comment = ModelFactory.Create(seller, commentModel);
                        // create comment
                        if (Service.Products.AddComment(ObjectId.Parse(productId), comment))
                        {
                            return StatusCode(HttpStatusCode.Created);
                        }
                        return BadRequest("Comment not created");
                    }
                    return BadRequest("User not found");
                }
                return BadRequest("The value " + productId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize]
        [Route("product/comment/{productId}/{commentId}/{string cookieUuid}")]
        [HttpPut]
        public IHttpActionResult PutComment(string productId, string commentId, string cookieUuid, CommentModel commentModel)
        {
            try
            {
                // check valid ids
                if (Validator.CheckValidId(productId) && Validator.CheckValidId(commentId))
                {
                    // get session
                    var session = Service.Sessions.Get(cookieUuid);
                    // get old comment
                    var oldComment = Service.Products.GetComment(ObjectId.Parse(productId), ObjectId.Parse(commentId));
                    if (oldComment != null)
                    {
                        // check owner comment
                        if (oldComment.Owner.User_Id == session.User_Id)
                        {
                            // provide comment by model and update
                            var comment = ModelFactory.Create(commentModel);
                            // update comment
                            if (Service.Products.UpdateComment(ObjectId.Parse(productId), ObjectId.Parse(commentId), comment))
                            {
                                return StatusCode(HttpStatusCode.Created);
                            }
                            return BadRequest("Comment not updated");
                        }
                        return StatusCode(HttpStatusCode.Forbidden);
                    }
                    return BadRequest("Comment not found");
                }
                return BadRequest("The value User Id:" + productId + " or value Comment Id" + commentId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [CustomAuthorize]
        [Route("product/comment/{productId}/{commentId}/{cookieUuid}")]
        [HttpDelete]
        public IHttpActionResult DeleteComment(string productId, string commentId, string cookieUuid)
        {
            try
            {
                // check valid ids
                if (Validator.CheckValidId(productId) && Validator.CheckValidId(commentId))
                {
                    // get session
                    var session = Service.Sessions.Get(cookieUuid);
                    // get old comment
                    var oldComment = Service.Products.GetComment(ObjectId.Parse(productId), ObjectId.Parse(commentId));
                    if (oldComment != null)
                    {
                        if (oldComment.Owner.User_Id == session.User_Id)
                        {
                            if (Service.Products.RemoveComment(ObjectId.Parse(productId), ObjectId.Parse(commentId)))
                            {
                                return Ok();
                            }
                            return BadRequest("Comment not deleted");
                        }
                        return StatusCode(HttpStatusCode.Forbidden);
                    }
                    return BadRequest("Comment not found");
                }
                return BadRequest("The value User Id:" + productId + " or value Comment Id" + commentId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
