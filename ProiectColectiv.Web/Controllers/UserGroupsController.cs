using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.ViewModel;

namespace ProiectColectiv.Web.Controllers
{
    public class UserGroupsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public UserGroupsController(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> UserGroupAdd(UserGroupAddViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_UserGroupAdd", model);

            unitOfWork.UserGroupsService.Add(model.Name);
            await unitOfWork.Commit();

            return Json(new {success = true, message = "User Grup a fost adaugat cu success."});
        }

        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> GetAllUserGroups()
            => Json(new SelectList(await unitOfWork.UserGroupsService.GetAll(), nameof(UserGroup.IdUserGroup), nameof(UserGroup.Name)));
    }
}