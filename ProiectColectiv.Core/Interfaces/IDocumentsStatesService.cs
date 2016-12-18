using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentsStatesService
    {
        Task<List<DocumentTemplateStateItem>> GetDocumentTemplateStateitems(int idDocument);
    }
}