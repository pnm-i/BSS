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
    public class UserController : BaseApiController
    {
        public UserController(IModelFactory modelFactory, IPikabaV3Service service, IValidator validator)
            : base(modelFactory, service, validator) { }

        [Route("users")]
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return Service.Users.GetAll();
        }


        [CustomAuthorize]
        [Route("user/{userId}")]
        [HttpGet]
        public IHttpActionResult GetUser(string userId)
        {
            try
            {
                // check valid user id
                if (Validator.CheckValidId(userId))
                {
                    // Get user (buyer)
                    var buyer = Service.Users.Get(ObjectId.Parse(userId));
                    if (buyer != null)
                    {
                        return Ok(buyer);
                    }
                    return NotFound();
                }
                return BadRequest("The value " + userId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [Route("user")]
        [HttpPost]
        public IHttpActionResult PostUser(RegisterUserModel registerUserModel)
        {
            try
            {
                // check busy email
                if (!Validator.IsBusyEmail(registerUserModel.Email))
                {
                    // provide new user
                    var user = ModelFactory.Create(registerUserModel);
                    // add new user
                    if (Service.Users.Add(user))
                    {
                        return StatusCode(HttpStatusCode.Created);
                    }
                    return BadRequest("User not registered");
                }
                return BadRequest("This Email is busy");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize(UserRole.Buyer, UserRole.Admin)]
        [Route("user/{userId}")]
        [HttpPut]
        public IHttpActionResult PutUser(UpdateUserModel userModel, string userId)
        {
            try
            {
                // check valid user id
                if (Validator.CheckValidId(userId))
                {
                    // Provide updated user
                    var user = ModelFactory.Create(userModel);
                    // Check busy email
                    if (!Validator.IsBusyEmail(user.Email))
                    {
                        // Update User
                        if (Service.Users.Update(ObjectId.Parse(userId), user))
                        {
                            return Ok();
                        }
                        return BadRequest("User not updated");
                    }
                    return BadRequest("The email " + user.Email + " is busy.");
                }
                return BadRequest("The value " + userId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize]
        [Route("user/ChangePassword/{userId}")]
        [HttpPut]
        public IHttpActionResult ChangePassword(string userId, ChangePasswordModel passwordModel)
        {
            try
            {
                // check valid user id
                if (Validator.CheckValidId(userId))
                {
                    // get session 
                    var userSession = Service.Sessions.Get(ObjectId.Parse(userId));
                    // identify user
                    if (userSession.User_Id == ObjectId.Parse(userId))
                    {
                        // provide new password
                        var newPassword = ModelFactory.Create(passwordModel);
                        // create new password
                        if (Service.Users.ChangePassword(ObjectId.Parse(userId), newPassword))
                        {
                            return Ok();
                        }
                        return BadRequest("Password not changes");
                    }
                    return StatusCode(HttpStatusCode.Forbidden);
                }
                return BadRequest("The value " + userId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [CustomAuthorize]
        [Route("user/{userId}")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(string userId)
        {
            try
            {
                // check valid id
                if (Validator.CheckValidId(userId))
                {
                    // remove user by id
                    if (Service.Users.Remove(ObjectId.Parse(userId)))
                    {
                        return Ok();
                    }
                    return NotFound();
                }
                return BadRequest("The value " + userId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize]
        [Route("user/comment/{userId}/{cookieUuid}")]
        [HttpPost]
        public IHttpActionResult AddComment(string userId, string cookieUuid, CommentModel commentModel)
        {
            try
            {
                // check valid user id
                if (Validator.CheckValidId(userId))
                {
                    // get session
                    var session = Service.Sessions.Get(cookieUuid);
                    // get user
                    var user = Service.Users.Get(session.User_Id);
                    // provide comment and owner
                    var comment = ModelFactory.Create(user, commentModel);
                    // create comment
                    if (Service.Users.AddComment(ObjectId.Parse(userId), comment))
                    {
                        return StatusCode(HttpStatusCode.Created);
                    }
                    return BadRequest("Comment not added.");
                }
                return BadRequest("The value " + userId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ModelValidator]
        [CustomAuthorize]
        [Route("user/comment/{userId}/{commentId}/{cookieUuid}")]
        [HttpPut]
        public IHttpActionResult PutComment(string userId, string commentId, string cookieUuid, CommentModel commentModel)
        {
            try
            {
                // check valid ids
                if (Validator.CheckValidId(userId) && Validator.CheckValidId(commentId))
                {
                    // get session
                    var session = Service.Sessions.Get(cookieUuid);
                    // get old comment
                    var oldComment = Service.Users.GetComment(ObjectId.Parse(userId), ObjectId.Parse(commentId));
                    if (oldComment != null)
                    {
                        // identify owner a comment
                        if (oldComment.Owner.User_Id == session.User_Id)
                        {
                            // provide comment by model 
                            var comment = ModelFactory.Create(commentModel);
                            // update comment
                            if (Service.Users.UpdateComment(ObjectId.Parse(userId), ObjectId.Parse(commentId), comment))
                            {
                                return Ok();
                            }
                            return BadRequest("Comment not updated");
                        }
                        return BadRequest("You not owner this Comment");
                    }
                    return BadRequest("Comment not found");
                }
                return BadRequest("The value User Id:" + userId + " or value Comment Id" + commentId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [CustomAuthorize]
        [Route("user/comment/{userId}/{commentId}/{cookieUuid}")]
        [HttpDelete]
        public IHttpActionResult DeleteComment(string userId, string commentId, string cookieUuid)
        {
            try
            {
                // check valid ids
                if (Validator.CheckValidId(userId) && Validator.CheckValidId(commentId))
                {
                    // get session
                    var session = Service.Sessions.Get(cookieUuid);
                    // get old comment
                    var oldComment = Service.Users.GetComment(ObjectId.Parse(userId), ObjectId.Parse(commentId));
                    if (oldComment != null)
                    {
                        if (oldComment.Owner.User_Id == session.User_Id)
                        {
                            if (Service.Users.RemoveComment(ObjectId.Parse(userId), ObjectId.Parse(commentId)))
                            {
                                return Ok();
                            }
                            return BadRequest("Comment not removed");
                        }
                        return BadRequest("You not owner this Comment");
                    }
                    return BadRequest("Comment not found");
                }
                return BadRequest("The value User Id:" + userId + " or value Comment Id" + commentId + " is not valid for Id.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
