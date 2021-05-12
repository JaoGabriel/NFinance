using System;
using System.Threading.Tasks;
using NFinance.Application.ViewModel.GastosViewModel;

namespace NFinance.Application.Interfaces
{
    public interface IGastoApp
    {
        public Task<ConsultarGastoViewModel.Response> ConsultarGasto(Guid idGasto);
        public Task<ConsultarGastosViewModel.Response> ConsultarGastos(Guid idCliente);
        public Task<CadastrarGastoViewModel.Response> CadastrarGasto(CadastrarGastoViewModel.Request request);
        public Task<AtualizarGastoViewModel.Response> AtualizarGasto(Guid idGasto, AtualizarGastoViewModel.Request request);
        public Task<ExcluirGastoViewModel.Response> ExcluirGasto(ExcluirGastoViewModel.Request request);

    }
}
