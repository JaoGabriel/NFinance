using NSubstitute;
using NFinance.Application;
using NFinance.Domain.Interfaces.Services;
using System.Threading.Tasks;
using Xunit;
using NFinance.Domain;
using NFinance.Application.ViewModel.ClientesViewModel;
using NFinance.Domain.Exceptions.Cliente;

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

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComNome_Invalido(string nome)
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Nome = nome;
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteService.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<NomeClienteException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComCPF_Invalido(string cpf)
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.LogoutToken = cpf;
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteService.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<CpfClienteException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComEmail_Invalido(string email)
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Email = email;
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteService.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<EmailClienteException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComSenha_Invalida(string senha)
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Senha = senha;
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteService.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<NomeClienteException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
    }
}
