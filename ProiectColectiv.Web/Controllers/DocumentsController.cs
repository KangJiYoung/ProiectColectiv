using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> DocumentUpload()
        {
            ViewBag.DocumentTemplates = new SelectList(await unitOfWork.DocumentsTemplateService.GetAllTemplates(), nameof(DocumentTemplate.IdDocumentTemplate), nameof(DocumentTemplate.Name));

            return View(new DocumentUploadViewModel());
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> DocumentUpload(DocumentUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DocumentTemplates = new SelectList(await unitOfWork.DocumentsTemplateService.GetAllTemplates(), nameof(DocumentTemplate.IdDocumentTemplate), nameof(DocumentTemplate.Name));

                return View(model);
            }

            var user = await userManager.GetUserAsync(HttpContext.User);

            string fileName;
            if (model.IsTemplate)
            {
                var itemsDict = model.Items.ToDictionary(it => it.IdDocumentTemplateItem, it => it.Value);
                fileName = model.DocumentName;

                await unitOfWork.DocumentsService.AddDocumentFromTemplate(user.Id, model.IdTemplate.Value, fileName, model.Abstract, model.Tags, itemsDict);
            }
            else
            {
                var fileData = await fileManager.GetFileBytes(model.File);
                fileName = model.File.FileName;

                await unitOfWork.DocumentsService.AddDocument(user.Id, fileName, model.Abstract, fileData, model.Tags);
            }

            unitOfWork.LogsService.Add(user.Id, $"Incarcat document: {fileName}");
            await unitOfWork.Commit();

            TempData[Notifications.DOCUMENT_UPLOADED] = "Document adaugat cu success.";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> GetDocumentTemplateItems(int id)
        {
            var items = await unitOfWork.DocumentsTemplateItemService.GetItemsFromTemplate(id);

            return PartialView("_DocumentUploadTemplateItems", ViewModelMapping.ConvertToViewModel(items));
        }

        #endregion

        #region Document Upload New Version

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> DocumentUploadNewVersion(DocumentUploadNewVersionViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_DocumentUploadNewVersion", model);

            var user = await userManager.GetUserAsync(HttpContext.User);
            var fileData = await fileManager.GetFileBytes(model.File);

            unitOfWork.LogsService.Add(user.Id, $"Adaugare versiune noua la document: {model.IdDocument.Value}");

            await unitOfWork.DocumentsService.AddDocumentNewVersion(user.Id, model.IdDocument.Value, fileData);
            await unitOfWork.Commit();

            TempData[Notifications.DOCUMENT_UPLOADED_NEW_VERSION] = "Versiune noua adaugata cu success.";

            return RedirectToAction(nameof(DocumentDetails), new { id = model.IdDocument });
        }

        #endregion

        #region Document Details

        [Authorize]
        public async Task<IActionResult> DocumentDetails(int id)
        {
            var document = await unitOfWork.DocumentsService.GetDocumentById(id);
            if (document == null)
                return NotFound();

            ViewBag.Versions = new SelectList(document.DocumentStates, nameof(DocumentState.IdDocumentState), nameof(DocumentState.Version));

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

            unitOfWork.LogsService.Add(user.Result.Id, $"Stergere document: {id}");

            await unitOfWork.DocumentsService.DeleteDocumentById(id);
            await unitOfWork.Commit();

            return Json(new { success = true, message = "Documentul a fost sters cu success!" });
        }

        #endregion

        #region Document Status Change

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> DocumentStatusChange(DocumentStatusChangeViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            unitOfWork.LogsService.Add(user.Id, $"Schimbare status document: {model.IdDocument.Value} in {model.DocumentStatus}");

            await unitOfWork.DocumentsService.ChangeStatus(model.IdDocument.Value, model.DocumentStatus);
            await unitOfWork.Commit(); 

            TempData[Notifications.DOCUMENT_VERSION_CHANGED] = "Versiunea documentului a fost schimbata cu success.";

            return RedirectToAction(nameof(DocumentDetails), new { id = model.IdDocument });
        }

        #endregion

        #region Document Download

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> DocumentDownload(DocumentDownloadViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_DocumentDownload", model);

            var state = await unitOfWork.DocumentsStatesService.GetDocumentStateById(model.IdDocumentState.Value, model.IsFromTemplate);
            if (!model.IsFromTemplate)
                return File(((DocumentDataUpload)state.DocumentData).Data, MediaTypes.MEDIA_TYPE_PDF, state.Document.Name);

            var documentData = (DocumentDataTemplate)state.DocumentData;
            var template = state.Document.DocumentTemplate;

            var stream = new MemoryStream();
            var reader = new PdfReader(template.Data);
            var stamper = new PdfStamper(reader, stream);
            foreach (var item in documentData.DocumentDataTemplateItems)
                stamper.AcroFields.SetField(item.DocumentTemplateItem.Label, item.Value);
            stamper.FormFlattening = true;
            stamper.Close();

            return File(stream.ToArray(), MediaTypes.MEDIA_TYPE_PDF, template.Name + FileExtensions.PDF_EXTENSION);
        }

        #endregion

        #region Document Edit

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> DocumentEdit(DocumentEditViewModel model)
        {
            var items = model.Items.ToDictionary(it => it.IdDocumentTemplateItem, it => it.Value);
            var user = await userManager.GetUserAsync(HttpContext.User);

            unitOfWork.LogsService.Add(user.Id, $"Editare document: {model.IdDocument}");

            await unitOfWork.DocumentsService.EditDocument(model.IdDocument, user.Id, items);
            await unitOfWork.Commit();

            TempData[Notifications.DOCUMENT_EDIT] = "Documentul a fost editat cu success.";

            return RedirectToAction(nameof(DocumentDetails), new { id = model.IdDocument });
        }

        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> GetDocumentDataTemplateItems(int id)
        {
            var items = await unitOfWork.DocumentsStatesService.GetDocumentDataTemplateItems(id);

            return PartialView("_DocumentUploadTemplateItems", ViewModelMapping.ConvertToViewModel(items));
        }

        #endregion

        #region Document Digitally Sign

        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> DocumentDigitallySign(DigitallySignViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_DigitallySign", model);

            var user = await userManager.GetUserAsync(HttpContext.User);
            var certificateData = await fileManager.GetFileBytes(model.Certificate);

            await unitOfWork.DocumentsService.DigitallySign(model.IdDocument, user.Id, model.Password, certificateData, model.Reason, model.Location);
            await unitOfWork.Commit();

            TempData[Notifications.DOCUMENT_DIGITALLY_SIGNED] = "Documentul a fost semnat cu success.";

            return RedirectToAction(nameof(DocumentDetails), new { id = model.IdDocument });
        }

        #endregion
    }
}