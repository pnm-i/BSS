using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using MongoDB.Bson;
using PikabaV3.API.Filters;
using PikabaV3.GeneralProvider;
using PikabaV3.Logic;
using PikabaV3.Models;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories;

namespace PikabaV3.API.Controllers
{
    [RoutePrefix("api")]
    public class SellerController : BaseApiController
    {
        public SellerController(IModelFactory modelFactory, IPikabaV3Service service, IValidator validator)
            : base(modelFactory, service, validator) { }

        [HttpGet]
        [Route("sellers")]
        public IEnumerable<Seller> GetAllSellers()
        {
            return Service.Sellers.GetAll();
        }

        [CustomAuthorize]
        [Route("seller/{sellerId}")]
        [HttpGet]
        public IHttpActionResult GetSeller(string sellerId)
        {
            try
            {
                // Check valid seller id
                if (Validator.CheckValidId(sellerId))
                {
                    // Get seller by id
                    var seller = Service.Sellers.Get(ObjectId.Parse(sellerId));
                    if (seller != null)
                    {
                        return Ok(seller);
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

        [ModelValidator]
        [Route("seller")]
        [HttpPost]
        public IHttpActionResult PostSeller(RegisterSellerModel sellerModel)
        {
            try
            {
                // Check busy email
                if (!Validator.IsBusyEmail(sellerModel.Email))
                {
                    // provide seller
                    var seller = ModelFactory.Create(sellerModel);
                    // create seller
                    if (Service.Sellers.Add(seller))
                    {
                        return StatusCode(HttpStatusCode.Created);
                    }
                    return BadRequest("Seller not registered");
                }
                return BadRequest("Email is Busy");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize(UserRole.Seller, UserRole.Admin)]
        [Route("seller/{sellerId}")]
        [HttpPut]
        public IHttpActionResult PutSeller(UpdateSellerModel sellerModel, string sellerId)
        {
            try
            {
                // check valid seller Id
                if (Validator.CheckValidId(sellerId))
                {
                    // Provide seller
                    var seller = ModelFactory.Create(sellerModel);
                    // Update seller
                    if (Service.Sellers.Update(ObjectId.Parse(sellerId), seller))
                    {
                        return Ok();
                    }
                    return BadRequest("Seller not updated");
                }
                return BadRequest("The value " + sellerId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
