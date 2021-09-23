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
    public class InvestimentosControllerTests
    {
        private readonly Mock<IInvestimentoApp> _investimentoApp;
        private readonly Mock<ILogger<InvestimentoController>> _logger;
        public InvestimentosControllerTests()
        {
            _investimentoApp = new Mock<IInvestimentoApp>();
            _logger = new Mock<ILogger<InvestimentoController>>();
        }

        private InvestimentoController InicializarInvestimentoController()
        {
            return new(_logger.Object, _investimentoApp.Object);
        }

        public static Investimento GeraInvestimento()
        {
            return new(Guid.NewGuid(),"asdygaygsd",37219783.09M,DateTime.Today);
        }

        private static Usuario GeraUsuario()
        {
            return new() { Id = Guid.NewGuid(), Email = "teste@teste.com", PasswordHash = "123456" };
        }

        public static Cliente GeraCliente()
        {
            return new("ASDASD", "12345678910", "teste@tst.com", GeraUsuario());
        }

        [Fact]
        public void InvestimentosController_RealizarInvestimento_ComSucesso()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentoApp.Setup(x => x.RealizarInvestimento(It.IsAny<RealizarInvestimentoViewModel.Request>())).ReturnsAsync(new RealizarInvestimentoViewModel.Response(investimento));
            var controller = InicializarInvestimentoController();
            var investimentoRequest = new RealizarInvestimentoViewModel.Request();
            var token = TokenApp.GerarToken(GeraUsuario());

            //Act
            var teste = controller.RealizarInvestimento(token, investimentoRequest);
            var okResult = teste.Result as ObjectResult;
            var realizarInvesitimentoViewModel = Assert.IsType<RealizarInvestimentoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(investimento.Id, realizarInvesitimentoViewModel.Id);
            Assert.Equal(investimento.NomeInvestimento, realizarInvesitimentoViewModel.NomeInvestimento);
            Assert.Equal(investimento.DataAplicacao, realizarInvesitimentoViewModel.DataAplicacao);
            Assert.Equal(investimento.Valor, realizarInvesitimentoViewModel.Valor);
            Assert.Equal(investimento.IdCliente, realizarInvesitimentoViewModel.IdCliente);
        }

        [Fact]
        public void InvestimentosController_AtualizarInvestimento_ComSucesso()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentoApp.Setup(x => x.AtualizarInvestimento(It.IsAny<Guid>(),It.IsAny<AtualizarInvestimentoViewModel.Request>())).ReturnsAsync(new AtualizarInvestimentoViewModel.Response(investimento));
            var controller = InicializarInvestimentoController();
            var investimentoRequest = new AtualizarInvestimentoViewModel.Request();
            var token = TokenApp.GerarToken(GeraUsuario());

            //Act
            var teste = controller.AtualizarInvestimento(token,investimento.Id,investimentoRequest);
            var okResult = teste.Result as ObjectResult;
            var atualizarInvesitimentoViewModel = Assert.IsType<AtualizarInvestimentoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(investimento.Id, atualizarInvesitimentoViewModel.Id);
            Assert.Equal(investimento.NomeInvestimento, atualizarInvesitimentoViewModel.NomeInvestimento);
            Assert.Equal(investimento.DataAplicacao, atualizarInvesitimentoViewModel.DataAplicacao);
            Assert.Equal(investimento.Valor, atualizarInvesitimentoViewModel.Valor);
            Assert.Equal(investimento.IdCliente, atualizarInvesitimentoViewModel.IdCliente);
        }

        [Fact]
        public void InvestimentosController_ConsultarInvestimento_ComSucesso()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentoApp.Setup(x => x.ConsultarInvestimento(It.IsAny<Guid>())).ReturnsAsync(new ConsultarInvestimentoViewModel.Response(investimento));
            var controller = InicializarInvestimentoController();
            var token = TokenApp.GerarToken(GeraUsuario());

            //Act
            var teste = controller.ConsultarInvestimento(token,investimento.Id);
            var okResult = teste.Result as ObjectResult;
            var consultarInvesitimentoViewModel = Assert.IsType<ConsultarInvestimentoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(investimento.Id, consultarInvesitimentoViewModel.Id);
            Assert.Equal(investimento.NomeInvestimento, consultarInvesitimentoViewModel.NomeInvestimento);
            Assert.Equal(investimento.DataAplicacao, consultarInvesitimentoViewModel.DataAplicacao);
            Assert.Equal(investimento.Valor, consultarInvesitimentoViewModel.Valor);
            Assert.Equal(investimento.IdCliente, consultarInvesitimentoViewModel.IdCliente);
        }

        [Fact]
        public void InvestimentosController_ConsultarInvestimentos_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeInvestimento = "uasduhasha";
            var valor = 1238.12M;
            var dataAplicacao = DateTime.Today;
            var listaInvestimento = new List<Investimento>();
            var investimento = new Investimento { Id = id, IdCliente = idCliente, NomeInvestimento = nomeInvestimento,Valor = valor, DataAplicacao = dataAplicacao };
            var investimento1 = new Investimento { Id = id1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valor, DataAplicacao = dataAplicacao };
            listaInvestimento.Add(investimento);
            listaInvestimento.Add(investimento1);
            var listarInvestimentos = new ConsultarInvestimentosViewModel.Response(listaInvestimento);
            _investimentoApp.Setup(x => x.ConsultarInvestimentos(It.IsAny<Guid>())).ReturnsAsync(listarInvestimentos);
            var controller = InicializarInvestimentoController();

            //Act
            var token = TokenApp.GerarToken(GeraUsuario());
            var teste = controller.ConsultarInvestimentos(token,idCliente);
            var okResult = teste.Result as ObjectResult;
            var consultarInvestimentosViewModel = Assert.IsType<ConsultarInvestimentosViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            //verificar o investimento
            var investimentoTest = consultarInvestimentosViewModel.Find(g => g.Id == id);
            Assert.Equal(id, investimentoTest.Id);
            Assert.Equal(idCliente, investimentoTest.IdCliente);
            Assert.Equal(nomeInvestimento, investimentoTest.NomeInvestimento);
            Assert.Equal(valor, investimentoTest.Valor);
            Assert.Equal(dataAplicacao, investimentoTest.DataAplicacao);
            //verificar o investimento1
            var investimentoTest1 = consultarInvestimentosViewModel.Find(g => g.Id == id1);
            Assert.Equal(id1, investimentoTest1.Id);
            Assert.Equal(idCliente, investimentoTest1.IdCliente);
            Assert.Equal(nomeInvestimento, investimentoTest1.NomeInvestimento);
            Assert.Equal(valor, investimentoTest1.Valor);
            Assert.Equal(dataAplicacao, investimentoTest1.DataAplicacao);
        }
    }
}
