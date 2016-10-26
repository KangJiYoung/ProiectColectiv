using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProiectColectiv.Tests.Mocks
{
    public class FormFileMock : IFormFile
    {
        public Stream OpenReadStream()
            => new MemoryStream(new byte[] {1, 2, 3});

        public void CopyTo(Stream target)
            => new MemoryStream(new byte[] { 1, 2, 3 }).CopyTo(target);

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = new CancellationToken())
            => new MemoryStream(new byte[] { 1, 2, 3 }).CopyToAsync(target);

        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public IHeaderDictionary Headers { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
    }
}
