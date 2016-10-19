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
            ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            UsersService = usersService;
        }

        #region Services

        public IUsersService UsersService { get; }

        #endregion

        public Task<int> Commit() => dbContext.SaveChangesAsync();
    }
}
