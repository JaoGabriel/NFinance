using System;
using System.Threading.Tasks;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.Application.Interfaces
{
    public interface IInvestimentoApp
    {
        public Task<ConsultarInvestimentoViewModel.Response> ConsultarInvestimento(Guid idInvestimento);
        public Task<ConsultarInvestimentosViewModel.Response> ConsultarInvestimentos(Guid idCliente);
        public Task<RealizarInvestimentoViewModel.Response> RealizarInvestimento(RealizarInvestimentoViewModel.Request request);
        public Task<AtualizarInvestimentoViewModel.Response> AtualizarInvestimento(Guid idInvestimento, AtualizarInvestimentoViewModel.Request request);
    }
}
