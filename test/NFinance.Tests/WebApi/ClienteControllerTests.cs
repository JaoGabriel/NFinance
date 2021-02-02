using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Collections.Generic;
using NFinance.Domain.ViewModel.ClientesViewModel;
using Xunit;

namespace NFinance.Tests.WebApi
{
    public class ClienteControllerTests
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClienteController> _logger;
        public ClienteControllerTests()
        {
            _clienteService = Substitute.For<IClienteService>();
            _logger = Substitute.For<ILogger<ClienteController>>();
        }

        private ClienteController InicializarClienteController()
        {
            return new ClienteController(_logger, _clienteService);
        }

        [Fact]
        public void ClienteController_CadastrarCliente_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jorgin da Lages";
            var cpf = "123.123.123-11";
            var email = "aloha@teste.com";
            _clienteService.CadastrarCliente(Arg.Any<CadastrarClienteViewModel.Request>())
                .Returns(new CadastrarClienteViewModel.Response
                {
                    Id = id,
                    Nome = nome,
                    Cpf = cpf,
                    Email = email
                });
            var controller = InicializarClienteController();
            var cliente = new CadastrarClienteViewModel.Request()
            {
                Nome = nome,
                Cpf = cpf,
                Email = email
            };

            //Act
            var teste = controller.CadastrarCliente(cliente);
            var okResult = teste.Result as ObjectResult;
            var cadastrarClienteViewModel = Assert.IsType<CadastrarClienteViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, cadastrarClienteViewModel.Id);
            Assert.Equal(nome, cadastrarClienteViewModel.Nome);
            Assert.Equal(cpf, cadastrarClienteViewModel.Cpf);
            Assert.Equal(email, cadastrarClienteViewModel.Email);
        }

        [Fact]
        public void ClienteController_ConsultarCliente_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jorgin da Lages";
            var cpf = "123.123.123-11";
            var email = "aloha@teste.com";
            _clienteService.ConsultarCliente(Arg.Any<Guid>())
                .Returns(new ConsultarClienteViewModel.Response
                {
                    Id = id,
                    Nome = nome,
                    Cpf = cpf,
                    Email = email
                });
            var controller = InicializarClienteController();

            //Act
            var teste = controller.ConsultarCliente(id);
            var okResult = teste.Result as ObjectResult;
            var consultarClienteViewModel = Assert.IsType<ConsultarClienteViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, consultarClienteViewModel.Id);
            Assert.Equal(nome, consultarClienteViewModel.Nome);
            Assert.Equal(cpf, consultarClienteViewModel.Cpf);
            Assert.Equal(email, consultarClienteViewModel.Email);
        }

        [Fact]
        public void ClienteController_ListarClientes_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jorgin da Lages";
            var cpf = "123.123.123-11";
            var email = "aloha@teste.com";
            var listaClientes = new List<Cliente>();
            var cliente = new Cliente() { Id = id, Nome = nome, CPF = cpf, Email = email};
            listaClientes.Add(cliente);
            var listarClientes = new ListarClientesViewModel.Response(listaClientes);
            _clienteService.ListarClientes().Returns(listarClientes);
            var controller = InicializarClienteController();           

            //Act
            var teste = controller.ListarClientes();
            var okResult = teste.Result as ObjectResult;
            var listarClientesViewModel = Assert.IsType<ListarClientesViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, listarClientesViewModel.Find(c => c.Id == id).Id);
            Assert.Equal(nome, listarClientesViewModel.Find(c => c.Id == id).Nome);
            Assert.Equal(cpf, listarClientesViewModel.Find(c => c.Id == id).Cpf);
            Assert.Equal(email, listarClientesViewModel.Find(c => c.Id == id).Email);
        }
    }
}
