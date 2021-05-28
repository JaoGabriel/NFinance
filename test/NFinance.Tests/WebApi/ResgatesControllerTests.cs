using Xunit;
using System;
using NSubstitute;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using NFinance.Domain.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.ResgatesViewModel;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.Tests.WebApi
{
    public class ResgatesControllerTests
    {
        private readonly IResgateApp _resgateApp;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<ResgateController> _logger;

        public ResgatesControllerTests()
        {
            _resgateApp = Substitute.For<IResgateApp>();
            _autenticacaoService = Substitute.For<IAutenticacaoService>();
            _logger = Substitute.For<ILogger<ResgateController>>();
        }

        private ResgateController InicializarResgateController()
        {
            return new ResgateController(_logger, _resgateApp, _autenticacaoService);
        }

        public static Resgate GeraResgate()
        {
            return new Resgate(Guid.NewGuid(),Guid.NewGuid(),32138123987.20138M,"asuhdasud",DateTime.Today);
        }
        public static Cliente GeraCliente()
        {
            return new Cliente("ASDASD", "12345678910", "teste@tst.com", "832911");
        }

        [Fact]
        public void ResgateController_RealizarResgate_ComSucesso()
        {
            //Arrange
            var resgate = GeraResgate();
            _resgateApp.RealizarResgate(Arg.Any<RealizarResgateViewModel.Request>()).Returns(new RealizarResgateViewModel.Response(resgate));
            var controller = InicializarResgateController();
            var resgateRequest = new RealizarResgateViewModel.Request();
            var token = TokenService.GerarToken(GeraCliente());
            Substitute.For<InvestimentoViewModel>().Returns(new InvestimentoViewModel {Id = resgate.IdInvestimento, IdCliente = resgate.IdCliente, NomeInvestimento = "Investimento ABC", Valor = 32138123987.20138M, DataAplicacao = DateTime.Today });
           
            //Act
            var teste = controller.RealizarResgate(token, resgateRequest);
            var okResult = teste.Result as ObjectResult;
            var realizarResgateViewModel = Assert.IsType<RealizarResgateViewModel.Response>(okResult.Value);
            
            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(resgate.Id, realizarResgateViewModel.Id);
            Assert.Equal(resgate.MotivoResgate, realizarResgateViewModel.MotivoResgate);
            Assert.Equal(resgate.Valor, realizarResgateViewModel.Valor);
            Assert.Equal(resgate.DataResgate, realizarResgateViewModel.DataResgate);
            Assert.Equal(resgate.IdInvestimento, realizarResgateViewModel.Investimento.Id);
            Assert.Equal("Investimento ABC", realizarResgateViewModel.Investimento.NomeInvestimento);
            Assert.Equal(resgate.IdCliente, realizarResgateViewModel.Investimento.IdCliente);
            Assert.Equal(DateTime.Today, realizarResgateViewModel.Investimento.DataAplicacao);
            Assert.Equal(resgate.Valor, realizarResgateViewModel.Investimento.Valor);
        }

        [Fact]
        public void ResgateController_ConsultarResgate_ComSucesso()
        {
            //Arrange
            var resgate = GeraResgate();
            Substitute.For<InvestimentoViewModel>().Returns(new InvestimentoViewModel { Id = resgate.IdInvestimento, DataAplicacao = DateTime.Today.AddMonths(-4), NomeInvestimento = "Investimento ABC", Valor = 32138123987.20138M, IdCliente = resgate.IdCliente});
            _resgateApp.ConsultarResgate(Arg.Any<Guid>()).Returns(new ConsultarResgateViewModel.Response(resgate));
            var controller = InicializarResgateController();

            //Act
            var token = TokenService.GerarToken(GeraCliente());
            var teste = controller.ConsultarResgate(token,resgate.Id);
            var okResult = teste.Result as ObjectResult;
            var consultarResgateViewModel = Assert.IsType<ConsultarResgateViewModel.Response>(okResult.Value);
            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(resgate.Id, consultarResgateViewModel.Id);
            Assert.Equal(resgate.MotivoResgate, consultarResgateViewModel.MotivoResgate);
            Assert.Equal(resgate.Valor, consultarResgateViewModel.Valor);
            Assert.Equal(DateTime.Today, consultarResgateViewModel.DataResgate);
            Assert.Equal(418371623812.23M, consultarResgateViewModel.Investimento.Valor);
            Assert.Equal("Investimento ABC", consultarResgateViewModel.Investimento.NomeInvestimento);
            Assert.Equal(resgate.IdInvestimento, consultarResgateViewModel.Investimento.Id);
            Assert.Equal(DateTime.Today.AddMonths(-4), consultarResgateViewModel.Investimento.DataAplicacao);
            Assert.Equal(resgate.IdCliente, consultarResgateViewModel.IdCliente);
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
            _resgateApp.ConsultarResgates(Arg.Any<Guid>()).Returns(listarResgates);
            var controller = InicializarResgateController();
            var token = TokenService.GerarToken(new Cliente { Id = Guid.NewGuid(), CPF = "12345678910", Email = "teste@teste.com", Nome = "teste da silva" });

            //Act
            var teste = controller.ConsultarResgates(token,idInvestimento);
            var okResult = teste.Result as ObjectResult;
            var consultarResgatesViewModel = Assert.IsType<ConsultarResgatesViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            //verificar o gasto
            var resgateTest = consultarResgatesViewModel.Find(g => g.Id == id);
            Assert.Equal(id, resgateTest.Id);
            Assert.Equal(idInvestimento, resgateTest.Investimento.Id);
            Assert.Equal(motivoResgate, resgateTest.MotivoResgate);
            Assert.Equal(valor, resgateTest.Valor);
            Assert.Equal(dataResgate, resgateTest.DataResgate);
            //verificar o gasto1
            var resgateTest1 = consultarResgatesViewModel.Find(g => g.Id == id1);
            Assert.Equal(id1, resgateTest1.Id);
            Assert.Equal(idInvestimento, resgateTest1.Investimento.Id);
            Assert.Equal(motivoResgate, resgateTest1.MotivoResgate);
            Assert.Equal(valor, resgateTest1.Valor);
            Assert.Equal(dataResgate, resgateTest1.DataResgate);
        }
    }
}
