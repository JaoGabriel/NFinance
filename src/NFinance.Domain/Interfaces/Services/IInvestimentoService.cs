using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IInvestimentoService
    {
        Task<Investimento> AtualizarInvestimento(Guid id, Investimento request);
        Task<Investimento> ConsultarInvestimento(Guid id);
        Task<List<Investimento>> ConsultarInvestimentos(Guid idCliente);
        Task<Investimento> RealizarInvestimento(Investimento request);
    }
}
