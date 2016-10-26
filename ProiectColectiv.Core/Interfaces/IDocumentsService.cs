using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentsService
    {
        Task AddDocument(string userId, IFormFile file, IList<string> tags);
        Task<List<Document>> GetDocumentsByUserId(string userId);
    }
}