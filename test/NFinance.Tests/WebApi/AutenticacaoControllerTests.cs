using System;
using Xunit;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.AutenticacaoViewModel;
using NFinance.Domain.Identidade;

namespace NFinance.Tests.WebApi
{
    public class AutenticacaoControllerTests
    {
        private readonly Mock<IAutenticacaoApp> _autenticacaoApp;
        private readonly Mock<ILogger<AutenticacaoController>> _logger;

        public AutenticacaoControllerTests()
        {
            _logger = new Mock<ILogger<AutenticacaoController>>();
            _autenticacaoApp = new Mock<IAutenticacaoApp>();
        }

        private AutenticacaoController InicializarAutenticacaoController()
        {
            return new(_logger.Object, _autenticacaoApp.Object);
        }

        private static Usuario GeraUsuario()
        {
            return new() { Id = Guid.NewGuid(), Email = "teste@teste.com", PasswordHash = "123456" };
        }

        public static Cliente GeraCliente()
        {
            return new("Jorgin da Lages", "12345678910", "aloha@teste.com", "41986531547");
        }

        [Fact]
        public void LoginController_EfetuarLogin_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var usuario = GeraUsuario();
            var loginViewModel = new LoginViewModel { Email = cliente.Email.ToString(), Senha = "123456"};
            var controller = InicializarAutenticacaoController();
            _autenticacaoApp.Setup(x => x. EfetuarLogin(It.IsAny<LoginViewModel>())).ReturnsAsync(new LoginViewModel.Response(usuario,"TESTEASDARASDADADRARASDASDASDASDASDASDA"));

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
            var logoutVm = new LogoutViewModel(cliente.Id);
            var controller = InicializarAutenticacaoController();
            _autenticacaoApp.Setup(x => x. EfetuarLogoff(It.IsAny<LogoutViewModel>())).ReturnsAsync(new LogoutViewModel.Response("Realizado com sucesso",true));

            //Act
            var teste = controller.Deslogar("TERASFDADASDADADADASEDASDASD",logoutVm);
            var okResult = teste.Result as ObjectResult;
            var autenticarViewModel = Assert.IsType<LogoutViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotNull(autenticarViewModel.Message);
        }
    }
}
