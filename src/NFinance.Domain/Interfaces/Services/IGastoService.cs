using NFinance.Model.GastosViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IGastoService
    {
        Task<CadastrarGastoViewModel.Response> CadastrarGasto(CadastrarGastoViewModel.Request request);
        Task<AtualizarGastoViewModel.Response> AtualizarGasto(Guid id, AtualizarGastoViewModel.Request request);
        Task<ExcluirGastoViewModel.Response> ExcluirGasto(ExcluirGastoViewModel.Request request);
        Task<ConsultarGastoViewModel.Response> ConsultarGasto(Guid id);
        Task<ConsultarGastosViewModel.Response> ConsultarGastos(Guid idCliente);
    }
}
