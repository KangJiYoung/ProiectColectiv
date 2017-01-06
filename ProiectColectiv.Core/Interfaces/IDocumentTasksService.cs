using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentTasksService
    {
        Task Add(string userId, int idDocumentTaskType, IEnumerable<int> idDocuments);
    }
}