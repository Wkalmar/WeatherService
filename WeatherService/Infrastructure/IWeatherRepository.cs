using System.Threading.Tasks;

namespace WeatherService.Provider
{
	public interface IWeatherRepository
	{
		Task<Forecast> GetDailyForecast(string city);

		Task<Forecast> GetDailyForecast(string city, int duration);
	}
}
