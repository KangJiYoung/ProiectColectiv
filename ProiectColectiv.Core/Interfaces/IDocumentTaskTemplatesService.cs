using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentTaskTemplatesService
    {
        Task<List<DocumentTaskTemplate>> GetAll();
        void Add(string name, int idDocumentTemplate, IDictionary<string, IList<int>> paths);
        Task<List<DocumentTaskType>> GetAllTaskTypes(int idDocumentTaskTemplate);
        Task<DocumentTaskTemplate> GetById(int idDocumentTaskTemplate);
    }
}