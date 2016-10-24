using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProiectColectiv.Web.Application.Providers
{
    public class FileProvider
    {
        public async Task UploadFile(IFormFile file, string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var fileStream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(fileStream);
        }
    }
}
