using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using WeatherService.Provider;
using WeatherService.Web.Cache;
using WeatherService.Web.Controllers;
using WeatherService.Web.Models;
using DailyForecast = WeatherService.Provider.DailyForecast;

namespace WeatherService.Web.Tests
{
	[TestFixture]
	public class WeatherController_Tests
	{
		[Test]
		public void Daily_IfCacheHasItem_ReturnsValueFromCache()
		{
			var city = "kek";
			var duration = 666;
			var reportFromCache = new WeatherReport()
			{
				Location = new Location()
				{
					Country = "HU",
					Name = "Kek"
				}

			};
			var cache = Substitute.For<ICacheService>();
			cache.Get<WeatherReport>(string.Format("{0}:{1}", duration, city))
				.Returns(reportFromCache);
			var repostitory = Substitute.For<IWeatherRepository>();
			var itemFromRepo = new Forecast()
			{
				City = "lul",
				Country = "LK"
			};
			repostitory.GetDailyForecast(city, duration).Returns(itemFromRepo);
			var controller = new WeatherController(cache, repostitory);
			var actualReport = controller.Daily(city, duration).Result;
			actualReport.ShouldBeEquivalentTo(reportFromCache);
		}

		[Test]
		public void Daily_IfCacheHasNotItem_ReturnsValueFromApi()
		{
			var city = "kek";
			var duration = 666;
			var cache = Substitute.For<ICacheService>();
			cache.Get<WeatherReport>(string.Format("{0}:{1}", duration, city))
				.ReturnsNull();
			var repostitory = Substitute.For<IWeatherRepository>();
			var itemFromRepo = new Forecast()
			{
				City = "lul",
				Country = "LK",
				DailyForecasts = new List<DailyForecast>()
				{
					new DailyForecast()
					{
						Date = new DateTime(2017, 8, 1),
						DayTemperature = 88,
						NightTemperature = 14,
						Summary = "ti lox",
						WindDirection = "lox",
						WindSpeed = "bisrto"
					}
				}
			};
			repostitory.GetDailyForecast(city, duration).Returns(itemFromRepo);
			var controller = new WeatherController(cache, repostitory);
			var expectedItem = new WeatherReport()
			{
				Location = new Location()
				{
					Country = "LK",
					Name = "lul"
				},
				Forecast = new List<Models.DailyForecast>()
				{
					new Models.DailyForecast()
					{
						Date = "2017-08-01",
						Summary = "ti lox",
						Temp = new Temperature()
						{
							Day = 88,
							Night = 14
						},
						Wind = new Wind()
						{
							Speed = "bisrto",
							Direction = "lox"
						}
					}
				}
			};
			var actualReport = controller.Daily(city, duration).Result;
			actualReport.ShouldBeEquivalentTo(expectedItem);
		}

		[Test]
		public void Today_IfCalled_ReturnsExpectedResult()
		{
			var cache = Substitute.For<ICacheService>();
			var reportFromCache = new WeatherReport()
			{
				Location = new Location()
				{
					Country = "HU",
					Name = "Kek"
				}

			};
			var reportFromCache2 = new WeatherReport()
			{
				Location = new Location()
				{
					Country = "LK",
					Name = "Lul"
				}

			};
			cache.Get<WeatherReport>(string.Format("{0}:{1}", 3, "kek"))
				.Returns(reportFromCache);
			cache.Get<WeatherReport>(string.Format("{0}:{1}", 3, "lul"))
				.Returns(reportFromCache2);
			var repostitory = Substitute.For<IWeatherRepository>();
			var controller = new WeatherController(cache, repostitory);
			var expectedResult = new List<WeatherReport>()
			{
				reportFromCache,
				reportFromCache2
			};
			var actualResult = controller.Today("kek,lul", 3).Result;
			actualResult.ShouldAllBeEquivalentTo(expectedResult);
		}
	}
}
