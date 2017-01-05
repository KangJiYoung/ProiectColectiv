using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.Application.Providers;
using ProiectColectiv.Web.ViewModel;

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

        public async Task<IActionResult> DocumentTaskTemplateAdd(DocumentTaskTemplateAddViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_DocumentTaskTemplateAdd", model);

            unitOfWork.DocumentTaskTemplatesService.Add(model.Name, model.IdDocumentTemplate.Value, model.Types.ToDictionary(it => it.Name, it => it.Paths));
            await unitOfWork.Commit();

            return Json(new { message = "Task template adaugat cu success" });
        }

        public async Task<IActionResult> GetAllDocumentTaskTemplates()
            => Json(new SelectList(await unitOfWork.DocumentTaskTemplatesService.GetAll(), nameof(DocumentTaskTemplate.IdDocumentTemplate), nameof(DocumentTaskTemplate.Name)));

        public IActionResult GetTaskTemplateType(int index) => PartialView("_TaskTemplateType", index);
    }
}