using System.Web.Http;
using PikabaV3.API.Filters;

namespace PikabaV3.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Filters.Add(new NotImplExceptionFilterAttribute());
            //config.Filters.Add(new ModelValidatorAttribute());

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "Api Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
