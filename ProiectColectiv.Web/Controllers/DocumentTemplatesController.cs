using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.Application.Providers;
using ProiectColectiv.Web.ViewModel;

namespace ProiectColectiv.Web.Controllers
{
    public class DocumentTemplatesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FileProvider fileManager;
        private readonly UserManager<User> userManager;

        public DocumentTemplatesController(IUnitOfWork unitOfWork,
            FileProvider fileManager,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.fileManager = fileManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> TemplateUpload(TemplateUploadViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_DocumentTemplateUpload", model);

            unitOfWork.DocumentsTemplateService.AddTemplate(model.Name, await fileManager.GetFileBytes(model.File));
            await unitOfWork.Commit();

            return Json(new { success = true, message = "Template adaugat cu success" });
        }

        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> GetAllTemplates() 
            => Json(new SelectList(await unitOfWork.DocumentsTemplateService.GetAllTemplates(), nameof(DocumentTemplate.IdDocumentTemplate), nameof(DocumentTemplate.Name)));
    }
}