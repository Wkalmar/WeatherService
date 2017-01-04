using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using WeatherService.Provider;
using WeatherService.Web.Cache;
using WeatherService.Web.CookieStorage;

namespace WeatherService.Web
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();
	  container.RegisterType<IWeatherRepository, OpenWeatherMapRepository>();
	  container.RegisterType<ICacheService, CacheService>();
	  container.RegisterType<ICookieStorage, CookieStorage.CookieStorage>();
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
    
    }
  }
}