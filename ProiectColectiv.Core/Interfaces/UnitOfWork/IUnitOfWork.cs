using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Services

        IUsersService UsersService { get; }

        IRolesService RolesService { get; }

        #endregion

        Task<int> Commit();
    }
}