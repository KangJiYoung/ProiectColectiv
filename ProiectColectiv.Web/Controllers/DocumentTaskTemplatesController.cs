using System;
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

namespace ProiectColectiv.Web.Controllers
{
    public class DocumentTaskTemplatesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public DocumentTaskTemplatesController(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> DocumentTaskTemplateAdd(DocumentTaskTemplateAddViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_DocumentTaskTemplateAdd", model);

            var user = await userManager.GetUserAsync(HttpContext.User);

            unitOfWork.LogsService.Add(user.Id, $"Adaugare template flux: {model.Name}");
            unitOfWork.DocumentTaskTemplatesService.Add(
                model.Name, 
                model.IdDocumentTemplate.Value, 
                model.Types.ToDictionary(it => new Tuple<string, int>(it.Name, it.DaysLimit), it => it.Paths));
            await unitOfWork.Commit();

            return Json(new { message = "Task template adaugat cu success" });
        }

        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public async Task<IActionResult> GetAllDocumentTaskTemplates()
            => Json(new SelectList(await unitOfWork.DocumentTaskTemplatesService.GetAll(), nameof(DocumentTaskTemplate.IdDocumentTaskTemplate), nameof(DocumentTaskTemplate.Name)));

        [Authorize(Roles = Roles.ADMINISTRATOR + "," + Roles.CONTRIBUTOR + "," + Roles.MANAGER)]
        public IActionResult GetTaskTemplateType(int index) => PartialView("_TaskTemplateType", index);
    }
}