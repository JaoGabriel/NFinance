using System;
using System.Threading.Tasks;
using NFinance.Application.ViewModel.ClientesViewModel;

namespace NFinance.Application.Interfaces
{
    public interface IClienteApp
    {
        public Task<ConsultarClienteViewModel.Response> ConsultaCliente(Guid id);
        public Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request request);
        public Task<AtualizarClienteViewModel.Response> AtualizarDadosCadastrais(Guid id, AtualizarClienteViewModel.Request request);
    }
}
