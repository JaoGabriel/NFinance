using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IInvestimentoRepository : IDisposable
    {
        Task<Investimento> RealizarInvestimento(Investimento investimentos);
        Task<Investimento> ConsultarInvestimento(Guid id);
        Task<List<Investimento>> ConsultarInvestimentos(Guid idCliente);
        Task<Investimento> AtualizarInvestimento(Guid id, Investimento investimento);
    }
}
