using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Services

        IUsersService UsersService { get; }

        #endregion

        Task<int> Commit();
    }
}