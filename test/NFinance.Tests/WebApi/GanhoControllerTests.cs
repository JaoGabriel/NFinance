using Xunit;
using System;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NFinance.Domain.Identidade;

namespace NFinance.Tests.WebApi
{
    public class GanhoControllerTests
    {
        private readonly Mock<IGanhoApp> _ganhoApp;
        private readonly Mock<ILogger<GanhoController>> _logger;

        public GanhoControllerTests()
        {
            _ganhoApp = new Mock<IGanhoApp>();
            _logger = new Mock<ILogger<GanhoController>>();
        }

        private GanhoController InicializarGanhoController()
        {
            return new(_logger.Object, _ganhoApp.Object);
        }

        public static Ganho GeraGanho()
        {
            return new(Guid.NewGuid(),"Salario",231207983.201983M,true,DateTime.Today);
        }

        private static Usuario GeraUsuario()
        {
            return new() { Id = Guid.NewGuid(), Email = "teste@teste.com", PasswordHash = "123456" };
        }

        [Fact]
        public void GanhoController_CadastrarGanho_ComSucesso()
        {
            //Arrange
            var ganho = GeraGanho();
            _ganhoApp.Setup(x => x.CadastrarGanho(It.IsAny<CadastrarGanhoViewModel.Request>())).ReturnsAsync(new CadastrarGanhoViewModel.Response(ganho));
            var controller = InicializarGanhoController();
            var ganhoRequest = new CadastrarGanhoViewModel.Request();

            //Act
            var teste = controller.CadastrarGanho(ganhoRequest);
            var okResult = teste.Result as ObjectResult;
            var cadastrarGanhoViewModel = Assert.IsType<CadastrarGanhoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(ganho.Id, cadastrarGanhoViewModel.Id);
            Assert.Equal(ganho.NomeGanho, cadastrarGanhoViewModel.NomeGanho);
            Assert.Equal(ganho.Valor, cadastrarGanhoViewModel.Valor);
            Assert.Equal(ganho.IdCliente, cadastrarGanhoViewModel.IdCliente);
            Assert.Equal(ganho.Id, cadastrarGanhoViewModel.Id);
            Assert.True(cadastrarGanhoViewModel.Recorrente);
        }

        [Fact]
        public void GanhoController_AtualizarGanho_ComSucesso()
        {
            //Arrange
            var ganho = GeraGanho();
            _ganhoApp.Setup(x => x.AtualizarGanho(It.IsAny<Guid>(),It.IsAny<AtualizarGanhoViewModel.Request>())).ReturnsAsync(new AtualizarGanhoViewModel.Response(ganho));
            var controller = InicializarGanhoController();
            var ganhoRequest = new AtualizarGanhoViewModel.Request(ganho);
            var token = TokenApp.GerarToken(GeraUsuario());

            //Act
            var teste = controller.AtualizarGanho(token,ganho.Id,ganhoRequest);
            var okResult = teste.Result as ObjectResult;
            var atualizarGanhoViewModel = Assert.IsType<AtualizarGanhoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(ganho.Id, atualizarGanhoViewModel.Id);
            Assert.Equal(ganho.NomeGanho, atualizarGanhoViewModel.NomeGanho);
            Assert.Equal(ganho.Valor, atualizarGanhoViewModel.Valor);
            Assert.Equal(ganho.IdCliente, atualizarGanhoViewModel.IdCliente);
            Assert.Equal(ganho.Id, atualizarGanhoViewModel.Id);
            Assert.True(atualizarGanhoViewModel.Recorrente);
        }

        [Fact]
        public void GanhoController_ConsultarGanho_ComSucesso()
        {
            //Arrange
            var ganho = GeraGanho();
            _ganhoApp.Setup(x => x.ConsultarGanho(It.IsAny<Guid>())).ReturnsAsync(new ConsultarGanhoViewModel.Response(ganho));
            var controller = InicializarGanhoController();
            var token = TokenApp.GerarToken(GeraUsuario());
            
            //Act
            var teste = controller.ConsultarGanho(token,ganho.Id);
            var okResult = teste.Result as ObjectResult;
            var consultarGanhoViewModel = Assert.IsType<ConsultarGanhoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(ganho.Id, consultarGanhoViewModel.Id);
            Assert.Equal(ganho.NomeGanho, consultarGanhoViewModel.NomeGanho);
            Assert.Equal(ganho.Valor, consultarGanhoViewModel.Valor);
            Assert.Equal(ganho.IdCliente, consultarGanhoViewModel.IdCliente);
            Assert.Equal(ganho.Id, consultarGanhoViewModel.Id);
            Assert.True(consultarGanhoViewModel.Recorrente);
        }

        [Fact]
        public void GanhoController_ExcluirGanho_ComSucesso()
        {
            //Arrange
            var idGanho = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var motivo = "Perdio money";
            var mensagemSucesso = "Excluido com sucesso";
            _ganhoApp.Setup(x => x.ExcluirGanho(It.IsAny<ExcluirGanhoViewModel.Request>())).ReturnsAsync(new ExcluirGanhoViewModel.Response {StatusCode = 200, DataExclusao = DateTime.Today, Mensagem = mensagemSucesso});
            var controller = InicializarGanhoController();
            var ganho = new ExcluirGanhoViewModel.Request {IdCliente = idCliente, IdGanho = idGanho, MotivoExclusao = motivo};
            var token = TokenApp.GerarToken(GeraUsuario());

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
            _ganhoApp.Setup(x => x.ConsultarGanhos(It.IsAny<Guid>())).ReturnsAsync(listarGanhos);
            var controller = InicializarGanhoController();
            var token = TokenApp.GerarToken(GeraUsuario());

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
