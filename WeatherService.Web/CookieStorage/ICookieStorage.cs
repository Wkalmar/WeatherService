using System.Collections.Generic;

namespace WeatherService.Web.CookieStorage
{
	public interface ICookieStorage
	{
		void SaveItems(IEnumerable<string> recentCities, string cookieName);

		IEnumerable<string> GetItems(string cookieName);
	}
}