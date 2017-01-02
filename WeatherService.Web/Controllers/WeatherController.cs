using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WeatherService.Provider;
using WeatherService.Web.Cache;
using WeatherService.Web.Models;
using DailyForecast = WeatherService.Web.Models.DailyForecast;

namespace WeatherService.Web.Controllers
{
	public class WeatherController : ApiController
	{
		private readonly IWeatherRepository _repository;

		private readonly ICacheService _cache;

		private const string CacheKeyPattern = "{0}:{1}";

		public WeatherController(ICacheService cache, IWeatherRepository repository)
		{
			_repository = repository;
			_cache = cache;
		}

		private WeatherReport BuildWeatherReport(Forecast forecast)
		{
			WeatherReport report = null;
			if (forecast != null)
			{
				report = new WeatherReport();
				report.Location = new Location()
				{
					Country = forecast.Country,
					Name = forecast.City
				};
				report.Forecast = new List<DailyForecast>();
				foreach (var dailyForecast in forecast.DailyForecasts)
				{
					var temperature = new Temperature()
					{
						Day = dailyForecast.DayTemperature,
						Night = dailyForecast.NightTemperature
					};
					var wind = new Wind()
					{
						Direction = dailyForecast.WindDirection,
						Speed = dailyForecast.WindSpeed
					};

					report.Forecast.Add(new DailyForecast()
					{
						Date = dailyForecast.Date.ToString("yyyy-MM-dd"),
						Summary = dailyForecast.Summary,
						Temp = temperature,
						Wind = wind
					});
				}
			}
			return report;
		}

		private async Task<WeatherReport> InternalGetWeatherReport(string city, int count)
		{
			var cacheKey = string.Format(CacheKeyPattern, count, city);
			var report = _cache.Get<WeatherReport>(cacheKey);
			if (report == null)
			{
				var forecast = await _repository.GetDailyForecast(city, count);
				report = BuildWeatherReport(forecast);
				_cache.Save(cacheKey, report);
			}
			return report;
		}

		[HttpGet]
		public async Task<WeatherReport> Daily(string city, int count = 3)
		{
			return await InternalGetWeatherReport(city, count);
		}

		[HttpGet]
		public async Task<IEnumerable<WeatherReport>> Today(string cities, int count = 3)
		{
			var cityArray = cities.Split(',');
			var tasks = new List<Task<WeatherReport>>();
			foreach (var city in cityArray)
			{
				tasks.Add(Task.Run(async () =>
				{
					var report = await InternalGetWeatherReport(city, count);
					return report;
				}));

			}
			var reports = (await Task.WhenAll(tasks)).Where(p => p != null).ToList();
			return reports;
		}
	}
}