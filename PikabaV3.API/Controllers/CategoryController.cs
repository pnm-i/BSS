using System;
using System.Collections.Generic;
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
    public class CategoryController : BaseApiController
    {
        public CategoryController(IModelFactory modelFactory, IPikabaV3Service service, IValidator validator)
            : base(modelFactory, service, validator) { }


        [Route("categories")]
        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return Service.Categories.GetAll();
        }

        [Route("category/{categoryId}")]
        [HttpGet]
        public IHttpActionResult GetCategory(string categoryId)
        {
            try
            {
                // check valid category id
                if (Validator.CheckValidId(categoryId))
                {
                    // Get category from db
                    var category = Service.Categories.Get(ObjectId.Parse(categoryId));
                    if (category != null)
                    {
                        return Ok(category);
                    }
                    return BadRequest();
                }
                return BadRequest("The value " + categoryId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize(UserRole.Admin)]
        [Route("category")]
        [HttpPost]
        public IHttpActionResult PostCategory(CategoryModel categoryModel)
        {
            try
            {
                // Provide category
                Category category = ModelFactory.Create(categoryModel);
                // Create category
                if (Service.Categories.Add(category))
                {
                    return Ok();
                }
                return BadRequest("Category not created");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize(UserRole.Admin)]
        [Route("category/{categoryId}")]
        [HttpPut]
        public IHttpActionResult PutCategory(string categoryId, CategoryModel categoryModel)
        {
            try
            {
                // check valid category Id
                if (Validator.CheckValidId(categoryId))
                {
                    // provide category
                    var category = ModelFactory.Create(categoryModel);
                    // update category
                    if (Service.Categories.Update(ObjectId.Parse(categoryId), category))
                    {
                        return Ok();
                    }
                    return BadRequest("Category not updated");
                }
                return BadRequest("The value " + categoryId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [CustomAuthorize(UserRole.Admin)]
        [Route("category/{categoryId}")]
        [HttpDelete]
        public IHttpActionResult DeleteCategory(string categoryId)
        {
            try
            {
                // caheck valid category id
                if (Validator.CheckValidId(categoryId))
                {
                    // Remove category
                    if (Service.Categories.Remove(ObjectId.Parse(categoryId)))
                    {
                        return Ok();
                    }
                    return BadRequest("Category not found");
                }
                return BadRequest("The value " + categoryId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
