using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentsService
    {
        Task AddDocument(string userId, string name, byte[] data, IList<string> tags);
        Task<List<Document>> GetDocumentsByUserId(string userId);
        Task<Document> GetDocumentById(int idDocument);
    }
}