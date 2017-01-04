using System.Collections.Generic;
using System.Web;

namespace WeatherService.Web.CookieStorage
{
	public class CookieStorage : ICookieStorage
	{
		private const char CitiesSeparator = ',';
		
		public void SaveItems(IEnumerable<string> recentCities, string cookieName)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
			if (cookie == null)
			{
				cookie = new HttpCookie(cookieName);
			}
			cookie.Value = string.Join(CitiesSeparator.ToString(), recentCities);
			HttpContext.Current.Response.SetCookie(cookie);
		}

		public IEnumerable<string> GetItems(string cookieName)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
			if (cookie == null)
			{
				return new List<string>();
			}
			return cookie.Value.Split(CitiesSeparator);
		}
	}
}