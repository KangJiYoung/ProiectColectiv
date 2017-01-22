using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.ViewModel;
using ProiectColectiv.Web.ViewModel.Mapping;

namespace ProiectColectiv.Web.Controllers
{
    public class DocumentTasksController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public DocumentTasksController(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        
        #region Index

        [Authorize]
        public async Task<IActionResult> GetTasksInitiate(bool final)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var tasks = await unitOfWork.DocumentTasksService.GetByUserId(user.Id, final);

            return PartialView("_Tasks", ViewModelMapping.ConvertToViewModel(tasks));
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var tasks = await unitOfWork.DocumentTasksService.GetByUserId(user.Id);

            return View(ViewModelMapping.ConvertToViewModel(tasks));
        }

        #endregion

        #region Task Details

        [Authorize]
        public async Task<IActionResult> TaskDetails(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var task = await unitOfWork.DocumentTasksService.GetById(id);

            return View(ViewModelMapping.ConvertToDetailViewModel(task, user.IdUserGroup));
        }

        [Authorize]
        public async Task<IActionResult> GetAllDocumentsByTemplate(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var documentTask = await unitOfWork.DocumentTaskTemplatesService.GetById(id);

            return Json(new SelectList(await unitOfWork.DocumentsService.GetDocumentsForTask(user.Id, documentTask.IdDocumentTemplate), nameof(Document.IdDocument), nameof(Document.Name)));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DocumentTaskStatusChange(DocumentTaskStatusChangeViewModel model)
        {
            await unitOfWork.DocumentTasksService.ChangeStatus(model.IdDocumentTask, model.DocumentStatus);
            await unitOfWork.Commit();

            TempData[Notifications.DOCUMENT_TASK_STATUS_CHANGE] = "Document Status a fost schimbat cu succes.";

            return RedirectToAction(nameof(TaskDetails), new { id = model.IdDocumentTask });
        }

        #endregion

        #region Document Tasks Add

        [Authorize]
        public async Task<IActionResult> GetAllDocumentTaskTypes(int id)
            => Json(new SelectList(await unitOfWork.DocumentTaskTemplatesService.GetAllTaskTypes(id), nameof(DocumentTaskType.IdDocumentTaskType), nameof(DocumentTaskType.Name)));

        [Authorize]
        public async Task<IActionResult> DocumentTasksAdd()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            ViewBag.DocumentTaskTemplates = new SelectList(await unitOfWork.DocumentTaskTemplatesService.GetAll(), nameof(DocumentTaskTemplate.IdDocumentTaskTemplate), nameof(DocumentTaskTemplate.Name));
            ViewBag.DocumentTemplates = new SelectList(await unitOfWork.DocumentsTemplateService.GetAllTemplates(), nameof(DocumentTemplate.IdDocumentTemplate), nameof(DocumentTemplate.Name));
            ViewBag.Documents = new SelectList(await unitOfWork.DocumentsService.GetDocumentsForTask(user.Id, null), nameof(Document.IdDocument), nameof(Document.Name));
            ViewBag.UserGroups = new SelectList(await unitOfWork.UserGroupsService.GetAll(), nameof(UserGroup.IdUserGroup), nameof(UserGroup.Name));
            var listaDoc = await unitOfWork.DocumentsService.GetDocumentsForTask(user.Id, null);
            ViewBag.DocumentsDetails = listaDoc.Select(ViewModelMapping.ConvertToDetailViewModel).ToList();
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DocumentTasksAdd(DocumentTaskAddViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(HttpContext.User);

            await unitOfWork.DocumentTasksService.Add(user.Id, model.IdDocumentTaskType.Value, model.OtherDocuments.Concat(new List<int> { model.IdDocumentFromTemplate.Value }));
            await unitOfWork.Commit();

            TempData[Notifications.TASK_ADDED] = "Task adaugat cu success";

            return RedirectToAction(nameof(DocumentTasksAdd));
        }

        #endregion
    }
}