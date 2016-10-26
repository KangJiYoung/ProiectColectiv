using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces
{
    public interface ITagsService
    {
        Task<List<string>> GetTagsByUserId(string userId);
    }
}