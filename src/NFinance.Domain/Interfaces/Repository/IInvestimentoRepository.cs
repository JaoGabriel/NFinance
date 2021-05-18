using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IInvestimentoRepository : IDisposable
    {
        Task<Investimento> RealizarInvestimento(Investimento investimentos);
        Task<Investimento> ConsultarInvestimento(Guid id);
        Task<List<Investimento>> ConsultarInvestimentos(Guid idCliente);
        Task<Investimento> AtualizarInvestimento(Investimento investimento);
    }
}
