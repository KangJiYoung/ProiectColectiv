using System.Threading.Tasks;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(
            IUsersService usersService,
            IRolesService rolesService,
            ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            UsersService = usersService;
            RolesService = rolesService;
        }

        #region Services

        public IUsersService UsersService { get; }

        public IRolesService RolesService { get; }

        #endregion

        public Task<int> Commit() => dbContext.SaveChangesAsync();
    }
}
