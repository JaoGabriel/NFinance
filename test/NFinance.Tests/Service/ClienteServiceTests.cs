using Xunit;
using System;
using System.Text;
using NSubstitute;
using NFinance.Domain;
using System.Globalization;
using System.Threading.Tasks;
using NFinance.Domain.Services;
using NFinance.Domain.Exceptions;
using System.Security.Cryptography;
using NFinance.Domain.Exceptions.Cliente;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Exceptions.Autenticacao;

namespace NFinance.Tests.Service
{
    public class ClienteServiceTests
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly string _cpf = "12345678910";
        private readonly string _email = "teste@teste.com";
        private readonly string _nome = "joaquin da zils";
        private readonly string _senha = "12391ukla";
        private readonly string _logoutToken = "asuhdhausduhasdiuhasuih";

        public ClienteServiceTests()
        {
            _clienteRepository = Substitute.For<IClienteRepository>();
        }

        private ClienteService InicializaServico()
        {
            return new(_clienteRepository);
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email,_senha);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();

            //Act
            var response = await services.CadastrarCliente(cliente);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotNull(response.CPF);
            Assert.NotNull(response.Email);
            Assert.NotNull(response.Senha);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(cliente.Id, response.Id);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.CPF, response.CPF);
            Assert.Equal(cliente.Email, response.Email);
            Assert.Equal(cliente.Senha, response.Senha);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_CadastrarCliente_ComNome_Invalido(string nome)
        {
            //Arrange
            var cliente = new Cliente(nome,_cpf,_email,_senha);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_CadastrarCliente_ComCPF_Invalido(string cpf)
        {
            //Arrange
            var cliente = new Cliente(_nome,cpf,_email,_senha);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_CadastrarCliente_ComEmail_Invalido(string email)
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,email,_senha);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_CadastrarCliente_ComSenha_Invalida(string senha)
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email,senha);
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<SenhaClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_AtualizarCliente_ComNome_Invalido(string nome)
        {
            //Arrange
            var cliente = new Cliente(nome,_cpf,_email,_senha);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComId_Vazio()
        {
            //Arrange
            var cliente = new Cliente(Guid.Empty,_nome,_cpf,_email,_senha);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarCliente(cliente));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_AtualizarCliente_ComCPF_Invalido(string cpf)
        {
            //Arrange
            var cliente = new Cliente(_nome,cpf,_email,_senha);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.AtualizarCliente(cliente));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_AtualizarCliente_ComEmail_Invalido(string email)
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,email,_senha);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.AtualizarCliente(cliente));
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_AtualizarCliente_ComSenha_Invalida(string senha)
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email,senha);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<SenhaClienteException>(() => /*Act*/ services.AtualizarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(),_nome,_cpf,_email,_senha);
            _clienteRepository.AtualizarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            
            //Act
            var response = await services.AtualizarCliente(cliente);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotNull(response.CPF);
            Assert.NotNull(response.Email);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(cliente.Id, response.Id);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.CPF, response.CPF);
            Assert.Equal(cliente.Email, response.Email);
        }

        [Fact]
        public async Task ClienteService_ConsultarCliente_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email,_senha);
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarCliente(cliente.Id);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotNull(response.CPF);
            Assert.NotNull(response.Email);
            Assert.NotNull(response.Senha);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(cliente.Id, response.Id);
            Assert.Equal(cliente.CPF, response.CPF);
            Assert.Equal(cliente.Email, response.Email);
            Assert.Equal(cliente.Senha, response.Senha);
        }

        [Fact]
        public async Task ClienteService_ConsultarCliente_ComId_Vazio()
        {
            //Arrange
            var cliente = new Cliente(Guid.Empty,_nome,_cpf,_email,_senha);
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            var services = InicializaServico();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarCliente(cliente.Id));
        }

        [Fact]
        public async Task ClienteService_CadastrarLogoutToken_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(),_nome,_cpf,_email,_senha,_logoutToken);
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(),Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            var response = await services.CadastrarLogoutToken(cliente,cliente.LogoutToken);

            Assert.NotNull(response.Nome);
            Assert.NotNull(response.CPF);
            Assert.NotNull(response.Email);
            Assert.NotNull(response.LogoutToken);
            Assert.NotNull(response.Senha);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(cliente.Id, response.Id);
            Assert.Equal(cliente.CPF, response.CPF);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.LogoutToken, response.LogoutToken);
            Assert.Equal(cliente.Senha, response.Senha);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_CadastrarLogoutToken_ComToken_Invalido(string token)
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(),_nome,_cpf,_email,_senha,token);
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LogoutTokenException>(() => /*Act*/ services.CadastrarLogoutToken(cliente, token));
        }

        [Fact]
        public async Task ClienteService_CadastrarLogoutToken_ComCliente_Nulo()
        {
            //Arrange
            Cliente cliente = null;
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LogoutTokenException>(() => /*Act*/ services.CadastrarLogoutToken(cliente, _logoutToken));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarLogoutToken_ComDadosRequestValidos_RetornoCadastro_RetornaNulo()
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(),_nome,_cpf,_email,_senha,_logoutToken);
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns((Cliente)null);
            var services = InicializaServico();
            
            //Act
            var resposta = await services.CadastrarLogoutToken(cliente, "auhdahuduhaisdhuiadhiuasiduhssaiuh");
            
            //Assert
            Assert.Null(resposta);
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(),_nome,_cpf,_email,_senha,_logoutToken);
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            var response = await services.ConsultarCredenciaisLogin(cliente.Email, cliente.Senha);

            Assert.NotNull(response.Nome);
            Assert.NotNull(response.CPF);
            Assert.NotNull(response.Email);
            Assert.NotNull(response.Senha);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(cliente.Id, response.Id);
            Assert.Equal(cliente.CPF, response.CPF);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.Senha, response.Senha);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComEmail_Invalido(string email)
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(),_nome,_cpf,email,_senha,_logoutToken);
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(email ,cliente.Senha));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComSenha_Invalida(string senha)
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(),_nome,_cpf,_email,senha,_logoutToken);
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(cliente.Email, senha));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComEmaileSenha_Invalida(string credenciais)
        {
            //Arrange
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns((Cliente)null);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(credenciais, credenciais));
        }
        
        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComDadosRequestValidos_RetornoConsulta_RetornaNulo()
        {
            //Arrange
            var cliente = new Cliente(Guid.NewGuid(),_nome,_cpf,_email,_senha,_logoutToken);
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns((Cliente)null);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(cliente.Email, cliente.Senha));
        }
    }
}
