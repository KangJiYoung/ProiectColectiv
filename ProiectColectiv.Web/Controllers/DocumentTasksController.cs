using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.Application.Providers;

namespace ProiectColectiv.Web.Controllers
{
    public class DocumentTasksController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FileProvider fileManager;
        private readonly UserManager<User> userManager;

        public DocumentTasksController(
            IUnitOfWork unitOfWork,
            FileProvider fileManager,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.fileManager = fileManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> DocumentTasksAdd()
        {
            ViewBag.DocumentTaskTemplates = new SelectList(await unitOfWork.DocumentTaskTemplatesService.GetAll(), nameof(DocumentTaskTemplate.IdDocumentTemplate), nameof(DocumentTaskTemplate.Name));
            ViewBag.DocumentTemplates = new SelectList(await unitOfWork.DocumentsTemplateService.GetAllTemplates(), nameof(DocumentTemplate.IdDocumentTemplate), nameof(DocumentTemplate.Name));

            return View();
        }
    }
}