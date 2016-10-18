using Microsoft.AspNetCore.Mvc;
using ProiectColectiv.Core.Interfaces.UnitOfWork;

namespace ProiectColectiv.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}