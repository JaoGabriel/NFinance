using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IInvestimentosService
    {
        Task<Investimentos> RealizarInvestimento(Investimentos investimentos);
        Task<List<Investimentos>> ListarInvestimentos();
        Task<Investimentos> ConsultarInvestimento(int id);
        Task<Investimentos> AtualizarInvestimento(int id,Investimentos investimento);
    }
}
