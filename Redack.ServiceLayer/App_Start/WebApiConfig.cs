using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using Redack.ServiceLayer.App_Start;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Security;

namespace Redack.ServiceLayer
{
	public static class WebApiConfig
	{
		public static string DefaultRouteName = "redack";

		public static void Register(HttpConfiguration config)
		{
			config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
			config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

			config.MapHttpAttributeRoutes(new InheritedRouteProvider());

			config.Routes.MapHttpRoute(
				name: DefaultRouteName,
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
