using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.ViewModel;
using ProiectColectiv.Web.ViewModel.Mapping;
using System.Linq;
using System.Threading.Tasks;

namespace ProiectColectiv.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var tasks = await unitOfWork.DocumentTasksService.GetByUserId(user.Id);
            var documents = await unitOfWork.DocumentsService.GetDocumentsByUserId(user.Id);

            ViewBag.Tags = await unitOfWork.TagsService.GetTagsByUserId(user.Id);

            var model = new DocumentAndTasksViewModel()
            {
                Documents = documents.Select(ViewModelMapping.ConvertToDetailViewModel).ToList(),
                Tasks = tasks.Select(ViewModelMapping.ConvertToViewModel).ToList()
            };

            return View(model);
        }
    }
}