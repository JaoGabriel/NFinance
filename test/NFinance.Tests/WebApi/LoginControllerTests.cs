using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.ViewModel.LoginViewModel;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.WebApi
{
    public class LoginControllerTests
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogger<LoginController> _logger;

        public LoginControllerTests()
        {
            _logger = Substitute.For<ILogger<LoginController>>();
            _clienteRepository = Substitute.For<IClienteRepository>();
        }

        private LoginController InicializarLoginController()
        {
            return new LoginController(_logger, _clienteRepository);
        }

        [Fact]
        public async Task LoginController_EfetuarLogin_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jorgin da Lages";
            var cpf = "123.123.123-11";
            var email = "aloha@teste.com";
            var senha = "123456";
            var cliente = new Cliente { Id = id, Nome = nome, CPF = cpf, Email = email, Senha = senha};
            var loginViewModel = new LoginViewModel { Email = email, Senha = senha};
            var controller = InicializarLoginController();
            _clienteRepository.CredenciaisLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(cliente);

            //Act
            var teste = await controller.Autenticar(loginViewModel);
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
