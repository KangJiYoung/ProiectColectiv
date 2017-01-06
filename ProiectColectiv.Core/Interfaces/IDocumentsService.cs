using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentsService
    {
        Task<List<Document>> GetDocumentsByUserAndTemplate(string userId, int? idDocumentTemplate);
        Task<List<Document>> GetDocumentsByUserId(string userId);
        Task<Document> GetDocumentById(int idDocument);
        Task DeleteDocumentById(int idDocument);
        Task AddDocument(string userId, string name, string @abstract, byte[] data, IList<string> tags);
        Task AddDocumentFromTemplate(string userId, int idTemplate, string name, string @abstract, IList<string> tags, IDictionary<int, string> items);
        Task AddDocumentNewVersion(string userId, int idDocument, byte[] data);
        Task ChangeStatus(int idDocument, DocumentStatus documentStatus);
        Task EditDocument(int idDocument, IDictionary<int, string> items);
    }
}