using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Web.Application.Providers;
using ProiectColectiv.Web.ViewModel;

namespace ProiectColectiv.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly FileProvider fileProvider;
        private readonly UserManager<User> userManager;

        public DocumentsController(IUnitOfWork unitOfWork,
            IHostingEnvironment hostingEnvironment,
            FileProvider fileProvider,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
            this.fileProvider = fileProvider;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
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
            //var filePath = Path.Combine(hostingEnvironment.WebRootPath, FilePath.DOCUMENTS, user.Id, model.File.FileName);

            //await fileProvider.UploadFile(model.File, filePath);

            await unitOfWork.DocumentsService.AddDocument(user.Id, model.File.FileName, model.Tags);

            TempData[Notifications.DOCUMENT_UPLOADED] = "Document adaugat cu success.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> DocumentUploadTemplate(DocumentUploadViewModel model, User user)
        {
            throw new System.NotImplementedException();
        }
    }
}