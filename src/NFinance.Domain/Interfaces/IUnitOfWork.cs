using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
