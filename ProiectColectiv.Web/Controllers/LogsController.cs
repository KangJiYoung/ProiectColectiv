using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.ViewModel;
using ProiectColectiv.Web.ViewModel.Mapping;

namespace ProiectColectiv.Web.Controllers
{
    public class LogsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public LogsController(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<IActionResult> Index()
        {
            var logs = await unitOfWork
                .LogsService
                .GetAll();

            ViewBag.Users = new SelectList(await unitOfWork.UsersService.GetAll(), nameof(Core.DomainModel.Entities.User.Id), nameof(Core.DomainModel.Entities.User.UserName));

            return View(ViewModelMapping.ConvertToViewModel(logs));
        }

        [Authorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<IActionResult> LogFilter(LogFilterViewModel model)
        {
            var logs = await unitOfWork
                .LogsService
                .GetFiltered(model.UserId, model.Date);

            return PartialView("_Logs", ViewModelMapping.ConvertToViewModel(logs));
        }
    }
}