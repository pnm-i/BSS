using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;
using PikabaV3.MongoRepositories.Repositories;

namespace PikabaV3.API.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly UserRole[] _roles;
        private readonly ISessionRepository _sessionRepository;

        public CustomAuthorizeAttribute(params UserRole[] listRoles)
        {
            _roles = listRoles;
        }

        public CustomAuthorizeAttribute(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public CustomAuthorizeAttribute()
        {
            _sessionRepository = new SessionRepository(new MongoContext());
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var cookie = HttpContext.Current.Request.Cookies["PikabaV3"];
            if (cookie != null)
            {
                UserSession userSession = _sessionRepository.Get(cookie.Value);
                if (userSession != null && _roles.Contains(userSession.UserRole))
                {
                    return true;
                }
            }
            return false;
        }
    }
}