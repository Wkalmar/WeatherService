using System;

namespace WeatherService.Provider
{
	public class DailyForecast
	{
		public DateTime Date { get; set; }

		public string Summary { get; set; }

		public int DayTemperature { get; set; }

		public int NightTemperature { get; set; }

		public string WindDirection { get; set; }

		public string WindSpeed { get; set; }
	}
}
