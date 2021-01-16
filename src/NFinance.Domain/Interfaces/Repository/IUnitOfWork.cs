using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
