using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.InvestimentosViewModel;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Collections.Generic;
using NFinance.Domain.ViewModel.ClientesViewModel;
using Xunit;

namespace NFinance.Tests.WebApi
{
    public class InvestimentosControllerTests
    {
        private readonly IInvestimentosService _investimentoService;
        private readonly ILogger<InvestimentoController> _logger;
        public InvestimentosControllerTests()
        {
            _investimentoService = Substitute.For<IInvestimentosService>();
            _logger = Substitute.For<ILogger<InvestimentoController>>();
        }

        private InvestimentoController InicializarInvestimentoController()
        {
            return new InvestimentoController(_logger, _investimentoService);
        }

        [Fact]
        public void InvestimentosController_RealizarInvestimento_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nomeInvestimento = "FIC FIM 123ADASE";
            decimal valorInvestimento = 1238.12M;
            var dataAplicacao = DateTime.Today;
            var idCliente = Guid.NewGuid();
            var nomeCliente = "Alberto Junior";
            var cliente = new ClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente };
            _investimentoService.RealizarInvestimento(Arg.Any<RealizarInvestimentoViewModel.Request>())
                .Returns(new RealizarInvestimentoViewModel.Response
                {
                    Id = id,
                    DataAplicacao = dataAplicacao,
                    NomeInvestimento = nomeInvestimento,
                    Valor = valorInvestimento,
                    Cliente = cliente
                });
            var controller = InicializarInvestimentoController();
            var investimento = new RealizarInvestimentoViewModel.Request()
            {
                DataAplicacao = dataAplicacao,
                NomeInvestimento = nomeInvestimento,
                Valor = valorInvestimento,
                IdCliente = idCliente
            };

