using System.Web.Http;
using Redack.ServiceLayer.App_Start;
using Redack.ServiceLayer.Filters;

namespace Redack.ServiceLayer
{
    public static class WebApiConfig
    {
        public static string DefaultRouteName = "redack";

        public static void Register(HttpConfiguration config)
        {
            //config.Filters.Add(new ValidateModelAttribute());

            config.MapHttpAttributeRoutes(new InheritedRouteProvider());

            config.Routes.MapHttpRoute(
                name: DefaultRouteName,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
