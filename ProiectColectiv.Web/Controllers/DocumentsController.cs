using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.Application.Providers;
using ProiectColectiv.Web.ViewModel;
using ProiectColectiv.Web.ViewModel.Mapping;

namespace ProiectColectiv.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FileProvider fileManager;
        private readonly UserManager<User> userManager;

        public DocumentsController(IUnitOfWork unitOfWork,
            FileProvider fileManager,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.fileManager = fileManager;
            this.userManager = userManager;
        }

        #region Index

        [Authorize]
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

        #endregion

        #region Document Upload

        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public IActionResult DocumentUpload() => View(new DocumentUploadViewModel());

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
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
            var fileData = await fileManager.GetFileBytes(model.File);

            await unitOfWork.DocumentsService.AddDocument(user.Id, model.File.FileName, fileData, model.Tags);
            await unitOfWork.Commit();

            TempData[Notifications.DOCUMENT_UPLOADED] = "Document adaugat cu success.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> DocumentUploadTemplate(DocumentUploadViewModel model, User user)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Document Details

        [Authorize]
        public async Task<IActionResult> DocumentDetails(int id)
        {
            var document = await unitOfWork.DocumentsService.GetDocumentById(id);
            if (document == null)
                return NotFound();

            return View(ViewModelMapping.ConvertToDetailViewModel(document));
        }

        #endregion

        #region Document Delete

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<JsonResult> DocumentDelete(int id)
        {
            var user = userManager.GetUserAsync(HttpContext.User);
            var document = unitOfWork.DocumentsService.GetDocumentById(id);

            await Task.WhenAll(user, document);

            if (document.Result.UserId != user.Result.Id && !await userManager.IsInRoleAsync(user.Result, Roles.ADMINISTRATOR))
                return Json(new { success = false, message = "Nu aveti destule drepturi!" });

            await unitOfWork.DocumentsService.DeleteDocumentById(id);

            return Json(new { success = true, message = "Documentul a fost sters cu success!" });
        }

        #endregion
    }
}