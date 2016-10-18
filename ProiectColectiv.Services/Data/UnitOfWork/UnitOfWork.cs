using System.Threading.Tasks;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<int> Commit() => dbContext.SaveChangesAsync();
    }
}