            //Act
            var teste = controller.RealizarInvestimento(investimento);
            var okResult = teste.Result as ObjectResult;
            var realizarInvesitimentoViewModel = Assert.IsType<RealizarInvestimentoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, realizarInvesitimentoViewModel.Id);
            Assert.Equal(nomeInvestimento, realizarInvesitimentoViewModel.NomeInvestimento);
            Assert.Equal(dataAplicacao, realizarInvesitimentoViewModel.DataAplicacao);
            Assert.Equal(valorInvestimento, realizarInvesitimentoViewModel.Valor);
            Assert.Equal(idCliente, realizarInvesitimentoViewModel.Cliente.Id);
            Assert.Equal(nomeCliente, realizarInvesitimentoViewModel.Cliente.Nome);
        }

        [Fact]
        public void InvestimentosController_AtualizarInvestimento_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nomeInvestimento = "FIC FIM 123ADASE";
            decimal valorInvestimento = 1238.12M;
            var dataAplicacao = DateTime.Today;
            var idCliente = Guid.NewGuid();
            var nomeCliente = "Alberto Junior";
            var cliente = new ClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente };
            _investimentoService.AtualizarInvestimento(Arg.Any<Guid>(),Arg.Any<AtualizarInvestimentoViewModel.Request>())
                .Returns(new AtualizarInvestimentoViewModel.Response
                {
                    Id = id,
                    DataAplicacao = dataAplicacao,
                    NomeInvestimento = nomeInvestimento,
                    Valor = valorInvestimento,
                    Cliente = cliente
                });
            var controller = InicializarInvestimentoController();
            var investimento = new AtualizarInvestimentoViewModel.Request()
            {
                DataAplicacao = dataAplicacao,
                NomeInvestimento = nomeInvestimento,
                Valor = valorInvestimento,
                IdCliente = idCliente
            };

            //Act
            var teste = controller.AtualizarInvestimento(id,investimento);
            var okResult = teste.Result as ObjectResult;
            var atualizarInvesitimentoViewModel = Assert.IsType<AtualizarInvestimentoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, atualizarInvesitimentoViewModel.Id);
            Assert.Equal(nomeInvestimento, atualizarInvesitimentoViewModel.NomeInvestimento);
            Assert.Equal(dataAplicacao, atualizarInvesitimentoViewModel.DataAplicacao);
            Assert.Equal(valorInvestimento, atualizarInvesitimentoViewModel.Valor);
            Assert.Equal(idCliente, atualizarInvesitimentoViewModel.Cliente.Id);
            Assert.Equal(nomeCliente, atualizarInvesitimentoViewModel.Cliente.Nome);
        }

        [Fact]
        public void InvestimentosController_ConsultarInvestimento_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nomeInvestimento = "FIC FIM 123ADASE";
            decimal valorInvestimento = 1238.12M;
            var dataAplicacao = DateTime.Today;
            var idCliente = Guid.NewGuid();
            var nomeCliente = "Alberto Junior";
            var cliente = new ClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente };
            _investimentoService.ConsultarInvestimento(Arg.Any<Guid>())
                .Returns(new ConsultarInvestimentoViewModel.Response
                {
                    Id = id,
                    DataAplicacao = dataAplicacao,
                    NomeInvestimento = nomeInvestimento,
                    Valor = valorInvestimento,
                    Cliente = cliente
                });
            var controller = InicializarInvestimentoController();

            //Act
            var teste = controller.ConsultarInvestimento(id);
            var okResult = teste.Result as ObjectResult;
            var consultarInvesitimentoViewModel = Assert.IsType<ConsultarInvestimentoViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, consultarInvesitimentoViewModel.Id);
            Assert.Equal(nomeInvestimento, consultarInvesitimentoViewModel.NomeInvestimento);
            Assert.Equal(dataAplicacao, consultarInvesitimentoViewModel.DataAplicacao);
            Assert.Equal(valorInvestimento, consultarInvesitimentoViewModel.Valor);
            Assert.Equal(idCliente, consultarInvesitimentoViewModel.Cliente.Id);
            Assert.Equal(nomeCliente, consultarInvesitimentoViewModel.Cliente.Nome);
        }

        [Fact]
        public void InvestimentosController_ListarInvestimentos_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nomeInvestimento = "FIC FIM 123ADASE";
            decimal valorInvestimento = 1238.12M;
            var dataAplicacao = DateTime.Today;
            var idCliente = Guid.NewGuid();
            var investimento = new Investimentos() {  Id = id,DataAplicacao = dataAplicacao, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, IdCliente = idCliente };
            var listaInvestimento = new List<Investimentos>();
            listaInvestimento.Add(investimento);
            var listarInvestimentos = new ListarInvestimentosViewModel.Response(listaInvestimento);
            _investimentoService.ListarInvestimentos().Returns(listarInvestimentos);
            var controller = InicializarInvestimentoController();

            //Act
            var teste = controller.ListarInvestimentos();
            var okResult = teste.Result as ObjectResult;
            var listarInvesitimentoViewModel = Assert.IsType<ListarInvestimentosViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, listarInvesitimentoViewModel.Find(i => i.Id == id).Id);
            Assert.Equal(nomeInvestimento, listarInvesitimentoViewModel.Find(i => i.Id == id).NomeInvestimento);
            Assert.Equal(dataAplicacao, listarInvesitimentoViewModel.Find(i => i.Id == id).DataAplicacao);
            Assert.Equal(valorInvestimento, listarInvesitimentoViewModel.Find(i => i.Id == id).Valor);
            Assert.Equal(idCliente, listarInvesitimentoViewModel.Find(i => i.Id == id).IdCliente);
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
            var listaInvestimento = new List<Investimentos>();
            var investimento = new Investimentos
            {
                Id = id,
                IdCliente = idCliente,
                NomeInvestimento = nomeInvestimento,
                Valor = valor,
                DataAplicacao = dataAplicacao
            };
            var investimento1 = new Investimentos
            {
                Id = id1,
                IdCliente = idCliente,
                NomeInvestimento = nomeInvestimento,
                Valor = valor,
                DataAplicacao = dataAplicacao
            };
            listaInvestimento.Add(investimento);
            listaInvestimento.Add(investimento1);
            var listarInvestimentos = new ConsultarInvestimentosViewModel.Response(listaInvestimento);
            _investimentoService.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listarInvestimentos);
            var controller = InicializarInvestimentoController();

            //Act
            var teste = controller.ConsultarInvestimentos(idCliente);
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
