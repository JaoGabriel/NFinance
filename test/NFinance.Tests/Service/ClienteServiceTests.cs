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

        public ClienteServiceTests()
        {
            _clienteRepository = Substitute.For<IClienteRepository>();
        }

        public ClienteService InicializaServico()
        {
            return new ClienteService(_clienteRepository);
        }

        public Cliente GeraCliente()
        {
            var nome = "Teste@Sucesso";
            var cpf = "123.654.987-96";
            var email = "teste@teste.com";
            var senha = "dahusdhuasuh";
            var senhaCriptografada = HashValue(senha);
            return new Cliente(nome, cpf, email, senhaCriptografada);
        }

        public Cliente GeraClienteComLogoutToken()
        {
            var nome = "Teste@Sucesso";
            var cpf = "123.654.987-96";
            var email = "teste@teste.com";
            var senha = "dahusdhuasuh";
            var senhaCriptografada = HashValue(senha);
            var cliente = new Cliente(nome, cpf, email, senhaCriptografada);
            var logoutToken = TokenService.GerarToken(cliente);
            cliente.LogoutToken = logoutToken;
            return cliente;
        }

        static string HashValue(string value)
        {
            var encoding = new UnicodeEncoding();
            byte[] hashBytes;
            using (HashAlgorithm hash = SHA256.Create())
                hashBytes = hash.ComputeHash(encoding.GetBytes(value));

            var hashValue = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
            }

            return hashValue.ToString();
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
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

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNome_Nulo()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Nome = null;
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNome_Vazio()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Nome = "";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNome_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Nome = " ";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComCPF_Vazio()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.CPF = "";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComCPF_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.CPF = " ";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComCPF_Nulo()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.CPF = null;
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComEmail_Vazio()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Email = "";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComEmail_Nulo()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Email = null;
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComEmail_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Email = " ";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComSenha_Nula()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Senha = null;
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<SenhaClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComSenha_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Senha = " ";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<SenhaClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComSenha_Vazia()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Senha = "";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<SenhaClienteException>(() => /*Act*/ services.CadastrarCliente(cliente));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Nome = " ";
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_Nulo()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Nome = null;
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_Vazio()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Nome = "";
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComId_Vazio()
        {
            //Arrange
            var id = Guid.Empty;
            var cliente = GeraCliente();
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarCliente(id, cliente));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComCPF_Vazio()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.CPF = "";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComCPF_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.CPF = " ";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComCPF_Nulo()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.CPF = null;
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComEmail_Vazio()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Email = "";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComEmail_Nulo()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Email = null;
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComEmail_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            cliente.Email = " ";
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.AtualizarCliente(cliente.Id, cliente));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(),Arg.Any<Cliente>()).Returns(cliente);
            var services = InicializaServico();
            
            //Act
            var response = await services.AtualizarCliente(cliente.Id, cliente);

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
            var cliente = GeraCliente();
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
            var cliente = GeraCliente();
            cliente.Id = Guid.Empty;
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            var services = InicializaServico();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarCliente(cliente.Id));
        }

        [Fact]
        public async Task ClienteService_CadastrarLogoutToken_ComSucesso()
        {
            //Arrange
            var cliente = GeraClienteComLogoutToken();
            var token = cliente.LogoutToken;
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(),Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            var response = await services.CadastrarLogoutToken(cliente,token);

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

        [Fact]
        public async Task ClienteService_CadastrarLogoutToken_ComToken_Nulo()
        {
            //Arrange
            var cliente = GeraClienteComLogoutToken();
            cliente.LogoutToken = null;
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LogoutTokenException>(() => /*Act*/ services.CadastrarLogoutToken(cliente, null));
        }

        [Fact]
        public async Task ClienteService_CadastrarLogoutToken_ComToken_Vazio()
        {
            //Arrange
            var cliente = GeraClienteComLogoutToken();
            cliente.LogoutToken = "";
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LogoutTokenException>(() => /*Act*/ services.CadastrarLogoutToken(cliente, ""));
        }

        [Fact]
        public async Task ClienteService_CadastrarLogoutToken_ComToken_EmBranco()
        {
            //Arrange
            var cliente = GeraClienteComLogoutToken();
            cliente.LogoutToken = " ";
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LogoutTokenException>(() => /*Act*/ services.CadastrarLogoutToken(cliente, " "));
        }

        [Fact]
        public async Task ClienteService_CadastrarLogoutToken_ComCliente_Nulo()
        {
            //Arrange
            Cliente cliente = null;
            _clienteRepository.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LogoutTokenException>(() => /*Act*/ services.CadastrarLogoutToken(cliente, "auhdahuduhaisdhuiadhiuasiduhssaiuh"));
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
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

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComEmail_Nulo()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(null ,cliente.Senha));
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComEmail_Vazio()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin("", cliente.Senha));
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComEmail_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(" ", cliente.Senha));
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComSenha_EmBranco()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(cliente.Email, " "));
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComSenha_Vazio()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(cliente.Email, ""));
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComSenha_Nula()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(cliente.Email, null));
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComEmail_Invalido()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            //await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(cliente.Email, null));
            Assert.False(true);
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComSenha_Invalida()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            //await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(cliente.Email, null));
            Assert.False(true);
        }

        [Fact]
        public async Task ClienteService_ConsultarCredenciaisLogin_ComEmaileSenha_Invalida()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteRepository.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            var services = InicializaServico();

            //Assert
            //await Assert.ThrowsAsync<LoginException>(() => /*Act*/ services.ConsultarCredenciaisLogin(cliente.Email, null));
            Assert.False(true);
        }
    }
}
