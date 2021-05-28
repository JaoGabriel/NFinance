using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Application.ViewModel.AutenticacaoViewModel;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.WebApi
{
    public class AutenticacaoControllerTests
    {
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<AutenticacaoController> _logger;

        public AutenticacaoControllerTests()
        {
            _logger = Substitute.For<ILogger<AutenticacaoController>>();
            _autenticacaoService = Substitute.For<IAutenticacaoService>();
        }

        private AutenticacaoController InicializarAutenticacaoController()
        {
            return new AutenticacaoController(_logger, _autenticacaoService);
        }

        public Cliente GeraCliente()
        {
            return new Cliente("Jorgin da Lages", "12345678910", "aloha@teste.com", "123456");
        }

        [Fact]
        public async Task LoginController_EfetuarLogin_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var loginViewModel = new LoginViewModel { Email = cliente.Email, Senha = cliente.Senha};
            var controller = InicializarAutenticacaoController();
            _autenticacaoService.RealizarLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);

            //Act
            var teste = controller.Autenticar(loginViewModel);
            var okResult = teste.Result as ObjectResult;
            var autenticarViewModel = Assert.IsType<LoginViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(cliente.Nome, autenticarViewModel.Nome);
            Assert.NotNull(autenticarViewModel.Token);
        }

        [Fact]
        public async Task LoginController_EfetuarLogout_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var token = TokenService.GerarToken(cliente);
            var controller = InicializarAutenticacaoController();
            _autenticacaoService.RealizarLogut(Arg.Any<Guid>()).Returns(cliente);

            //Act
            var teste = controller.Deslogar(token, cliente.Id);
            var okResult = teste.Result as ObjectResult;
            var autenticarViewModel = Assert.IsType<LogoutViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Logot Realizado Com Sucesso!", autenticarViewModel.Message);
            Assert.True(autenticarViewModel.Deslogado);
        }
    }
}
