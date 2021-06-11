using Xunit;
using System;
using NSubstitute;
using NFinance.Domain;
using NFinance.Application;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Cliente;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.ClientesViewModel;

namespace NFinance.Tests.Application
{
    public class ClienteAppTests
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly string _cpf = "12345678910";
        private readonly string _email = "teste@teste.com";
        private readonly string _nome = "joaquin da zils";
        private readonly string _senha = "12391ukla";

        public ClienteAppTests()
        {
            _clienteRepository = Substitute.For<IClienteRepository>();
        }

        public ClienteApp IniciaApplication()
        {
            return new ClienteApp(_clienteRepository);
        }

        [Fact]
        public async Task ClienteApp_CadastroCliente_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email,_senha);
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
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
        public async Task ClienteApp_CadastroCliente_ComNome_Invalido(string nome)
        {
            //Arrange
            var cliente = new Cliente(nome,_cpf,_email,_senha);
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<NomeClienteException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_CadastroCliente_ComCPF_Invalido(string cpf)
        {
            //Arrange
            var cliente = new Cliente(_nome,cpf,_email,_senha);
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<CpfClienteException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_CadastroCliente_ComEmail_Invalido(string email)
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,email,_senha);
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<EmailClienteException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_CadastroCliente_ComSenha_Invalida(string senha)
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email,senha);
            var cadastroClienteVM = new CadastrarClienteViewModel.Request(cliente);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<SenhaClienteException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Fact]
        public async Task ClienteApp_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email,_senha);
            var atualizarClienteVM = new AtualizarClienteViewModel.Request(cliente);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            var response = await app.AtualizarCliente(cliente.Id,atualizarClienteVM);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<AtualizarClienteViewModel.Response>(response);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.CPF, response.Cpf);
            Assert.Equal(cliente.Email, response.Email);
            Assert.Equal(cliente.Senha, response.Senha);
        }
        
        [Fact]
        public async Task ClienteApp_AtualizarCliente_ComIdCliente_Invalido()
        {
            //Arrange
            var cliente = new Cliente(Guid.Empty, _nome,_cpf,_email,_senha);
            var atualizarClienteVM = new AtualizarClienteViewModel.Request(cliente);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => app.AtualizarCliente(cliente.Id, atualizarClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComNome_Invalido(string nome)
        {
            //Arrange
            var cliente = new Cliente(nome,_cpf,_email,_senha);
            var atualizarClienteVM = new AtualizarClienteViewModel.Request(cliente);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<NomeClienteException>(() => app.AtualizarCliente(cliente.Id, atualizarClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComCPF_Invalido(string cpf)
        {
            //Arrange
            var cliente = new Cliente(_nome,cpf,_email,_senha);
            var atualizarClienteVM = new AtualizarClienteViewModel.Request(cliente);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<CpfClienteException>(() => app.AtualizarCliente(cliente.Id, atualizarClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComEmail_Invalido(string email)
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,email,_senha);
            var atualizarClienteVM = new AtualizarClienteViewModel.Request(cliente);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<EmailClienteException>(() => app.AtualizarCliente(cliente.Id, atualizarClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComSenha_Invalida(string senha)
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email,senha);
            var atualizarClienteVM = new AtualizarClienteViewModel.Request(cliente);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<SenhaClienteException>(() => app.AtualizarCliente(cliente.Id, atualizarClienteVM));
        }

        [Fact]
        public async Task ClienteApp_ConsultaCliente_ComSucesso()
        {
            //Arrage
            var cliente = new Cliente(_nome,_cpf,_email,_senha);
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            var app = IniciaApplication();
            
            //Act
            var response = await app.ConsultaCliente(cliente.Id);
            
            //Assert
            Assert.NotNull(response);
            Assert.IsType<ConsultarClienteViewModel.Response>(response);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.CPF, response.Cpf);
            Assert.Equal(cliente.Email, response.Email);
            Assert.Equal(cliente.Senha, response.Senha);
        }

        [Fact]
        public async Task ClienteApp_ConsultaCliente_ComId_Invalido()
        {
            //Arrage
            var cliente = new Cliente(Guid.Empty,_nome,_cpf,_email,_senha);
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => app.ConsultaCliente(cliente.Id));
        }
    }
}
