using System;
using System.Threading.Tasks;
using NFinance.Domain.ViewModel.GanhoViewModel;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IGanhoService
    {
        Task<CadastrarGanhoViewModel.Response> CadastrarGanho(CadastrarGanhoViewModel.Request request);
        Task<AtualizarGanhoViewModel.Response> AtualizarGanho(Guid id, AtualizarGanhoViewModel.Request request);
        Task<ConsultarGanhoViewModel.Response> ConsultarGanho(Guid id);
        Task<ConsultarGanhosViewModel.Response> ConsultarGanhos(Guid idCliente);
        Task<ExcluirGanhoViewModel.Response> ExcluirGanho(ExcluirGanhoViewModel.Request request);
    }
}