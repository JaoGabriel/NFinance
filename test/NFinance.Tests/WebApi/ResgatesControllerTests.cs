using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.ClientesViewModel;
using NFinance.Model.InvestimentosViewModel;
using NFinance.Model.ResgatesViewModel;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Collections.Generic;
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
        public void ResgateController_ListarResgates_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var resgate = new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = "Pagamento Faculdade", Valor = 1234.55M, DataResgate = DateTime.Today };
            var listaResgates = new List<Resgate>();
            listaResgates.Add(resgate);
            var listarResgates = new ListarResgatesViewModel.Response(listaResgates);
            _resgateService.ListarResgates().Returns(listarResgates);
            var controller = InicializarResgateController();
            
            //Act
            var teste = controller.ListarResgates();
            var okResult = teste.Result as ObjectResult;
            var listarResgatesViewModel = Assert.IsType<ListarResgatesViewModel.Response>(okResult.Value);
            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(id, listarResgatesViewModel.Find(r => r.Id == id).Id);
            Assert.Equal("Pagamento Faculdade", listarResgatesViewModel.Find(r => r.Id == id).MotivoResgate);
            Assert.Equal(1234.55M, listarResgatesViewModel.Find(r => r.Id == id).Valor);
            Assert.Equal(DateTime.Today, listarResgatesViewModel.Find(r => r.Id == id).DataResgate);
            Assert.Equal(idInvestimento, listarResgatesViewModel.Find(r => r.Id == id).IdInvestimento);
        }
    }
}
