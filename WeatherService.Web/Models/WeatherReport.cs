using System.Collections.Generic;

namespace WeatherService.Web.Models
{
	public class WeatherReport
	{
		public Location Location { get; set; }

		public List<DailyForecast> Forecast { get; set; }
	}
}