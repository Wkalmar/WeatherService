using System.Collections.Generic;

namespace WeatherService.Provider
{
	public class Forecast
	{
		public string City { get; set; }

		public string Country { get; set; }

		public List<DailyForecast> DailyForecasts { get; set; }
	}
}
