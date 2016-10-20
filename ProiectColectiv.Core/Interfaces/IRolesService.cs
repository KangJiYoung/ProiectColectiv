using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IRolesService
    {
        Task<List<IdentityRole>> GetRoles();
    }
}