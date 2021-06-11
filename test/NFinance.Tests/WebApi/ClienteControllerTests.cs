using Xunit;
using System;
using NSubstitute;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.ClientesViewModel;
using NFinance.Application;

namespace NFinance.Tests.WebApi
{
    public class ClienteControllerTests
    {
        private readonly IClienteApp _clienteApp;
        private readonly ILogger<ClienteController> _logger;
        private readonly IAutenticacaoApp _autenticacaoApp;

        public ClienteControllerTests()
        {
            _clienteApp = Substitute.For<IClienteApp>();
            _autenticacaoApp = Substitute.For<IAutenticacaoApp>();
            _logger = Substitute.For<ILogger<ClienteController>>();
        }

        private ClienteController InicializarClienteController()
        {
            return new ClienteController(_logger, _clienteApp,_autenticacaoApp);
        }

        public Cliente GeraCliente()
        {
            return new Cliente("Jorgin da Lages", "12345678910", "aloha@teste.com", "123456");
        }

        [Fact]
        public void ClienteController_CadastrarCliente_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteApp.CadastrarCliente(Arg.Any<CadastrarClienteViewModel.Request>()).Returns(new CadastrarClienteViewModel.Response(cliente));
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
            _clienteApp.ConsultaCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response(cliente));
            var controller = InicializarClienteController();
            var token = TokenApp.GerarToken(cliente);

            //Act
            var teste = controller.ConsultarCliente(token,cliente.Id);
            var okResult = teste.Result as ObjectResult;
            var consultarClienteViewModel = Assert.IsType<ConsultarClienteViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(cliente.Id, consultarClienteViewModel.Id);
            Assert.Equal(cliente.Nome, consultarClienteViewModel.Nome);
            Assert.Equal(cliente.CPF, consultarClienteViewModel.Cpf);
            Assert.Equal(cliente.Email, consultarClienteViewModel.Email);
        }

        [Fact]
        public void ClienteController_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            _clienteApp.AtualizarCliente(Arg.Any<Guid>(),Arg.Any<AtualizarClienteViewModel.Request>())
                .Returns(new AtualizarClienteViewModel.Response(cliente));
            var clienteRequest = new AtualizarClienteViewModel.Request(cliente);
            var controller = InicializarClienteController();
            var token = TokenApp.GerarToken(cliente);

            //Act
            var teste = controller.AtualizarCliente(token,cliente.Id, clienteRequest);
            var okResult = teste.Result as ObjectResult;
            var AtualizarClienteViewModel = Assert.IsType<AtualizarClienteViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(cliente.Id, AtualizarClienteViewModel.Id);
            Assert.Equal(cliente.Nome, AtualizarClienteViewModel.Nome);
            Assert.Equal(cliente.CPF, AtualizarClienteViewModel.Cpf);
            Assert.Equal(cliente.Email, AtualizarClienteViewModel.Email);
        }
    }
}
