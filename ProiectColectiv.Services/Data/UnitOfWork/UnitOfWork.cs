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
            IDocumentsService documentsService,
            IRolesService rolesService,
            ITagsService tagsService,
            IUsersService usersService,
            ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            DocumentsService = documentsService;
            RolesService = rolesService;
            TagsService = tagsService;
            UsersService = usersService;
        }

        #region Services

        public IDocumentsService DocumentsService { get; }

        public IRolesService RolesService { get; }

        public ITagsService TagsService { get; }

        public IUsersService UsersService { get; }

        #endregion

        public Task<int> Commit() => dbContext.SaveChangesAsync();
    }
}
