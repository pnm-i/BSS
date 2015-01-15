using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using PikabaV3.Api;
using PikabaV3.GeneralProvider;
using PikabaV3.Logic;
using PikabaV3.MongoRepositories;

namespace PikabaV3.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            /* Dependency injection */
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<PikabaV3Service>().As<IPikabaV3Service>();
            builder.RegisterType<ModelFactory>().As<IModelFactory>();
            builder.RegisterType<Validator>().As<IValidator>();
            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}