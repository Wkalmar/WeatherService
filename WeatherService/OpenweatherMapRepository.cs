using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherService.Provider
{
	public class OpenWeatherMapRepository : IWeatherRepository
	{
		private const string RequestUrlPattern =
			"http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&mode=json&units=metric&cnt={1}&apikey={2}";

		private const int DefaultForecastDuration = 3;

		private const string ApiKey = "94b7c9a9908d65670675b7edd72079fc";

		private async Task<string> GetWeatherDataFromApi(string url)
		{
			var client = new HttpClient();
			return await client.GetStringAsync(url);
		}

		private Forecast BuildForecast(string weatherData)
		{
			var forecast = new Forecast();
			var token = JObject.Parse(weatherData);
			forecast.City = (string)token.SelectToken("city.name");
			forecast.Country = (string)token.SelectToken("city.country");
			var list = (JArray)token.SelectToken("list");
			forecast.DailyForecasts = new List<DailyForecast>();
			foreach (var item in list)
			{
				forecast.DailyForecasts.Add(new DailyForecast()
				{
					Date = Utils.TimestampToDateTime((int)item.SelectToken("dt")),
					DayTemperature = (int)item.SelectToken("temp.day"),
					NightTemperature = (int)item.SelectToken("temp.night"),
					Summary = (string)item.SelectToken("weather[0].main"),
					WindDirection = Utils.DegreeToDirection((int)item.SelectToken("deg")),
					WindSpeed = Utils.WindSpeedToDescription((double)item.SelectToken("speed"))
				});
			}
			return forecast;
		}

		public async Task<Forecast> GetDailyForecast(string city)
		{
			return await GetDailyForecast(city, DefaultForecastDuration);
		}

		public async Task<Forecast> GetDailyForecast(string city, int duration)
		{
			try
			{
				var url = String.Format(RequestUrlPattern, city, duration, ApiKey);
				var data = await GetWeatherDataFromApi(url);
				return BuildForecast(data);
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}
}
