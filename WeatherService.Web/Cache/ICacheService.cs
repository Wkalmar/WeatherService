using System.Web;

namespace WeatherService.Web.Cache
{
	public interface ICacheService
	{
		void Save(string key, object value);

		T Get<T>(string key) where T : class;
	}
}
