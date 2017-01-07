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
using ProiectColectiv.Web.Application.Providers;
using ProiectColectiv.Web.ViewModel;

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

        [Authorize]
        public async Task<IActionResult> GetAllDocumentsByTemplate(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var documentTask = await unitOfWork.DocumentTaskTemplatesService.GetById(id);

            return Json(new SelectList(await unitOfWork.DocumentsService.GetDocumentsForTask(user.Id, documentTask.IdDocumentTemplate), nameof(Document.IdDocument), nameof(Document.Name)));
        }

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
    }
}