using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IInvestimentosRepository : IDisposable
    {
        Task<Investimentos> RealizarInvestimento(Investimentos investimentos);
        Task<List<Investimentos>> ListarInvestimentos();
        Task<Investimentos> ConsultarInvestimento(Guid id);
        Task<List<Investimentos>> ConsultarInvestimentos(Guid idCliente);
        Task<Investimentos> AtualizarInvestimento(Guid id, Investimentos investimento);
    }
}
