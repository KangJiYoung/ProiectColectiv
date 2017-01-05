using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentTaskTemplatesService
    {
        Task<List<DocumentTaskTemplate>> GetAll();
        void Add(string name, int idDocumentTemplate, IDictionary<string, IList<int>> paths);
    }
}