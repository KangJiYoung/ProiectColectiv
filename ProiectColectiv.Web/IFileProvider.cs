using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProiectColectiv.Web.Application.Providers
{
    public interface IFileProvider
    {
        Task UploadFile(IFormFile file, string path);
    }
}