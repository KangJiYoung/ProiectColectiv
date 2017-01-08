using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentTasksService
    {
        Task Add(string userId, int idDocumentTaskType, IEnumerable<int> idDocuments);
        Task<List<DocumentTask>> GetByUserId(string userId);
    }
}