using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.ViewModel.Mapping;

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

            return View(ViewModelMapping.ConvertToViewModel(users, userManager));
        }
    }
}