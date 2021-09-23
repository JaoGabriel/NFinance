using System;
using Xunit;
using System.Linq;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.GanhoViewModel;
using NFinance.Application.ViewModel.GastosViewModel;
using NFinance.Application.ViewModel.ResgatesViewModel;
using NFinance.Application.ViewModel.TelaInicialViewModel;
using NFinance.Application.ViewModel.InvestimentosViewModel;
using NFinance.Application;
using NFinance.Domain.Identidade;

namespace NFinance.Tests.WebApi
{
    public class TelaInicialControllerTests
    {
        private readonly ITelaInicialApp _telaInicialApp;
        private readonly ILogger<TelaInicialController> _logger;

        public TelaInicialControllerTests()
        {
            _telaInicialApp = Substitute.For<ITelaInicialApp>();
            _logger = Substitute.For<ILogger<TelaInicialController>>();
        }

        private TelaInicialController InicializarLoginController()
        {
            return new(_logger, _telaInicialApp);
        }

        private static Usuario GeraUsuario()
        {
            return new() { Id = Guid.NewGuid(), Email = "teste@teste.com", PasswordHash = "123456" };
        }

        public static Cliente GeraCliente()
        {
            return new("ASDASD", "12345678910", "teste@tst.com", GeraUsuario());
        }

        private static Gasto GeraGasto(Cliente cliente)
        {
            return new(cliente.Id, "Caixas", 1000M, 3, DateTime.Today.AddDays(3));
        }

        private static Investimento GeraInvestimento(Cliente cliente)
        {
            return new(cliente.Id, "CDB", 10000M, DateTime.Today);
        }

        private static Resgate GeraResgate(Investimento investimento, Cliente cliente)
        {
            return new(investimento.Id, cliente.Id, 5000M, "Necessidade", DateTime.Today.AddDays(-2));
        }

        private static Ganho GeraGanho(Cliente cliente)
        {
            return new(cliente.Id, "Salario", 5000M, true, DateTime.Today);
        }

        [Fact]
        public void LoginController_ExibirDados_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var ganho = new GanhoViewModel(GeraGanho(cliente));
            var gasto = new GastoViewModel(GeraGasto(cliente));
            var investimento = GeraInvestimento(cliente);
            var investimentoVm = new InvestimentoViewModel(investimento);
            var resgate = new ResgateViewModel(GeraResgate(investimento,cliente));
            var listGanho = new List<GanhoViewModel> { ganho };
            var listGasto = new List<GastoViewModel> { gasto };
            var listInvestimento = new List<InvestimentoViewModel> { investimentoVm };
            var listResgate = new List<ResgateViewModel> { resgate };
            var ganhoMensal = new GanhoMensalViewModel(listGanho);
            var gastoMensal = new GastoMensalViewModel(listGasto);
            var investimentoMensal = new InvestimentoMensalViewModel(listInvestimento);
            var resgateMensal = new ResgateMensalViewModel(listResgate);
            var telaInicialViewModelR = new TelaInicialViewModel(cliente,ganhoMensal,gastoMensal,investimentoMensal,resgateMensal,4000M);
            var controller = InicializarLoginController();
            _telaInicialApp.TelaInicial(Arg.Any<Guid>()).Returns(telaInicialViewModelR);
            var token = TokenApp.GerarToken(GeraUsuario());

            //Act
            var teste = controller.TelaInicial(token,cliente.Id);
            var okResult = teste.Result as ObjectResult;
            var telaInicialViewModel = Assert.IsType<TelaInicialViewModel>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotEqual(Guid.Empty, telaInicialViewModel.Cliente.Id);
            Assert.Single(telaInicialViewModel.GanhoMensal.Ganhos);
            Assert.Single(telaInicialViewModel.GastoMensal.Gastos);
            Assert.Single(telaInicialViewModel.ResgateMensal.Resgates);
            Assert.Single(telaInicialViewModel.InvestimentoMensal.Investimentos);
            Assert.Equal(5000, telaInicialViewModel.GanhoMensal.SaldoMensal);
            Assert.Equal(1000, telaInicialViewModel.GastoMensal.SaldoMensal);
            Assert.Equal(5000, telaInicialViewModel.ResgateMensal.SaldoMensal);
            Assert.Equal(10000, telaInicialViewModel.InvestimentoMensal.SaldoMensal);
            Assert.Equal(4000, telaInicialViewModel.ResumoMensal);
            //Assert Cliente
            Assert.Equal(cliente.Id, telaInicialViewModel.Cliente.Id);
            Assert.Equal(cliente.Nome, telaInicialViewModel.Cliente.Nome);
            Assert.Equal(cliente.CPF, telaInicialViewModel.Cliente.CPF);
            Assert.Equal(cliente.Email, telaInicialViewModel.Cliente.Email);
            //Assert Ganho
            var ganhoTest = telaInicialViewModel.GanhoMensal.Ganhos.FirstOrDefault(g => g.Id == ganho.Id);
            Assert.Equal(ganho.Id, ganhoTest.Id);
            Assert.Equal(cliente.Id, ganhoTest.IdCliente);
            Assert.Equal(ganho.NomeGanho, ganhoTest.NomeGanho);
            Assert.Equal(ganho.Valor, ganhoTest.Valor);
            Assert.Equal(ganho.Recorrente, ganhoTest.Recorrente);
            Assert.Equal(ganho.DataDoGanho, ganhoTest.DataDoGanho);
            //Assert Gasto
            var gastoTest = telaInicialViewModel.GastoMensal.Gastos.FirstOrDefault(g => g.Id == gasto.Id);
            Assert.Equal(gasto.Id, gastoTest.Id);
            Assert.Equal(cliente.Id, gastoTest.IdCliente);
            Assert.Equal(gasto.NomeGasto, gastoTest.NomeGasto);
            Assert.Equal(gasto.Valor, gastoTest.Valor);
            Assert.Equal(gasto.QuantidadeParcelas, gastoTest.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto, gastoTest.DataDoGasto);
            //Assert Investimento
            var investimentoTest = telaInicialViewModel.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == investimento.Id);
            Assert.Equal(investimento.Id, investimentoTest.Id);
            Assert.Equal(cliente.Id, investimentoTest.IdCliente);
            Assert.Equal(investimento.NomeInvestimento, investimentoTest.NomeInvestimento);
            Assert.Equal(investimento.Valor, investimentoTest.Valor);
            Assert.Equal(investimento.DataAplicacao, investimentoTest.DataAplicacao);
            //Assert Resgate
            var resgateTest = telaInicialViewModel.ResgateMensal.Resgates.FirstOrDefault(g => g.Id == resgate.Id);
            Assert.Equal(resgate.Id, resgateTest.Id);
            Assert.Equal(cliente.Id, resgateTest.IdCliente);
            Assert.Equal(resgate.MotivoResgate, resgateTest.MotivoResgate);
            Assert.Equal(resgate.Valor, resgateTest.Valor);
            Assert.Equal(resgate.DataResgate, resgateTest.DataResgate);
        }
    }
}
