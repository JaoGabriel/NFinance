using Xunit;
using System;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.ClientesViewModel;
using NFinance.Domain.Identidade;

namespace NFinance.Tests.WebApi
{
    public class ClienteControllerTests
    {
        private readonly Mock<IClienteApp> _clienteApp;
        private readonly Mock<ILogger<ClienteController>> _logger;

        public ClienteControllerTests()
        {
            _clienteApp = new Mock<IClienteApp>();
            _logger = new Mock<ILogger<ClienteController>>();
        }

        private ClienteController InicializarClienteController()
        {
            return new(_logger.Object, _clienteApp.Object);
        }

        private static Usuario GeraUsuario()
        {
            return new() { Id = Guid.NewGuid(), Email = "teste@teste.com", PasswordHash = "123456" };
        }

        public static Cliente GeraCliente()
        {
            return new("Jorgin da Lages", "12345678910", "aloha@teste.com","41986537456");
        }

        [Fact]
        public void ClienteController_CadastrarCliente_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteApp.Setup(x => x.CadastrarCliente(It.IsAny<CadastrarClienteViewModel.Request>())).ReturnsAsync(new CadastrarClienteViewModel.Response(cliente));
            var controller = InicializarClienteController();
            var clienteRequest = new CadastrarClienteViewModel.Request(cliente);

            //Act
            var teste = controller.CadastrarCliente(clienteRequest);
            var okResult = teste.Result as ObjectResult;
            var cadastrarClienteViewModel = Assert.IsType<CadastrarClienteViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(clienteRequest.Id, cadastrarClienteViewModel.Id);
            Assert.Equal(clienteRequest.Nome, cadastrarClienteViewModel.Nome);
            Assert.Equal(clienteRequest.Cpf, cadastrarClienteViewModel.Cpf);
            Assert.Equal(clienteRequest.Email, cadastrarClienteViewModel.Email);
        }

        [Fact]
        public void ClienteController_ConsultarCliente_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteApp.Setup(x => x.ConsultaCliente(It.IsAny<Guid>())).ReturnsAsync(new ConsultarClienteViewModel.Response(cliente));
            var controller = InicializarClienteController();

            //Act
            var teste = controller.ConsultarCliente("DHUAUHDASUIHDIAUHDSIUHASDHUI",cliente.Id);
            var okResult = teste.Result as ObjectResult;
            var consultarClienteViewModel = Assert.IsType<ConsultarClienteViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(cliente.Id, consultarClienteViewModel.Id);
            Assert.Equal(cliente.Nome, consultarClienteViewModel.Nome);
            Assert.Equal(cliente.Cpf.ToString(), consultarClienteViewModel.Cpf);
            Assert.Equal(cliente.Email.ToString(), consultarClienteViewModel.Email);
        }

        [Fact]
        public void ClienteController_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteApp.Setup(x => x.AtualizarDadosCadastrais(It.IsAny<Guid>(),It.IsAny<AtualizarClienteViewModel.Request>())).ReturnsAsync(new AtualizarClienteViewModel.Response(cliente));
            var clienteRequest = new AtualizarClienteViewModel.Request(cliente);
            var controller = InicializarClienteController();

            //Act
            var teste = controller.AtualizarCliente("21U3LKJSDALIJ12OI3KJHASDHI12342EQLJEUH127",cliente.Id, clienteRequest);
            var okResult = teste.Result as ObjectResult;
            var AtualizarClienteViewModel = Assert.IsType<AtualizarClienteViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(cliente.Id, AtualizarClienteViewModel.Id);
            Assert.Equal(cliente.Nome, AtualizarClienteViewModel.Nome);
            Assert.Equal(cliente.Cpf.ToString(), AtualizarClienteViewModel.Cpf);
            Assert.Equal(cliente.Email.ToString(), AtualizarClienteViewModel.Email);
        }
    }
}
