using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Autenticacao;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NSubstitute;
using Xunit;

namespace NFinance.Tests.Service
{
    public class AutenticacaoServiceTests
    {
        private readonly IRedisService _redis;
        private readonly IClienteService _clienteService;
        
        public AutenticacaoServiceTests()
        {
            _redis = Substitute.For<IRedisService>();
            _clienteService = Substitute.For<IClienteService>();
        }

        public AutenticacaoService InicializaServico()
        {
            return new(_clienteService,_redis);
        }

        public static Cliente GeraCliente()
        {
            var cliente = new Cliente(Guid.NewGuid(), "Nome","CPF","Email","Senha","TokenTEste");
            return cliente;
        }

        [Fact]
        public async Task AutenticacaoService_RealizarLogin_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var servico = InicializaServico();
            _redis.IncluiValorCache(Arg.Any<Cliente>()).Returns(true);
            _clienteService.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);
            
            //Act
            var resposta = await servico.RealizarLogin(cliente.Email,cliente.Senha);

            //Assert
            Assert.NotNull(resposta);
            Assert.Equal(cliente.Nome,resposta.Nome);
            Assert.Equal(cliente.Email,resposta.Email);
            Assert.Equal(cliente.Senha,resposta.Senha);
            Assert.Equal(cliente.CPF,resposta.CPF);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task AutenticacaoService_RealizarLogin_ComLoginInvalidoESenhaValida_RetornaException(string login)
        {
            //Arrange
            var cliente = GeraCliente();
            var servico = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => servico.RealizarLogin(login, cliente.Senha));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task AutenticacaoService_RealizarLogin_ComLoginValidoESenhaInvalida_RetornaException(string senha)
        {
            //Arrange
            var cliente = GeraCliente();
            var servico = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => servico.RealizarLogin(cliente.Email, senha));
        }
        
        [Fact]
        public async Task AutenticacaoService_RealizarLogin_ComLoginESenhaInvalidos_RetornaException()
        {
            //Arrange
            var cliente = GeraCliente();
            var servico = InicializaServico();
            _clienteService.ConsultarCredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns((Cliente)null);

            //Assert
            await Assert.ThrowsAsync<LoginException>(() => servico.RealizarLogin(cliente.Email, cliente.Senha));
        }
        
        [Fact]
        public async Task AutenticacaoService_RealizarLogout_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var servico = InicializaServico();
            _redis.RetornaValorPorChave(Arg.Any<string>()).Returns(cliente);
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            _clienteService.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns(cliente);
            _redis.RemoverValorCache(Arg.Any<string>()).Returns(true);
            
            //Act
            var resposta = await servico.RealizarLogut(cliente.Id);

            //Assert
            Assert.NotNull(resposta);
            Assert.Equal(cliente.Nome,resposta.Nome);
            Assert.Equal(cliente.Email,resposta.Email);
            Assert.Equal(cliente.Senha,resposta.Senha);
            Assert.Equal(cliente.CPF,resposta.CPF);
            Assert.Equal(cliente.LogoutToken,resposta.LogoutToken);
        }
        
        [Fact]
        public async Task AutenticacaoService_RealizarLogout_ComIdInvalido_RetornaException()
        {
            //Arrange
            var servico = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => servico.RealizarLogut(Guid.Empty));
        }
        
        [Fact]
        public async Task AutenticacaoService_RealizarLogout_ComErroExclusaoNoRedis_RetornaException()
        {
            //Arrange
            var cliente = GeraCliente();
            var servico = InicializaServico();
            _redis.RetornaValorPorChave(Arg.Any<string>()).Returns(cliente);
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            _clienteService.CadastrarLogoutToken(Arg.Any<Cliente>(), Arg.Any<string>()).Returns(cliente);
            _redis.RemoverValorCache(Arg.Any<string>()).Returns(false);

            //Assert
            await Assert.ThrowsAsync<LogoutException>(() => servico.RealizarLogut(cliente.Id));
        }
        
        [Fact]
        public async Task AutenticacaoService_ValidaTokenRequest_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var servico = InicializaServico();
            var listaToken = new List<string> {"Bearer udauisdauhsduashuiauhdiahiusdahuidhuiasduhiahuidsui"};
            _redis.RetornaValorPorChave(Arg.Any<string>()).Returns(cliente);
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            TokenService.LerToken(Arg.Any<string>()).Returns(listaToken);
            
            //Act
            var resposta = await servico.ValidaTokenRequest("Bearer udauisdauhsduashuiauhdiahiusdahuidhuiasduhiahuidsui");
        
            //Assert
            Assert.True(resposta);
        }
    }
}