using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;

        public UsersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<User> GetUser(string userId)
        {
            return dbContext
                .Users
                .FirstAsync(it => it.Id == userId);
        }

        public Task<List<User>> GetUsers()
        {
            return dbContext
                .Users
                .ToListAsync();
        }
    }
}
