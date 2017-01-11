using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentTasksService
    {
        Task Add(string userId, int idDocumentTaskType, IEnumerable<int> idDocuments);
        Task<List<DocumentTask>> GetByUserId(string userId, bool final = false);
        Task<DocumentTask> GetById(int id);
        Task ChangeStatus(int idDocumentTask, DocumentTaskStatus documentStatus);
    }
}