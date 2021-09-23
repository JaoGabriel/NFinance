using System;
using Xunit;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.AutenticacaoViewModel;
using NFinance.Application;
using NFinance.Domain.Identidade;

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
            return new(_logger, _autenticacaoApp);
        }

        private static Usuario GeraUsuario()
        {
            return new() { Id = Guid.NewGuid(), Email = "teste@teste.com", PasswordHash = "123456" };
        }

        public static Cliente GeraCliente()
        {
            return new("Jorgin da Lages", "12345678910", "aloha@teste.com", GeraUsuario());
        }

        [Fact]
        public void LoginController_EfetuarLogin_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var usuario = GeraUsuario();
            var token = TokenApp.GerarToken(usuario);
            var loginViewModel = new LoginViewModel { Email = cliente.Email, Senha = "123456"};
            var controller = InicializarAutenticacaoController();
            _autenticacaoApp.EfetuarLogin(Arg.Any<LoginViewModel>()).Returns(new LoginViewModel.Response(usuario,token));

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
            var token = TokenApp.GerarToken(GeraUsuario());
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
