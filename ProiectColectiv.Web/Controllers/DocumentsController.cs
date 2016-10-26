using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.ViewModel;
using ProiectColectiv.Web.ViewModel.Mapping;

namespace ProiectColectiv.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public DocumentsController(IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var documents = await unitOfWork
                .DocumentsService
                .GetDocumentsByUserId(user.Id);

            ViewBag.Tags = await unitOfWork
                .TagsService
                .GetTagsByUserId(user.Id);

            return View(ViewModelMapping.ConvertToViewModel(documents));
        }

        public IActionResult DocumentUpload() => View(new DocumentUploadViewModel());

        [HttpPost]
        public async Task<IActionResult> DocumentUpload(DocumentUploadViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(HttpContext.User);

            return model.IsTemplate
                ? await DocumentUploadTemplate(model, user)
                : await DocumentUploadFile(model, user);
        }

        private async Task<IActionResult> DocumentUploadFile(DocumentUploadViewModel model, User user)
        {
            await unitOfWork.DocumentsService.AddDocument(user.Id, model.File, model.Tags);

            TempData[Notifications.DOCUMENT_UPLOADED] = "Document adaugat cu success.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> DocumentUploadTemplate(DocumentUploadViewModel model, User user)
        {
            throw new System.NotImplementedException();
        }
    }
}