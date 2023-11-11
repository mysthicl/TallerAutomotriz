using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class TeamController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
