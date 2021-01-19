using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IPainelDeControleService
    {
        Task<PainelDeControle> PainelDeControle(int id);
    }
}
