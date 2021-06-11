using Xunit;
using System;
using NSubstitute;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.GastosViewModel;
using NFinance.Application;

namespace NFinance.Tests.WebApi
{
    public class GastosControllerTests
    {
        private readonly IGastoApp _gastosApp;
        private readonly IAutenticacaoApp _autenticacaoApp;
        private readonly ILogger<GastosController> _logger;
        public GastosControllerTests()
        {
            _gastosApp = Substitute.For<IGastoApp>();
            _autenticacaoApp = Substitute.For<IAutenticacaoApp>();
            _logger = Substitute.For<ILogger<GastosController>>();
        }

        private GastosController InicializarGastosController()
        {
            return new GastosController(_logger, _gastosApp, _autenticacaoApp);
        }

        public static Gasto GeraGasto()
        {
            return new Gasto(Guid.NewGuid(),"uhsdauhsduh",237891289.0923M,10,DateTime.Today);
        }

        public static Cliente GeraCliente()
        {
            return new Cliente("ASDASD", "12345678910","teste@tst.com","832911");
        }

        [Fact]
        public void GastosController_CadastrarGasto_ComSucesso()
        {
            //Arrange
            var gasto = GeraGasto();
            _gastosApp.CadastrarGasto(Arg.Any<CadastrarGastoViewModel.Request>()).Returns(new CadastrarGastoViewModel.Response(gasto));
            var controller = InicializarGastosController();
            var gastoRequest = new CadastrarGastoViewModel.Request();
            var token = TokenApp.GerarToken(GeraCliente());

            //Act
            var teste = controller.CadastrarGasto(token,gastoRequest);
            var okResult = teste.Result as ObjectResult;
            var cadastrarGastoViewModel = Assert.IsType<CadastrarGastoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(gasto.Id, cadastrarGastoViewModel.Id);
            Assert.Equal(gasto.NomeGasto, cadastrarGastoViewModel.NomeGasto);
            Assert.Equal(gasto.QuantidadeParcelas, cadastrarGastoViewModel.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto, cadastrarGastoViewModel.DataDoGasto);
            Assert.Equal(gasto.Valor, cadastrarGastoViewModel.Valor);
            Assert.Equal(gasto.IdCliente, cadastrarGastoViewModel.IdCliente);
        }

        [Fact]
        public void GastosController_AtualizarGasto_ComSucesso()
        {
            //Arrange
            var gasto = GeraGasto();
            _gastosApp.AtualizarGasto(Arg.Any<Guid>(),Arg.Any<AtualizarGastoViewModel.Request>()).Returns(new AtualizarGastoViewModel.Response(gasto));
            var controller = InicializarGastosController();
            var gastoRequest = new AtualizarGastoViewModel.Request();
            var token = TokenApp.GerarToken(GeraCliente());

            //Act
            var teste = controller.AtualizarGasto(token,gasto.Id,gastoRequest);
            var okResult = teste.Result as ObjectResult;
            var atualizarGastoViewModel = Assert.IsType<AtualizarGastoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(gasto.Id, atualizarGastoViewModel.Id);
            Assert.Equal(gasto.NomeGasto, atualizarGastoViewModel.NomeGasto);
            Assert.Equal(gasto.QuantidadeParcelas, atualizarGastoViewModel.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto, atualizarGastoViewModel.DataDoGasto);
            Assert.Equal(gasto.Valor, atualizarGastoViewModel.Valor);
            Assert.Equal(gasto.IdCliente, atualizarGastoViewModel.IdCliente);
        }

