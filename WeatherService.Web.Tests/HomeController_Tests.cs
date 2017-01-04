using System.Collections.Generic;
using System.Web.Mvc;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using WeatherService.Provider;
using WeatherService.Web.Cache;
using WeatherService.Web.Controllers;
using WeatherService.Web.CookieStorage;
using WeatherService.Web.ViewModels;

namespace WeatherService.Web.Tests
{
	[TestFixture]
	public class HomeController_Tests
	{
		[Test]
		public void _Forecast_IfCalled_AddsItemToRecentCities()
		{
			var cache = Substitute.For<ICacheService>();
			var repo = Substitute.For<IWeatherRepository>();
			var cookieStorage = Substitute.For<ICookieStorage>();
			cookieStorage.GetItems("cookie_recent_cities").Returns(new List<string>() {"lul"});
			var controller = new HomeController(cache, repo, cookieStorage);
			var view = controller._Forecast("kek").Result as PartialViewResult;
			var viewModel = view.Model as WeatherReportViewModel;
			viewModel.RecentCities.ShouldAllBeEquivalentTo(new List<string>() {"lul", "kek"});
		}

		[Test]
		public void _Forecast_IfCacheHasValues_ReturnsItemFromCache()
		{
			var forecastFromCache = new Forecast()
			{
				City = "kek",
				Country = "HU"
			};
			var cache = Substitute.For<ICacheService>();
			cache.Get<Forecast>("forecastkek").Returns(forecastFromCache);
			var forecastFromRepo = new Forecast()
			{
				City = "lul",
				Country = "LK"
			};
			var repo = Substitute.For<IWeatherRepository>();
			repo.GetDailyForecast("kek").Returns(forecastFromRepo);
			var cookieStorage = Substitute.For<ICookieStorage>();
			var controller = new HomeController(cache, repo, cookieStorage);
			var view = controller._Forecast("kek").Result as PartialViewResult;
			var viewModel = view.Model as WeatherReportViewModel;
			viewModel.Forecast.ShouldBeEquivalentTo(forecastFromCache);
		}

		[Test]
		public void _Forecast_IfCacheHasnotValues_ReturnsItemFromRepo()
		{
			var cache = Substitute.For<ICacheService>();
			cache.Get<Forecast>("forecastkek").ReturnsNull();
			var forecastFromRepo = new Forecast()
			{
				City = "lul",
				Country = "LK"
			};
			var repo = Substitute.For<IWeatherRepository>();
			repo.GetDailyForecast("kek").Returns(forecastFromRepo);
			var cookieStorage = Substitute.For<ICookieStorage>();
			var controller = new HomeController(cache, repo, cookieStorage);
			var view = controller._Forecast("kek").Result as PartialViewResult;
			var viewModel = view.Model as WeatherReportViewModel;
			viewModel.Forecast.ShouldBeEquivalentTo(forecastFromRepo);
		}
	}
}
