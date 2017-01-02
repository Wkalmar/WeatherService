using System;

namespace WeatherService.Web.Models
{
	public class DailyForecast
	{
		public string Date { get; set; }

		public string Summary { get; set; }

		public Temperature Temp { get; set; }

		public Wind Wind { get; set; }
	}
}