using System.Threading.Tasks;

namespace WeatherService.Provider
{
	interface IWeatherRepository
	{
		Task<Forecast> GetDailyForecast(string city);

		Task<Forecast> GetDailyForecast(string city, int duration);
	}
}
