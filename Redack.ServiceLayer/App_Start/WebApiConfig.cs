using System.Web.Http;
using Redack.ServiceLayer.App_Start;

namespace Redack.ServiceLayer
{
    public static class WebApiConfig
    {
        public static string DefaultRouteName = "redack";

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes(new InheritedRouteProvider());

            config.Routes.MapHttpRoute(
                name: DefaultRouteName,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
