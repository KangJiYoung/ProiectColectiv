using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.ViewModel.Mapping;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProiectColectiv.Web.ViewModel;

namespace ProiectColectiv.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public UsersController(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [Authorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<IActionResult> Index()
        {
            var users = await unitOfWork
                .UsersService
                .GetUsers();

            var roles = await unitOfWork
                .RolesService
                .GetRoles();

            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            return View(ViewModelMapping.ConvertToViewModel(users, userManager));
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<IActionResult> ChangeRole(RoleChangeViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_RoleChange", model);

            var user = await unitOfWork
                .UsersService
                .GetUser(model.UserId);

            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);
            await userManager.AddToRoleAsync(user, model.Role);

            TempData[Notifications.ROLE_CHANGED] = "Rol Utilizator schimbat cu success.";

            return RedirectToAction(nameof(Index));
        }
    }
}