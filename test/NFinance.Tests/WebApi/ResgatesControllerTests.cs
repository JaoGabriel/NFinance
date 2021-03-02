using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.InvestimentosViewModel;
using NFinance.Model.ResgatesViewModel;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Collections.Generic;
using NFinance.Domain.ViewModel.ClientesViewModel;
using Xunit;

namespace NFinance.Tests.WebApi
{
    public class ResgatesControllerTests
    {
        private readonly IResgateService _resgateService;
        private readonly ILogger<ResgateController> _logger;
        public ResgatesControllerTests()
        {
            _resgateService = Substitute.For<IResgateService>();
            _logger = Substitute.For<ILogger<ResgateController>>();
        }

        private ResgateController InicializarResgateController()
        {
            return new ResgateController(_logger, _resgateService);
        }

        [Fact]
        public void ResgateController_RealizarResgate_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var cliente = new ClienteViewModel.Response() {Id = idCliente, Nome = "Joao Testes"};
            var investimento = new InvestimentoViewModel() { Id = idInvestimento, DataAplicacao = DateTime.Today.AddMonths(-4), NomeInvestimento = "Investimento ABC", Valor = 418371623812.23M, Cliente = cliente };
            _resgateService.RealizarResgate(Arg.Any<RealizarResgateViewModel.Request>())
                .Returns(new RealizarResgateViewModel.Response
                {
                    Id = id,
                    Investimento = investimento,
                    MotivoResgate = "Pagamento Faculdade",
                    Valor = 1234.55M,
                    DataResgate = DateTime.Today
                });
            var controller = InicializarResgateController();
            var resgate = new RealizarResgateViewModel.Request()
            {
                IdInvestimento = idInvestimento,
                MotivoResgate = "Pagamento Faculdade",
                Valor = 1234.55M,
                DataResgate = DateTime.Today
            };
            //Act
            var teste = controller.RealizarResgate(resgate);
            var okResult = teste.Result as ObjectResult;
            var RealizarResgateViewModel = Assert.IsType<RealizarResgateViewModel.Response>(okResult.Value);
            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, RealizarResgateViewModel.Id);
            Assert.Equal("Pagamento Faculdade", RealizarResgateViewModel.MotivoResgate);
            Assert.Equal(1234.55M, RealizarResgateViewModel.Valor);
            Assert.Equal(DateTime.Today, RealizarResgateViewModel.DataResgate);
            Assert.Equal(418371623812.23M, RealizarResgateViewModel.Investimento.Valor);
            Assert.Equal("Investimento ABC", RealizarResgateViewModel.Investimento.NomeInvestimento);
            Assert.Equal(idInvestimento, RealizarResgateViewModel.Investimento.Id);
            Assert.Equal(DateTime.Today.AddMonths(-4), RealizarResgateViewModel.Investimento.DataAplicacao);
            Assert.Equal(idCliente, RealizarResgateViewModel.Investimento.Cliente.Id);
            Assert.Equal("Joao Testes", RealizarResgateViewModel.Investimento.Cliente.Nome);
        }

        [Fact]
        public void ResgateController_ConsultarResgate_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var cliente = new ClienteViewModel.Response() { Id = idCliente, Nome = "Joao Testes" };
            var investimento = new InvestimentoViewModel() { Id = idInvestimento, DataAplicacao = DateTime.Today.AddMonths(-4), NomeInvestimento = "Investimento ABC", Valor = 418371623812.23M, Cliente = cliente };
            _resgateService.ConsultarResgate(Arg.Any<Guid>())
                .Returns(new ConsultarResgateViewModel.Response
                {
                    Id = id,
                    Investimento = investimento,
                    MotivoResgate = "Pagamento Faculdade",
                    Valor = 1234.55M,
                    DataResgate = DateTime.Today
                });
            var controller = InicializarResgateController();

            //Act
            var teste = controller.ConsultarResgate(id);
            var okResult = teste.Result as ObjectResult;
            var consultarResgateViewModel = Assert.IsType<ConsultarResgateViewModel.Response>(okResult.Value);
            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, consultarResgateViewModel.Id);
            Assert.Equal("Pagamento Faculdade", consultarResgateViewModel.MotivoResgate);
            Assert.Equal(1234.55M, consultarResgateViewModel.Valor);
            Assert.Equal(DateTime.Today, consultarResgateViewModel.DataResgate);
            Assert.Equal(418371623812.23M, consultarResgateViewModel.Investimento.Valor);
            Assert.Equal("Investimento ABC", consultarResgateViewModel.Investimento.NomeInvestimento);
            Assert.Equal(idInvestimento, consultarResgateViewModel.Investimento.Id);
            Assert.Equal(DateTime.Today.AddMonths(-4), consultarResgateViewModel.Investimento.DataAplicacao);
            Assert.Equal(idCliente, consultarResgateViewModel.Investimento.Cliente.Id);
            Assert.Equal("Joao Testes", consultarResgateViewModel.Investimento.Cliente.Nome);
        }

        [Fact]
        public void ResgateController_ConsultarResgates_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivoResgate = "uasduhasha";
            var valor = 1238.12M;
            var dataResgate = DateTime.Today;
            var listaResgate = new List<Resgate>();
            var gasto = new Resgate
            {
                Id = id,
                IdInvestimento = idInvestimento,
                MotivoResgate = motivoResgate,
                Valor = valor,
                DataResgate = dataResgate
            };
            var gasto1 = new Resgate
            {
                Id = id1,
                IdInvestimento = idInvestimento,
                MotivoResgate = motivoResgate,
                Valor = valor,
                DataResgate = dataResgate
            };
            listaResgate.Add(gasto);
            listaResgate.Add(gasto1);
            var listarResgates = new ConsultarResgatesViewModel.Response(listaResgate);
            _resgateService.ConsultarResgates(Arg.Any<Guid>()).Returns(listarResgates);
            var controller = InicializarResgateController();

            //Act
            var teste = controller.ConsultarResgates(idInvestimento);
            var okResult = teste.Result as ObjectResult;
            var consultarResgatesViewModel = Assert.IsType<ConsultarResgatesViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            //verificar o gasto
            var resgateTest = consultarResgatesViewModel.Find(g => g.Id == id);
            Assert.Equal(id, resgateTest.Id);
            Assert.Equal(idInvestimento, resgateTest.IdInvestimento);
            Assert.Equal(motivoResgate, resgateTest.MotivoResgate);
            Assert.Equal(valor, resgateTest.Valor);
            Assert.Equal(dataResgate, resgateTest.DataResgate);
            //verificar o gasto1
            var resgateTest1 = consultarResgatesViewModel.Find(g => g.Id == id1);
            Assert.Equal(id1, resgateTest1.Id);
            Assert.Equal(idInvestimento, resgateTest1.IdInvestimento);
            Assert.Equal(motivoResgate, resgateTest1.MotivoResgate);
            Assert.Equal(valor, resgateTest1.Valor);
            Assert.Equal(dataResgate, resgateTest1.DataResgate);
        }
    }
}
