using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.WebApi
{
    public class LoginControllerTests
    {
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<AutenticacaoController> _logger;

        public LoginControllerTests()
        {
            _logger = Substitute.For<ILogger<AutenticacaoController>>();
            _autenticacaoService = Substitute.For<IAutenticacaoService>();
        }

        private AutenticacaoController InicializarLoginController()
        {
            return new AutenticacaoController(_logger, _autenticacaoService);
        }

        [Fact]
        public async Task LoginController_EfetuarLogin_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idSessao = Guid.NewGuid();
            var nome = "Jorgin da Lages";
            var email = "aloha@teste.com";
            var senha = "123456";
            var token = "dgyagdyaygsdyaguadarqiuwhasdaweuhqiuahsdjkahsduahsdl";
            var response = new LoginViewModel.Response { IdSessao = idSessao, IdCliente = id, Nome = nome, Token = token, Autenticado = true, Erro = null };
            var loginViewModel = new LoginViewModel { Email = email, Senha = senha};
            var controller = InicializarLoginController();
            _autenticacaoService.RealizarLogin(Arg.Any<LoginViewModel>()).Returns(response);

            //Act
            var teste = controller.Autenticar(loginViewModel);
            var okResult = teste.Result as ObjectResult;
            var autenticarViewModel = Assert.IsType<LoginViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotEqual(Guid.Empty, autenticarViewModel.IdSessao);
            Assert.Equal(nome, autenticarViewModel.Nome);
            Assert.NotNull(autenticarViewModel.Token);
        }
    }
}