        [Fact]
        public void GastosController_ConsultarGasto_ComSucesso()
        {
            //Arrange
            var gasto = GeraGasto();
            _gastosApp.ConsultarGasto(Arg.Any<Guid>()).Returns(new ConsultarGastoViewModel.Response(gasto));
            var controller = InicializarGastosController();
            var token = TokenApp.GerarToken(GeraCliente());

            //Act
            var teste = controller.ConsultarGasto(token,gasto.Id);
            var okResult = teste.Result as ObjectResult;
            var consultarGastoViewModel = Assert.IsType<ConsultarGastoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(gasto.Id, consultarGastoViewModel.Id);
            Assert.Equal(gasto.NomeGasto, consultarGastoViewModel.NomeGasto);
            Assert.Equal(gasto.QuantidadeParcelas, consultarGastoViewModel.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto, consultarGastoViewModel.DataDoGasto);
            Assert.Equal(gasto.Valor, consultarGastoViewModel.Valor);
            Assert.Equal(gasto.IdCliente, consultarGastoViewModel.IdCliente);
        }

        [Fact]
        public void GastosController_ExcluirGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var dataExclusao = DateTime.UtcNow;
            var idCliente = Guid.NewGuid();
            _gastosApp.ExcluirGasto(Arg.Any<ExcluirGastoViewModel.Request>()).Returns(new ExcluirGastoViewModel.Response { Mensagem = "Excluido Com Sucesso", StatusCode = 200, DataExclusao = dataExclusao });
            var controller = InicializarGastosController();
            var gasto = new ExcluirGastoViewModel.Request { IdGasto = id, MotivoExclusao = "Finalizado Pagamento", IdCliente = idCliente };
            var token = TokenApp.GerarToken(GeraCliente());

            //Act
            var teste = controller.ExcluirGasto(token,gasto);
            var okResult = teste.Result as ObjectResult;
            var excluirGastoViewModel = Assert.IsType<ExcluirGastoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(dataExclusao, excluirGastoViewModel.DataExclusao);
            Assert.Equal("Excluido Com Sucesso", excluirGastoViewModel.Mensagem);
            Assert.Equal(200, excluirGastoViewModel.StatusCode);
        }

        [Fact]
        public void GastosController_ConsultarGastos_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "uasduhasha";
            var valor = 1238.12M;
            var dataGasto = DateTime.Today;
            var listaGasto = new List<Gasto>();
            var gasto = new Gasto { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valor, QuantidadeParcelas = 5, DataDoGasto = dataGasto };
            var gasto1 = new Gasto { Id = id1, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valor, QuantidadeParcelas = 5, DataDoGasto = dataGasto };
            listaGasto.Add(gasto);
            listaGasto.Add(gasto1);
            var listarGastos = new ConsultarGastosViewModel.Response(listaGasto);
            _gastosApp.ConsultarGastos(Arg.Any<Guid>()).Returns(listarGastos);
            var controller = InicializarGastosController();
            var token = TokenApp.GerarToken(GeraCliente());

            //Act
            var teste = controller.ConsultarGastos(token,idCliente);
            var okResult = teste.Result as ObjectResult;
            var consultarGastosViewModel = Assert.IsType<ConsultarGastosViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            //verificar o gasto
            var gastoTest = consultarGastosViewModel.Find(g => g.Id == id);
            Assert.Equal(id, gastoTest.Id);
            Assert.Equal(idCliente, gastoTest.IdCliente);
            Assert.Equal(nomeGasto, gastoTest.NomeGasto);
            Assert.Equal(valor, gastoTest.Valor);
            Assert.Equal(5, gastoTest.QuantidadeParcelas);
            Assert.Equal(dataGasto, gastoTest.DataDoGasto);
            //verificar o gasto1
            var gastoTest1 = consultarGastosViewModel.Find(g => g.Id == id1);
            Assert.Equal(id1, gastoTest1.Id);
            Assert.Equal(idCliente, gastoTest1.IdCliente);
            Assert.Equal(nomeGasto, gastoTest1.NomeGasto);
            Assert.Equal(valor, gastoTest1.Valor);
            Assert.Equal(5, gastoTest1.QuantidadeParcelas);
            Assert.Equal(dataGasto, gastoTest1.DataDoGasto);
        }
    }
}
