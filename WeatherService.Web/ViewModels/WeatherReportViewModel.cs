using System.Collections.Generic;
using WeatherService.Provider;

namespace WeatherService.Web.ViewModels
{
	public class WeatherReportViewModel
	{
		public IEnumerable<string> RecentCities { get; set; }

		public Forecast Forecast { get; set; }
	}
}