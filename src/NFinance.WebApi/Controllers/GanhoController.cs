using Microsoft.AspNetCore.Mvc;

namespace NFinance.WebApi.Controllers
{
    public class GanhoController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}