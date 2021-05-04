using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.Domain.ViewModel.GanhoViewModel;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace NFinance.Tests.WebApi
{
    public class GanhoControllerTests
    {
        private readonly IGanhoService _ganhoService;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<GanhoController> _logger;

        public GanhoControllerTests()
        {
            _ganhoService = Substitute.For<IGanhoService>();
            _autenticacaoService = Substitute.For<IAutenticacaoService>();
            _logger = Substitute.For<ILogger<GanhoController>>();
        }

        private GanhoController InicializarGanhoController()
        {
            return new GanhoController(_logger, _ganhoService, _autenticacaoService);
        }

        [Fact]
        public void GanhoController_CadastrarGanho_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGanho = "Testeee";
            var valor = 123712930.89M;
            _ganhoService.CadastrarGanho(Arg.Any<CadastrarGanhoViewModel.Request>())
                .Returns(new CadastrarGanhoViewModel.Response
                {
                    Id = id,
                    IdCliente = idCliente,
                    NomeGanho = nomeGanho,
                    Valor = valor,
                    Recorrente = false
                });
            var controller = InicializarGanhoController();
            var ganho = new CadastrarGanhoViewModel.Request
            {
                IdCliente = idCliente,
                NomeGanho = nomeGanho,
                Valor = valor,
                Recorrente = false
            };

            //Act
            var teste = controller.CadastrarGanho(ganho);
            var okResult = teste.Result as ObjectResult;
            var cadastrarGanhoViewModel = Assert.IsType<CadastrarGanhoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, cadastrarGanhoViewModel.Id);
            Assert.Equal(nomeGanho, cadastrarGanhoViewModel.NomeGanho);
            Assert.Equal(valor, cadastrarGanhoViewModel.Valor);
            Assert.Equal(idCliente, cadastrarGanhoViewModel.IdCliente);
            Assert.Equal(id, cadastrarGanhoViewModel.Id);
            Assert.False(cadastrarGanhoViewModel.Recorrente);
        }

        [Fact]
        public void GanhoController_AtualizarGanho_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGanho = "Testeee";
            var nomeNovo = "jhaasduhasd"; 
            var valor = 123712930.89M;
            _ganhoService.AtualizarGanho(Arg.Any<Guid>(),Arg.Any<AtualizarGanhoViewModel.Request>())
                .Returns(new AtualizarGanhoViewModel.Response
                {
                    Id = id,
                    IdCliente = idCliente,
                    NomeGanho = nomeGanho,
                    Valor = valor,
                    Recorrente = false
                });
            var controller = InicializarGanhoController();
            var ganho = new AtualizarGanhoViewModel.Request
            {
                IdCliente = idCliente,
                NomeGanho = nomeNovo,
                Valor = valor,
                Recorrente = true
            };
            var token = TokenService.GerarToken(new Cliente { Id = idCliente, CPF = "12345678910", Email = "teste@teste.com", Nome = "teste da silva" });

            //Act
            var teste = controller.AtualizarGanho(token,id,ganho);
            var okResult = teste.Result as ObjectResult;
            var atualizarGanhoViewModel = Assert.IsType<AtualizarGanhoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, atualizarGanhoViewModel.Id);
            Assert.Equal(nomeGanho, atualizarGanhoViewModel.NomeGanho);
            Assert.DoesNotMatch(nomeNovo, atualizarGanhoViewModel.NomeGanho);
            Assert.Equal(valor, atualizarGanhoViewModel.Valor);
            Assert.Equal(idCliente, atualizarGanhoViewModel.IdCliente);
            Assert.Equal(id, atualizarGanhoViewModel.Id);
            Assert.False(atualizarGanhoViewModel.Recorrente);
        }

        [Fact]
        public void GanhoController_ConsultarGanho_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGanho = "Testeee";
            var valor = 123712930.89M;
            _ganhoService.ConsultarGanho(Arg.Any<Guid>())
                .Returns(new ConsultarGanhoViewModel.Response
                {
                    Id = id,
                    IdCliente = idCliente,
                    NomeGanho = nomeGanho,
                    Valor = valor,
                    Recorrente = false
                });
            var controller = InicializarGanhoController();
            var token = TokenService.GerarToken(new Cliente { Id = idCliente, CPF = "12345678910", Email = "teste@teste.com", Nome = "teste da silva" });
            
            //Act
            var teste = controller.ConsultarGanho(token,id);
            var okResult = teste.Result as ObjectResult;
            var consultarGanhoViewModel = Assert.IsType<ConsultarGanhoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, consultarGanhoViewModel.Id);
            Assert.Equal(nomeGanho, consultarGanhoViewModel.NomeGanho);
            Assert.Equal(valor, consultarGanhoViewModel.Valor);
            Assert.Equal(idCliente, consultarGanhoViewModel.IdCliente);
            Assert.Equal(id, consultarGanhoViewModel.Id);
            Assert.False(consultarGanhoViewModel.Recorrente);
        }

        [Fact]
        public void GanhoController_ExcluirGanho_ComSucesso()
        {
            //Arrange
            var idGanho = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var motivo = "Perdio money";
            var mensagemSucesso = "Excluido com sucesso";
            _ganhoService.ExcluirGanho(Arg.Any<ExcluirGanhoViewModel.Request>())
                .Returns(new ExcluirGanhoViewModel.Response
                {
                    StatusCode = 200,
                    DataExclusao = DateTime.Today,
                    Mensagem = mensagemSucesso,
                });
            var controller = InicializarGanhoController();
            var ganho = new ExcluirGanhoViewModel.Request
            {
                IdCliente = idCliente,
                IdGanho = idGanho,
                MotivoExclusao = motivo
            };
            var token = TokenService.GerarToken(new Cliente { Id = idCliente, CPF = "12345678910", Email = "teste@teste.com", Nome = "teste da silva" });

            //Act
            var teste = controller.ExcluirGanho(token,ganho);
            var okResult = teste.Result as ObjectResult;
            var excluirGanhoViewModel = Assert.IsType<ExcluirGanhoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(200, excluirGanhoViewModel.StatusCode);
            Assert.Equal(DateTime.Today, excluirGanhoViewModel.DataExclusao);
            Assert.Equal(mensagemSucesso, excluirGanhoViewModel.Mensagem);
        }

        [Fact]
        public void GanhoController_ConsultarGanhos_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGanho = "uasduhasha";
            var valor = 1238.12M;
            var listaGanho = new List<Ganho>();
            var ganho = new Ganho
            {
                Id = id,
                IdCliente = idCliente,
                NomeGanho = nomeGanho,
                Recorrente = true,
                Valor = valor
            };
            var ganho1 = new Ganho
            {
                Id = id1,
                IdCliente = idCliente,
                NomeGanho = nomeGanho,
                Recorrente = false,
                Valor = valor
            };
            listaGanho.Add(ganho);
            listaGanho.Add(ganho1);
            var listarGanhos = new ConsultarGanhosViewModel.Response(listaGanho);
            _ganhoService.ConsultarGanhos(Arg.Any<Guid>()).Returns(listarGanhos);
            var controller = InicializarGanhoController();
            var token = TokenService.GerarToken(new Cliente { Id = idCliente, CPF = "12345678910", Email = "teste@teste.com", Nome = "teste da silva" });

            //Act
            var teste = controller.ConsultarGanhos(token,idCliente);
            var okResult = teste.Result as ObjectResult;
            var consultarGanhosViewModel = Assert.IsType<ConsultarGanhosViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            //verificar o ganho
            var ganhoTest = consultarGanhosViewModel.Find(g => g.Id == id);
            Assert.Equal(id, ganhoTest.Id);
            Assert.Equal(idCliente, ganhoTest.IdCliente);
            Assert.Equal(nomeGanho, ganhoTest.NomeGanho);
            Assert.Equal(valor, ganhoTest.Valor);
            Assert.True(ganhoTest.Recorrente);
            //verificar o ganho1
            var ganhoTest1 = consultarGanhosViewModel.Find(g => g.Id == id1);
            Assert.Equal(id1, ganhoTest1.Id);
            Assert.Equal(idCliente, ganhoTest1.IdCliente);
            Assert.Equal(nomeGanho, ganhoTest1.NomeGanho);
            Assert.Equal(valor, ganhoTest1.Valor);
            Assert.False(ganhoTest1.Recorrente);
        }
    }
}
