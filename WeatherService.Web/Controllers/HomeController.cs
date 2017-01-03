using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeatherService.Provider;
using WeatherService.Web.Cache;
using WeatherService.Web.Utils;
using WeatherService.Web.ViewModels;

namespace WeatherService.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IWeatherRepository _repository;

		private readonly ICacheService _cache;

		private const int RecentCitiesCapacity = 5;

		private const string RecentCitiesCookieName = "cookie_recent_cities";

		private const char CitiesSeparator = ',';

		private const string CacheKeyPattern = "forecast{0}";

		public HomeController(ICacheService cache, IWeatherRepository repository)
		{
			_repository = repository;
			_cache = cache;
		}

		private IEnumerable<string> GetRecentCities()
		{
			HttpCookie cookie = HttpContext.Request.Cookies.Get(RecentCitiesCookieName);
			if (cookie == null)
			{
				return new List<string>();
			}
			return cookie.Value.Split(CitiesSeparator);
		}

		private void SaveRecentCities(IEnumerable<string> recentCities)
		{
			HttpCookie cookie = HttpContext.Request.Cookies.Get(RecentCitiesCookieName);
			if (cookie == null)
			{
				cookie = new HttpCookie(RecentCitiesCookieName);
			}
			cookie.Value = string.Join(CitiesSeparator.ToString(), recentCities);
			HttpContext.Response.SetCookie(cookie);
		}

		private void BuildRecentCities(string city, WeatherReportViewModel report)
		{
			var recentCities = GetRecentCities();
			if (!string.IsNullOrEmpty(city))
			{
				recentCities = 
					LimitedCapacityCollectionHelper.Add(recentCities, city, RecentCitiesCapacity);
				SaveRecentCities(recentCities);
			}
			report.RecentCities = recentCities;
		}

		private async Task BuildForecast(string city, WeatherReportViewModel report)
		{
			if (!string.IsNullOrEmpty(city))
			{
				var cacheKey = string.Format(CacheKeyPattern, city);
				var forecast = _cache.Get<Forecast>(cacheKey);
				if (forecast == null)
				{
					forecast = await _repository.GetDailyForecast(city);
				}
				report.Forecast = forecast;
				_cache.Save(cacheKey, report);
			}
		}

		public ActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> _Forecast(string city)
		{
			var report = new WeatherReportViewModel();
			await BuildForecast(city, report);
			BuildRecentCities(city, report);
			return PartialView(report);
		}
	}
}
