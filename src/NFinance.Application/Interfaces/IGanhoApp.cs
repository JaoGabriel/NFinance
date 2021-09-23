using System;
using System.Threading.Tasks;
using NFinance.Application.ViewModel.GanhoViewModel;

namespace NFinance.Application.Interfaces
{
    public interface IGanhoApp
    {
        public Task<ConsultarGanhoViewModel.Response> ConsultarGanho(Guid idGanho);
        public Task<ConsultarGanhosViewModel.Response> ConsultarGanhos(Guid idCliente);
        public Task<CadastrarGanhoViewModel.Response> CadastrarGanho(CadastrarGanhoViewModel.Request request);
        public Task<AtualizarGanhoViewModel.Response> AtualizarGanho(Guid idGanho, AtualizarGanhoViewModel.Request request);
        public Task<ExcluirGanhoViewModel.Response> ExcluirGanho(ExcluirGanhoViewModel.Request request);
    }
}
