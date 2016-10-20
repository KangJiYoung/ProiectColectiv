using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProiectColectiv.Services
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext dbContext;

        public RolesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<IdentityRole>> GetRoles()
        {
            return dbContext
                .Roles
                .ToListAsync();
        }
    }
}
