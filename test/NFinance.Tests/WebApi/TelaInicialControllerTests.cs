﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.GanhoViewModel;
using NFinance.Application.ViewModel.GastosViewModel;
using NFinance.Application.ViewModel.InvestimentosViewModel;
using NFinance.Application.ViewModel.ResgatesViewModel;
using NFinance.Application.ViewModel.TelaInicialViewModel;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.WebApi.Controllers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.WebApi
{
    public class TelaInicialControllerTests
    {
        private readonly ITelaInicialApp _telaInicialApp;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<TelaInicialController> _logger;

        public TelaInicialControllerTests()
        {
            _telaInicialApp = Substitute.For<ITelaInicialApp>();
            _autenticacaoService = Substitute.For<IAutenticacaoService>();
            _logger = Substitute.For<ILogger<TelaInicialController>>();
        }

        private TelaInicialController InicializarLoginController()
        {
            return new TelaInicialController(_logger, _telaInicialApp,_autenticacaoService);
        }

        [Fact]
        public async Task LoginController_ExibirDados_ComSucesso()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idGasto = Guid.NewGuid();
            var idResgate = Guid.NewGuid();
            var idGanho = Guid.NewGuid();
            var nomeCliente = "Teste@Sucesso";
            var cpfCliente = "123.654.987-96";
            var emailCliente = "teste@teste.com";
            var valorGanho = 5000M;
            var dataGanho = DateTime.Today;
            var nomeGanho = "Salario";
            var recorrente = false;
            var valorGasto = 1000M;
            var dataGasto = DateTime.Today.AddDays(3);
            var nomeGasto = "CAixas";
            var qtdParcelas = 3;
            var valorInvestimento = 10000M;
            var nomeInvestimento = "CDB";
            var dataAplicacao = DateTime.Today;
            var dataResgate = DateTime.Today.AddDays(-2);
            var motivoResgate = "Necessidade";
            var ganho = new GanhoViewModel { Id = idGanho, IdCliente = idCliente, NomeGanho = nomeGanho, Valor = valorGanho, DataDoGanho = dataGanho, Recorrente = recorrente};
            var gasto = new GastoViewModel { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var investimento = new InvestimentoViewModel { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataAplicacao };
            var resgate = new ResgateViewModel { Id = idResgate, IdCliente = idCliente, Investimento = investimento, Valor = valorInvestimento, DataResgate = dataResgate, MotivoResgate = motivoResgate };
            var listGanho = new List<GanhoViewModel>();
            var listGasto = new List<GastoViewModel>();
            var listInvestimento = new List<InvestimentoViewModel>();
            var listResgate = new List<ResgateViewModel>();
            listGanho.Add(ganho);
            listGasto.Add(gasto);
            listInvestimento.Add(investimento);
            listResgate.Add(resgate);
            var cliente = new Cliente(nomeCliente, cpfCliente, emailCliente , "aaaaaa");
            var ganhoMensal = new GanhoMensalViewModel(listGanho);
            var gastoMensal = new GastoMensalViewModel(listGasto);
            var investimentoMensal = new InvestimentoMensalViewModel(listInvestimento);
            var resgateMensal = new ResgateMensalViewModel(listResgate);
            var telaInicialViewModelR = new TelaInicialViewModel(cliente,ganhoMensal,gastoMensal,investimentoMensal,resgateMensal,4000M);
            var controller = InicializarLoginController();
            _telaInicialApp.TelaInicial(Arg.Any<Guid>()).Returns(telaInicialViewModelR);
            var token = TokenService.GerarToken(cliente);

            //Act
            var teste = controller.TelaInicial(token,idCliente);
            var okResult = teste.Result as ObjectResult;
            var telaInicialViewModel = Assert.IsType<TelaInicialViewModel>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotEqual(Guid.Empty, telaInicialViewModel.Cliente.Id);
            Assert.Single(telaInicialViewModel.GanhoMensal.Ganhos);
            Assert.NotEmpty(telaInicialViewModel.GastoMensal.Gastos);
            Assert.NotEmpty(telaInicialViewModel.ResgateMensal.Resgates);
            Assert.NotEmpty(telaInicialViewModel.InvestimentoMensal.Investimentos);
            Assert.Equal(5000, telaInicialViewModel.GanhoMensal.SaldoMensal);
            Assert.Equal(1000, telaInicialViewModel.GastoMensal.SaldoMensal);
            Assert.Equal(10000, telaInicialViewModel.ResgateMensal.SaldoMensal);
            Assert.Equal(10000, telaInicialViewModel.InvestimentoMensal.SaldoMensal);
            Assert.Equal(4000, telaInicialViewModel.ResumoMensal);
            //Assert Cliente
            Assert.Equal(idCliente, telaInicialViewModel.Cliente.Id);
            Assert.Equal(nomeCliente, telaInicialViewModel.Cliente.Nome);
            Assert.Equal(cpfCliente, telaInicialViewModel.Cliente.CPF);
            Assert.Equal(emailCliente, telaInicialViewModel.Cliente.Email);
            //Assert Ganho
            var ganhoTest = telaInicialViewModel.GanhoMensal.Ganhos.FirstOrDefault(g => g.Id == idGanho);
            Assert.Equal(idGanho, ganhoTest.Id);
            Assert.Equal(idCliente, ganhoTest.IdCliente);
            Assert.Equal(nomeGanho, ganhoTest.NomeGanho);
            Assert.Equal(valorGanho, ganhoTest.Valor);
            Assert.Equal(recorrente, ganhoTest.Recorrente);
            Assert.Equal(dataGanho, ganhoTest.DataDoGanho);
            //Assert Gasto
            var gastoTest = telaInicialViewModel.GastoMensal.Gastos.FirstOrDefault(g => g.Id == idGasto);
            Assert.Equal(idGasto, gastoTest.Id);
            Assert.Equal(idCliente, gastoTest.IdCliente);
            Assert.Equal(nomeGasto, gastoTest.NomeGasto);
            Assert.Equal(valorGasto, gastoTest.Valor);
            Assert.Equal(qtdParcelas, gastoTest.QuantidadeParcelas);
            Assert.Equal(dataGasto, gastoTest.DataDoGasto);
            //Assert Investimento
            var investimentoTest = telaInicialViewModel.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == idInvestimento);
            Assert.Equal(idInvestimento, investimentoTest.Id);
            Assert.Equal(idCliente, investimentoTest.IdCliente);
            Assert.Equal(nomeInvestimento, investimentoTest.NomeInvestimento);
            Assert.Equal(valorInvestimento, investimentoTest.Valor);
            Assert.Equal(dataAplicacao, investimentoTest.DataAplicacao);
            //Assert Resgate
            var resgateTest = telaInicialViewModel.ResgateMensal.Resgates.FirstOrDefault(g => g.Id == idResgate);
            Assert.Equal(idResgate, resgateTest.Id);
            Assert.Equal(idCliente, resgateTest.IdCliente);
            Assert.Equal(motivoResgate, resgateTest.MotivoResgate);
            Assert.Equal(valorInvestimento, resgateTest.Valor);
            Assert.Equal(dataResgate, resgateTest.DataResgate);
        }
    }
}
