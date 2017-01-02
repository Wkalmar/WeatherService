using System.Web.Mvc;
using WeatherService.Provider;

namespace WeatherService.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IWeatherRepository _repository;

		public HomeController(IWeatherRepository repository)
		{
			_repository = repository;
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}
