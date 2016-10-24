using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentsService
    {
        Task AddDocument(string userId, string fileName, IList<string> tags);
    }
}