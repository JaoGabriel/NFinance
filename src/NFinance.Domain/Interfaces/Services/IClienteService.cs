using NFinance.Model.ClientesViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IClienteService
    {
        Task<ListarClientesViewModel.Response> ListarClientes();
        Task<ConsultarClienteViewModel.Response> ConsultarCliente(Guid id);
        Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request clienteRequest);
        Task<AtualizarClienteViewModel.Response> AtualizarCliente(Guid id, AtualizarClienteViewModel.Request clienteRequest);
    }
}
