using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using NFinance.Domain.ViewModel.ClientesViewModel;
using Xunit;
using NFinance.Domain.Services;
using NFinance.Domain;

namespace NFinance.Tests.WebApi
{
    public class ClienteControllerTests
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClienteController> _logger;
        private readonly IAutenticacaoService _autenticacaoService;

        public ClienteControllerTests()
        {
            _clienteService = Substitute.For<IClienteService>();
            _autenticacaoService = Substitute.For<IAutenticacaoService>();
            _logger = Substitute.For<ILogger<ClienteController>>();
        }

        private ClienteController InicializarClienteController()
        {
            return new ClienteController(_logger, _clienteService,_autenticacaoService);
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
            var cliente = new CadastrarClienteViewModel.Request
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
            var token = TokenService.GerarToken(new Cliente { Id = id, CPF = "12345678910", Email = "teste@teste.com", Nome = "teste da silva" });

            //Act
            var teste = controller.ConsultarCliente(id,token);
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
        public void ClienteController_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jorgin da Lages";
            var cpf = "123.123.123-11";
            var email = "aloha@teste.com";
            _clienteService.AtualizarCliente(Arg.Any<Guid>(),Arg.Any<AtualizarClienteViewModel.Request>())
                .Returns(new AtualizarClienteViewModel.Response
                {
                    Id = id,
                    Nome = nome,
                    Cpf = cpf,
                    Email = email
                });
            var cliente = new AtualizarClienteViewModel.Request
            {
                Nome = nome,
                Cpf = cpf,
                Email = email
            };
            var controller = InicializarClienteController();
            var token = TokenService.GerarToken(new Cliente { Id = id, CPF = "12345678910", Email = "teste@teste.com", Nome = "teste da silva" });

            //Act
            var teste = controller.AtualizarCliente(token,id, cliente);
            var okResult = teste.Result as ObjectResult;
            var AtualizarClienteViewModel = Assert.IsType<AtualizarClienteViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, AtualizarClienteViewModel.Id);
            Assert.Equal(nome, AtualizarClienteViewModel.Nome);
            Assert.Equal(cpf, AtualizarClienteViewModel.Cpf);
            Assert.Equal(email, AtualizarClienteViewModel.Email);
        }


    }
}
