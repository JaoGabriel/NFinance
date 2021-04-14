using System;
using System.Threading.Tasks;
using NFinance.Domain.ViewModel.ClientesViewModel;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IClienteService
    {
        Task<ConsultarClienteViewModel.Response> ConsultarCliente(Guid id);
        Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request clienteRequest);
        Task<AtualizarClienteViewModel.Response> AtualizarCliente(Guid id, AtualizarClienteViewModel.Request clienteRequest);
        Task<LoginViewModel.Response> ConsultarCredenciaisLogin(LoginViewModel request);
    }
}
