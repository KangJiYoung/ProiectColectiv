using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Services

        IDocumentsService DocumentsService { get; }

        IRolesService RolesService { get; }

        IUsersService UsersService { get; }

        #endregion

        Task<int> Commit();
    }
}