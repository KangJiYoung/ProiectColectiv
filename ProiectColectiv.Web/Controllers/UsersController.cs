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
                .GetAll();

            var roles = await unitOfWork
                .RolesService
                .GetAll();

            var groups = await unitOfWork
                .UserGroupsService
                .GetAll();

            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            ViewBag.UserGroups = new SelectList(groups, nameof(UserGroup.IdUserGroup), nameof(UserGroup.Name));

            return View(ViewModelMapping.ConvertToViewModel(users, userManager));
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR)]
        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_UserEdit", model);

            var user = await unitOfWork
                .UsersService
                .GetUser(model.UserId);

            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);
            await userManager.AddToRoleAsync(user, model.Role);

            if (user.IdUserGroup != model.IdUserGroup.Value)
            {
                user.IdUserGroup = model.IdUserGroup.Value;
                await userManager.UpdateAsync(user);
            }

            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            unitOfWork.LogsService.Add(currentUser.Id, $"Modificare utilizator: {user.UserName}");
            await unitOfWork.Commit();

            TempData[Notifications.USER_EDITED] = "Utilizatorul a fost modificat cu success.";

            return RedirectToAction(nameof(Index));
        }
    }
}