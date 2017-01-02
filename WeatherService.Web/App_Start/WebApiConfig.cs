using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Practices.Unity;
using WeatherService.Provider;
using WeatherService.Web.Cache;
using WeatherService.Web.Resolver.ProductStore.Resolver;

namespace WeatherService.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var container = new UnityContainer();
			container.RegisterType<IWeatherRepository, OpenWeatherMapRepository>(new HierarchicalLifetimeManager());
			container.RegisterType<ICacheService, CacheService>(new HierarchicalLifetimeManager());
			config.DependencyResolver = new UnityResolver(container);

			config.Formatters.JsonFormatter.SupportedMediaTypes
				.Add(new MediaTypeHeaderValue("text/html"));

			config.Routes.MapHttpRoute(
				name: "WeatherRoute",
				routeTemplate: "api/{controller}/{action}/{city}"
			);
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
