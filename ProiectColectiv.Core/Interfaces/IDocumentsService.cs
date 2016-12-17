using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentsService
    {
        Task<List<Document>> GetDocumentsByUserId(string userId);
        Task<Document> GetDocumentById(int idDocument);
        Task DeleteDocumentById(int idDocument);
        Task AddDocument(string userId, string name, byte[] data, IList<string> tags);
        Task AddDocumentFromTemplate(string userId, int idTemplate, string name, string @abstract, IList<string> tags, IDictionary<int, string> items);
    }
}