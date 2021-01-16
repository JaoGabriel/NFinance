using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IInvestimentosRepository : IDisposable
    {
        Task<Investimentos> RealizarInvestimento(Investimentos investimentos);
        Task<List<Investimentos>> ListarInvestimentos();
        Task<Investimentos> ConsultarInvestimento(int id);
        Task<Investimentos> AtualizarInvestimento(int id, Investimentos investimento);
    }
}
