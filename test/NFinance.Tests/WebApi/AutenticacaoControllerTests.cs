using Xunit;
using NSubstitute;
using NFinance.Domain;
using NFinance.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.AutenticacaoViewModel;

namespace NFinance.Tests.WebApi
{
    public class AutenticacaoControllerTests
    {
        private readonly IAutenticacaoApp _autenticacaoApp;
        private readonly ILogger<AutenticacaoController> _logger;

        public AutenticacaoControllerTests()
        {
            _logger = Substitute.For<ILogger<AutenticacaoController>>();
            _autenticacaoApp = Substitute.For<IAutenticacaoApp>();
        }

        private AutenticacaoController InicializarAutenticacaoController()
        {
            return new AutenticacaoController(_logger, _autenticacaoApp);
        }

        public static Cliente GeraCliente()
        {
            return new Cliente("Jorgin da Lages", "12345678910", "aloha@teste.com", "123456");
        }

        [Fact]
        public void LoginController_EfetuarLogin_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var loginViewModel = new LoginViewModel { Email = cliente.Email, Senha = cliente.Senha};
            var controller = InicializarAutenticacaoController();
            _autenticacaoApp.EfetuarLogin(Arg.Any<LoginViewModel>()).Returns(new LoginViewModel.Response(cliente,"aaaaaaaaaaaaaa"));

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
        public void LoginController_EfetuarLogout_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var token = TokenService.GerarToken(cliente);
            var logoutVm = new LogoutViewModel(cliente.Id);
            var controller = InicializarAutenticacaoController();
            _autenticacaoApp.EfetuarLogoff(Arg.Any<LogoutViewModel>()).Returns(new LogoutViewModel.Response("Realizado com sucesso",true));

            //Act
            var teste = controller.Deslogar(token,logoutVm);
            var okResult = teste.Result as ObjectResult;
            var autenticarViewModel = Assert.IsType<LogoutViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Realizado com sucesso", autenticarViewModel.Message);
            Assert.True(autenticarViewModel.Deslogado);
        }
    }
}
