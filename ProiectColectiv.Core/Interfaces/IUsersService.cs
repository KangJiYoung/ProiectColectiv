using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IUsersService
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(string userId);
    }
}