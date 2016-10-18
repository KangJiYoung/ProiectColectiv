using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> Commit();
    }
}