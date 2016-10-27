using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProiectColectiv.Web.Application.Providers
{
    public class FileProvider
    {
        public async Task<byte[]> GetFileBytes(IFormFile file)
        {
            var dataStream = new MemoryStream();
            await file.CopyToAsync(dataStream);

            return dataStream.ToArray();
        }
    }
}
