using System.Data.Common;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using Redack.DatabaseLayer.DataAccess;
using Redack.ServiceLayer.App_Start;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Security;
using Unity;
using Unity.Registration;
using Unity.Lifetime;
using Unity.Injection;
using System;
using Redack.ServiceLayer.Controllers;
using Unity.AspNet.WebApi;

namespace Redack.ServiceLayer
{
	public static class WebApiConfig
	{
		public static string DefaultRouteName = "redack";

		public static void Register(HttpConfiguration config)
		{
			var container = new UnityContainer();

			try
			{
				HttpApiConfiguration apiConfig = (HttpApiConfiguration)config;

				container.RegisterType<IDbContext, RedackDbContext>(
					new HierarchicalLifetimeManager(),
					new InjectionConstructor(apiConfig.DbConnection));
			}
			catch(InvalidCastException)
			{
				container.RegisterType<IDbContext, RedackDbContext>(
					new HierarchicalLifetimeManager(),
					new InjectionConstructor());
			}
			
			config.DependencyResolver = new UnityDependencyResolver(container);

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
