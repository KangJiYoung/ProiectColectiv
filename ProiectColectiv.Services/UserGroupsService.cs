using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class UserGroupsService : IUserGroups
    {
        private readonly ApplicationDbContext dbContext;

        public UserGroupsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<UserGroup>> GetAll()
        {
            return dbContext
                .UserGroups
                .ToListAsync();
        }

        public void Add(string name)
        {
            dbContext.UserGroups.Add(new UserGroup { Name = name });
        }
    }
}