using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;
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

        [Fact]
        public async Task LoginController_EfetuarLogin_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jorgin da Lages";
            var email = "aloha@teste.com";
            var senha = "123456";
            var token = TokenService.GerarToken(new Cliente { Id = id, CPF = "12345678910", Email = "teste@teste.com", Nome = "teste da silva" });
            var response = new LoginViewModel.Response { IdCliente = id, Nome = nome, Token = token, Autenticado = true, Erro = null };
            var loginViewModel = new LoginViewModel { Email = email, Senha = senha};
            var controller = InicializarAutenticacaoController();
            _autenticacaoService.RealizarLogin(Arg.Any<LoginViewModel>()).Returns(response);

            //Act
            var teste = controller.Autenticar(loginViewModel);
            var okResult = teste.Result as ObjectResult;
            var autenticarViewModel = Assert.IsType<LoginViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(nome, autenticarViewModel.Nome);
            Assert.NotNull(autenticarViewModel.Token);
        }

        [Fact]
        public async Task LoginController_EfetuarLogout_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jorgin da Lages";
            var email = "aloha@teste.com";
            var cpf = "12345678910";
            var token = TokenService.GerarToken(new Cliente { Id = id, CPF = cpf, Email = email, Nome = nome });
            var response = new LogoutViewModel.Response { Deslogado = true, Message = "Logout com sucesso" };
            var logoutViewModel = new LogoutViewModel(id);
            var controller = InicializarAutenticacaoController();
            _autenticacaoService.RealizarLogut(Arg.Any<LogoutViewModel>()).Returns(response);

            //Act
            var teste = controller.Deslogar(token, logoutViewModel);
            var okResult = teste.Result as ObjectResult;
            var autenticarViewModel = Assert.IsType<LoginViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(nome, autenticarViewModel.Nome);
            Assert.NotNull(autenticarViewModel.Token);
        }
    }
}
