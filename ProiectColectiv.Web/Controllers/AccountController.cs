using Microsoft.AspNetCore.Mvc;

namespace ProiectColectiv.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}