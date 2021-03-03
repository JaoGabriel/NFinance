using NFinance.Model.InvestimentosViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IInvestimentoService
    {
        Task<RealizarInvestimentoViewModel.Response> RealizarInvestimento(RealizarInvestimentoViewModel.Request request);
        Task<ConsultarInvestimentoViewModel.Response> ConsultarInvestimento(Guid id);
        Task<ConsultarInvestimentosViewModel.Response> ConsultarInvestimentos(Guid idCliente);
        Task<AtualizarInvestimentoViewModel.Response> AtualizarInvestimento(Guid id, AtualizarInvestimentoViewModel.Request request);
    }
}
