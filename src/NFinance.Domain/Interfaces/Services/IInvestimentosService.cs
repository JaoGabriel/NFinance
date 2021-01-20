using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IInvestimentosService
    {
        Task<Investimentos> RealizarInvestimento(Investimentos investimentos);
        Task<List<Investimentos>> ListarInvestimentos();
        Task<Investimentos> ConsultarInvestimento(Guid id);
        Task<Investimentos> AtualizarInvestimento(Guid id,Investimentos investimento);
    }
}
