using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IPainelDeControleRepository : IDisposable
    {
        Task<PainelDeControle> PainelDeControle(int id);
    }
}
