using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.Application.Providers;

namespace ProiectColectiv.Web.Controllers
{
    public class DocumentTaskTemplatesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FileProvider fileManager;
        private readonly UserManager<User> userManager;

        public DocumentTaskTemplatesController(
            IUnitOfWork unitOfWork,
            FileProvider fileManager,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.fileManager = fileManager;
            this.userManager = userManager;
        }

        public IActionResult DocumentTaskTemplateAdd()
        {
            throw new System.NotImplementedException();
        }
    }
}