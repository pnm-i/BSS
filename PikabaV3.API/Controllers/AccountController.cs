using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PikabaV3.API.Filters;
using PikabaV3.GeneralProvider;
using PikabaV3.Logic;
using PikabaV3.Models;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories;

namespace PikabaV3.API.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        public AccountController(IModelFactory modelFactory, IPikabaV3Service service, IValidator validator)
            : base(modelFactory, service, validator) { }

        [ModelValidator]
        [HttpPost]
        [Route("login/{cookieUuid}")]
        public IHttpActionResult LogIn(string cookieUuid, LoginModel loginModel)
        {
            try
            {
                var user = Service.Sessions.GetUser(loginModel);
                if (user != null)
                {
                    // Provide session
                    var session = ModelFactory.Create(user, loginModel, cookieUuid);
                    // Create session
                    if (Service.Sessions.Add(session))
                    {
                        return Ok(user.Role.ToString());
                    }
                    return BadRequest("Session not Created");
                }
                return BadRequest("User not found");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("islogin/{cookieUuid}")]
        public bool IsLogin(string cookieUuid)
        {
            if (Service.Sessions.Get(cookieUuid) == null)
            {
                return false;
            }
            return true;
        }


        // Get account to show user profile
        [CustomAuthorize]
        [HttpGet]
        [Route("{cookieUuid}")]
        public HttpResponseMessage GetAccount(string cookieUuid)
        {
            // Checking session
            var session = Service.Sessions.Get(cookieUuid);
            if (session != null)
            {
                // Check role user
                if (session.UserRole == UserRole.Buyer)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Found);
                    response.Headers.Location = new Uri("http://localhost:49909/api/buyer/" + session.User_Id);
                    return response;
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.Found);
                    response.Headers.Location = new Uri("http://localhost:49909/api/seller/" + session.User_Id);
                    return response;
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "Account not found");
        }
    }
}
