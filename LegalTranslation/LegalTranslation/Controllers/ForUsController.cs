using Microsoft.AspNetCore.Mvc;

namespace LegalTranslation.Controllers
{
    public class ForUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
