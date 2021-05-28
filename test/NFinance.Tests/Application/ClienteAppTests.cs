using NSubstitute;
using NFinance.Application;
using NFinance.Domain.Interfaces.Services;
using System.Threading.Tasks;
using Xunit;
using NFinance.Domain;
using NFinance.Application.ViewModel.ClientesViewModel;

namespace NFinance.Tests.Application
{
    public class ClienteAppTests
    {
        private readonly IClienteService _clienteService;

        public ClienteAppTests()
        {
            _clienteService = Substitute.For<IClienteService>();
        }

        public ClienteApp IniciaApplication()
        {
            return new ClienteApp(_clienteService);
        }

        public Cliente GeraCliente()
        {
            var nome = "Jorge Santos";
            var cpf = "123.456.789-10";
            var email = "jorgin@teste.com";
            var senha = "sduhasdasuidaiusdhaoisdhiu";
            return new Cliente(nome, cpf, email, senha);
        }

        [Fact]
        public async Task ClienteApp_CadastroCliente_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteService.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            var response = await app.CadastrarCliente(cadastroClienteVM);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<CadastrarClienteViewModel.Response>(response);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.CPF, response.Cpf);
            Assert.Equal(cliente.Email, response.Email);
            Assert.Equal(cliente.Senha, response.Senha);
        }

        [Fact]
        public async Task ClienteApp_AtualizarCliente_ComNome_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Nome = " ";
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteService.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            var response = await app.CadastrarCliente(cadastroClienteVM);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<CadastrarClienteViewModel.Response>(response);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.CPF, response.Cpf);
            Assert.Equal(cliente.Email, response.Email);
            Assert.Equal(cliente.Senha, response.Senha);
        }
    }
}
