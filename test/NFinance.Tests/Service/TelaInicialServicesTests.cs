using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.Domain.ViewModel.ClientesViewModel;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.Service
{
    public class TelaInicialServicesTests
    {
        private readonly IClienteService _clienteService;
        private readonly IGanhoRepository _ganhoRepository;
        private readonly IGastoRepository _gastoRepository;
        private readonly IInvestimentoRepository _investimentoRepository;
        private readonly IResgateRepository _resgateRepository;

        public TelaInicialServicesTests()
        {
            _clienteService = Substitute.For<IClienteService>();
            _gastoRepository = Substitute.For<IGastoRepository>();
            _ganhoRepository = Substitute.For<IGanhoRepository>();
            _investimentoRepository = Substitute.For<IInvestimentoRepository>();
            _resgateRepository = Substitute.For<IResgateRepository>();
        }

        public TelaInicialService InicializaServico()
        {
            return new TelaInicialService(_ganhoRepository, _investimentoRepository, _gastoRepository, _resgateRepository, _clienteService);
        }

        [Fact]
        public async Task TelaInicialService_TelaInicial_ComSucesso()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var idGasto = Guid.NewGuid();
            var idGasto1 = Guid.NewGuid();
            var idGanho1 = Guid.NewGuid();
            var idGanho2 = Guid.NewGuid();
            var idResgate = Guid.NewGuid();
            var idGanho = Guid.NewGuid();
            var nomeCliente = "Teste@Sucesso";
            var cpfCliente = "123.654.987-96";
            var emailCliente = "teste@teste.com";
            var valorGanho = 5000M;
            var dataGanho = DateTime.Today;
            var dataGanho1 = DateTime.Today.AddMonths(3);
            var dataGanho2 = DateTime.Today.AddMonths(-3);
            var nomeGanho = "Salario";
            var recorrente = false;
            var recorrente1 = true;
            var valorGasto = 1000M;
            var dataGasto = DateTime.Today.AddDays(3);
            var dataGasto1 = DateTime.Today.AddDays(5);
            var nomeGasto = "CAixas";
            var qtdParcelas = 3;
            var qtdParcelas1 = 6;
            var valorInvestimento = 10000M;
            var nomeInvestimento = "CDB";
            var dataAplicacao = DateTime.Today;
            var dataAplicacao1 = DateTime.Today.AddDays(-1);
            var dataResgate = DateTime.Today.AddDays(-1);
            var motivoResgate = "Necessidade";
            var listInvestimento = new List<Investimento>();
            var listResgate = new List<Resgate>();
            var listGanho = new List<Ganho>();
            var listGasto = new List<Gasto>();
            var ganho = new Ganho { Id = idGanho, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho, Recorrente = recorrente };
            var ganho1 = new Ganho { Id = idGanho1, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho1, Recorrente = recorrente1 };
            var ganho2 = new Ganho { Id = idGanho2, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho2, Recorrente = recorrente };
            var gasto = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var gasto1 = new Gasto { Id = idGasto1, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto1, QuantidadeParcelas = qtdParcelas1 };
            var investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataAplicacao };
            var investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataAplicacao1 };
            var resgate = new Resgate { Id = idResgate, IdCliente = idCliente, IdInvestimento = idInvestimento, Valor = valorInvestimento, DataResgate = dataResgate, MotivoResgate = motivoResgate };
            var clienteResponse = new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente, Cpf = cpfCliente, Email = emailCliente };
            listGanho.Add(ganho);
            listGanho.Add(ganho1);
            listGanho.Add(ganho2);
            listGasto.Add(gasto);
            listGasto.Add(gasto1);
            listInvestimento.Add(investimento);
            listInvestimento.Add(investimento1);
            listResgate.Add(resgate);
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            _gastoRepository.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            _resgateRepository.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(clienteResponse);
            _investimentoRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaServico();

            //Act
            var response = await services.TelaInicial(idCliente);

            //Assert
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.Cliente.Id);
            Assert.Single(response.GanhoMensal.Ganhos);
            Assert.NotEmpty(response.GastoMensal.Gastos);
            Assert.NotEmpty(response.ResgateMensal.Resgates);
            Assert.NotEmpty(response.InvestimentoMensal.Investimentos);
            Assert.Equal(5000, response.GanhoMensal.SaldoMensal);
            Assert.Equal(500, response.GastoMensal.SaldoMensal);
            Assert.Equal(10000, response.ResgateMensal.SaldoMensal);
            Assert.Equal(20000, response.InvestimentoMensal.SaldoMensal);
            Assert.Equal(4500, response.ResumoMensal);
            //Assert Cliente
            Assert.Equal(idCliente, response.Cliente.Id);
            Assert.Equal(nomeCliente, response.Cliente.Nome);
            Assert.Equal(cpfCliente, response.Cliente.Cpf);
            Assert.Equal(emailCliente, response.Cliente.Email);
            //Assert Ganho
            var ganhoTest = response.GanhoMensal.Ganhos.FirstOrDefault(g => g.Id == idGanho);
            Assert.Equal(idGanho, ganhoTest.Id);
            Assert.Equal(idCliente, ganhoTest.IdCliente);
            Assert.Equal(nomeGanho, ganhoTest.NomeGanho);
            Assert.Equal(valorGanho, ganhoTest.Valor);
            Assert.Equal(recorrente, ganhoTest.Recorrente);
            Assert.Equal(dataGanho, ganhoTest.DataDoGanho);
            //Assert Gasto
            var gastoTest = response.GastoMensal.Gastos.FirstOrDefault(g => g.Id == idGasto);
            Assert.Equal(idGasto, gastoTest.Id);
            Assert.Equal(idCliente, gastoTest.IdCliente);
            Assert.Equal(nomeGasto, gastoTest.NomeGasto);
            Assert.Equal(333.33333333333333333333333333M, gastoTest.Valor);
            Assert.Equal(qtdParcelas, gastoTest.QuantidadeParcelas);
            Assert.Equal(dataGasto, gastoTest.DataDoGasto);
            //Assert Gasto 1
            var gastoTest1 = response.GastoMensal.Gastos.FirstOrDefault(g => g.Id == idGasto1);
            Assert.Equal(idGasto1, gastoTest1.Id);
            Assert.Equal(idCliente, gastoTest1.IdCliente);
            Assert.Equal(nomeGasto, gastoTest1.NomeGasto);
            Assert.Equal(166.66666666666666666666666667M, gastoTest1.Valor);
            Assert.Equal(qtdParcelas1, gastoTest1.QuantidadeParcelas);
            Assert.Equal(dataGasto1, gastoTest1.DataDoGasto);
            //Assert Investimento
            var investimentoTest = response.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == idInvestimento);
            Assert.Equal(idInvestimento, investimentoTest.Id);
            Assert.Equal(idCliente, investimentoTest.IdCliente);
            Assert.Equal(nomeInvestimento, investimentoTest.NomeInvestimento);
            Assert.Equal(valorInvestimento, investimentoTest.Valor);
            Assert.Equal(dataAplicacao, investimentoTest.DataAplicacao);
            //Assert Investimento 1
            var investimentoTest1 = response.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == idInvestimento1);
            Assert.Equal(idInvestimento1, investimentoTest1.Id);
            Assert.Equal(idCliente, investimentoTest1.IdCliente);
            Assert.Equal(nomeInvestimento, investimentoTest1.NomeInvestimento);
            Assert.Equal(valorInvestimento, investimentoTest1.Valor);
            Assert.Equal(dataAplicacao1, investimentoTest1.DataAplicacao);
            //Assert Resgate
            var resgateTest = response.ResgateMensal.Resgates.FirstOrDefault(g => g.Id == idResgate);
            Assert.Equal(idResgate, resgateTest.Id);
            Assert.Equal(idCliente, resgateTest.IdCliente);
            Assert.Equal(motivoResgate, resgateTest.MotivoResgate);
            Assert.Equal(valorInvestimento, resgateTest.Valor);
            Assert.Equal(dataResgate, resgateTest.DataResgate);
        }

        [Fact]
        public async Task TelaInicialService_TelaInicial_ComIdCliente_Incorreto()
        {
            //Arrange
            var idCliente = Guid.Empty;
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var idGasto = Guid.NewGuid();
            var idGasto1 = Guid.NewGuid();
            var idGanho1 = Guid.NewGuid();
            var idGanho2 = Guid.NewGuid();
            var idResgate = Guid.NewGuid();
            var idGanho = Guid.NewGuid();
            var nomeCliente = "Teste@Sucesso";
            var cpfCliente = "123.654.987-96";
            var emailCliente = "teste@teste.com";
            var valorGanho = 5000M;
            var dataGanho = DateTime.Today;
            var dataGanho1 = DateTime.Today.AddMonths(3);
            var dataGanho2 = DateTime.Today.AddMonths(-3);
            var nomeGanho = "Salario";
            var recorrente = false;
            var recorrente1 = true;
            var valorGasto = 1000M;
            var dataGasto = DateTime.Today.AddDays(3);
            var dataGasto1 = DateTime.Today.AddDays(5);
            var nomeGasto = "CAixas";
            var qtdParcelas = 3;
            var qtdParcelas1 = 6;
            var valorInvestimento = 10000M;
            var nomeInvestimento = "CDB";
            var dataAplicacao = DateTime.Today;
            var dataAplicacao1 = DateTime.Today.AddDays(-1);
            var dataResgate = DateTime.Today.AddDays(-2);
            var motivoResgate = "Necessidade";
            var listInvestimento = new List<Investimento>();
            var listResgate = new List<Resgate>();
            var listGanho = new List<Ganho>();
            var listGasto = new List<Gasto>();
            var ganho = new Ganho { Id = idGanho, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho, Recorrente = recorrente };
            var ganho1 = new Ganho { Id = idGanho1, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho1, Recorrente = recorrente1 };
            var ganho2 = new Ganho { Id = idGanho2, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho2, Recorrente = recorrente };
            var gasto = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var gasto1 = new Gasto { Id = idGasto1, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto1, QuantidadeParcelas = qtdParcelas1 };
            var investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataAplicacao };
            var investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataAplicacao1 };
            var resgate = new Resgate { Id = idResgate, IdCliente = idCliente, IdInvestimento = idInvestimento, Valor = valorInvestimento, DataResgate = dataResgate, MotivoResgate = motivoResgate };
            var clienteResponse = new ConsultarClienteViewModel.Response { Id = idCliente, Nome = nomeCliente, Cpf = cpfCliente, Email = emailCliente };
            listGanho.Add(ganho);
            listGanho.Add(ganho1);
            listGanho.Add(ganho2);
            listGasto.Add(gasto);
            listGasto.Add(gasto1);
            listInvestimento.Add(investimento);
            listInvestimento.Add(investimento1);
            listResgate.Add(resgate);
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            _gastoRepository.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            _resgateRepository.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(clienteResponse);
            _investimentoRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaServico();

            //Act
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.TelaInicial(idCliente));
        }

        [Fact]
        public async Task TelaInicialService_GanhoMensal_ComSucesso()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idGanho = Guid.NewGuid();
            var idGanho1 = Guid.NewGuid();
            var valorGanho = 6000M;
            var valorGanho1 = 3000M;
            var nomeGanho = "Ganho";
            var nomeGanho1 = "Ganho1";
            var dataGanho = DateTime.Today;
            var dataGanho1 = DateTime.Today.AddDays(5);
            var recorrente = false;
            var recorrente1 = true;
            var ganho = new Ganho { Id = idGanho, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho, Recorrente = recorrente };
            var ganho1 = new Ganho { Id = idGanho1, IdCliente = idCliente, Valor = valorGanho1, NomeGanho = nomeGanho1, DataDoGanho = dataGanho1, Recorrente = recorrente1 };
            var listGanho = new List<Ganho>();
            listGanho.Add(ganho);
            listGanho.Add(ganho1);
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();
            
            //Act
            var response = await services.GanhoMensal(idCliente);

            //Assert
            Assert.Equal(9000,response.SaldoMensal);
            //Ganho
            var ganhoTest = response.Ganhos.FirstOrDefault(g => g.Id == idGanho);
            Assert.Equal(idGanho, ganhoTest.Id);
            Assert.Equal(idCliente, ganhoTest.IdCliente);
            Assert.Equal(nomeGanho, ganhoTest.NomeGanho);
            Assert.Equal(valorGanho, ganhoTest.Valor);
            Assert.Equal(recorrente, ganhoTest.Recorrente);
            Assert.Equal(dataGanho, ganhoTest.DataDoGanho);
            //Ganho 1
            var ganhoTest1 = response.Ganhos.FirstOrDefault(g => g.Id == idGanho1);
            Assert.Equal(idGanho1, ganhoTest1.Id);
            Assert.Equal(idCliente, ganhoTest1.IdCliente);
            Assert.Equal(nomeGanho1, ganhoTest1.NomeGanho);
            Assert.Equal(valorGanho1, ganhoTest1.Valor);
            Assert.Equal(recorrente1, ganhoTest1.Recorrente);
            Assert.Equal(dataGanho1, ganhoTest1.DataDoGanho);
        }

        [Fact]
        public async Task TelaInicialService_GanhoMensal_ComGanhos_Futuros()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idGanho = Guid.NewGuid();
            var idGanho1 = Guid.NewGuid();
            var valorGanho = 6000M;
            var valorGanho1 = 3000M;
            var nomeGanho = "Ganho";
            var nomeGanho1 = "Ganho1";
            var dataGanho = DateTime.Today.AddMonths(5);
            var dataGanho1 = DateTime.Today.AddMonths(3);
            var recorrente = false;
            var recorrente1 = true;
            var ganho = new Ganho { Id = idGanho, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho, Recorrente = recorrente };
            var ganho1 = new Ganho { Id = idGanho1, IdCliente = idCliente, Valor = valorGanho1, NomeGanho = nomeGanho1, DataDoGanho = dataGanho1, Recorrente = recorrente1 };
            var listGanho = new List<Ganho>();
            listGanho.Add(ganho);
            listGanho.Add(ganho1);
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();

            //Act
            var response = await services.GanhoMensal(idCliente);

            //Assert
            Assert.Empty(response.Ganhos);
            Assert.Equal(0, response.SaldoMensal);
        }

        [Fact]
        public async Task TelaInicialService_GanhoMensal_ComGanhos_Passados()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idGanho = Guid.NewGuid();
            var idGanho1 = Guid.NewGuid();
            var valorGanho = 6000M;
            var valorGanho1 = 3000M;
            var nomeGanho = "Ganho";
            var nomeGanho1 = "Ganho1";
            var dataGanho = DateTime.Today.AddMonths(-5);
            var dataGanho1 = DateTime.Today.AddMonths(-3);
            var recorrente = false;
            var recorrente1 = true;
            var ganho = new Ganho { Id = idGanho, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho, Recorrente = recorrente };
            var ganho1 = new Ganho { Id = idGanho1, IdCliente = idCliente, Valor = valorGanho1, NomeGanho = nomeGanho1, DataDoGanho = dataGanho1, Recorrente = recorrente1 };
            var listGanho = new List<Ganho>();
            listGanho.Add(ganho);
            listGanho.Add(ganho1);
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();

            //Act
            var response = await services.GanhoMensal(idCliente);

            //Assert
            Assert.Empty(response.Ganhos);
            Assert.Equal(0, response.SaldoMensal);
        }

        [Fact]
        public async Task TelaInicialService_GanhoMensal_Com_GanhoFuturoRecorrente_E_GanhoNaoRecorrente()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idGanho = Guid.NewGuid();
            var idGanho1 = Guid.NewGuid();
            var valorGanho = 6000M;
            var valorGanho1 = 3000M;
            var nomeGanho = "Ganho";
            var nomeGanho1 = "Ganho1";
            var dataGanho = DateTime.Today;
            var dataGanho1 = DateTime.Today.AddMonths(5);
            var recorrente = false;
            var recorrente1 = true;
            var ganho = new Ganho { Id = idGanho, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho, Recorrente = recorrente };
            var ganho1 = new Ganho { Id = idGanho1, IdCliente = idCliente, Valor = valorGanho1, NomeGanho = nomeGanho1, DataDoGanho = dataGanho1, Recorrente = recorrente1 };
            var listGanho = new List<Ganho>();
            listGanho.Add(ganho);
            listGanho.Add(ganho1);
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();

            //Act
            var response = await services.GanhoMensal(idCliente);

            //Assert
            Assert.Equal(6000, response.SaldoMensal);
            //Ganho
            var ganhoTest = response.Ganhos.FirstOrDefault(g => g.Id == idGanho);
            Assert.Equal(idGanho, ganhoTest.Id);
            Assert.Equal(idCliente, ganhoTest.IdCliente);
            Assert.Equal(nomeGanho, ganhoTest.NomeGanho);
            Assert.Equal(valorGanho, ganhoTest.Valor);
            Assert.Equal(recorrente, ganhoTest.Recorrente);
            Assert.Equal(dataGanho, ganhoTest.DataDoGanho);
        }

        [Fact]
        public async Task TelaInicialService_GastoMensal_ComSucesso()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idGasto = Guid.NewGuid();
            var idGasto1 = Guid.NewGuid();
            var valorGasto = 6000M;
            var valorGasto1 = 3000M;
            var nomeGasto = "Gasto";
            var nomeGasto1 = "Gasto1";
            var dataGasto1 = DateTime.Today.AddDays(5);
            var dataGasto = DateTime.Today.AddDays(10);
            var qtdParcelas = 0;
            var gasto = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var gasto1 = new Gasto { Id = idGasto1, IdCliente = idCliente, NomeGasto = nomeGasto1, Valor = valorGasto1, DataDoGasto = dataGasto1, QuantidadeParcelas = qtdParcelas };
            var listGasto = new List<Gasto>();
            listGasto.Add(gasto);
            listGasto.Add(gasto1);
            _gastoRepository.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            var services = InicializaServico();

            //Act
            var response = await services.GastoMensal(idCliente);

            //Assert
            Assert.Equal(2,response.Gastos.Count);
            Assert.Equal(9000, response.SaldoMensal);
            //Assert Gasto
            var gastoTest = response.Gastos.FirstOrDefault(g => g.Id == idGasto);
            Assert.Equal(idGasto, gastoTest.Id);
            Assert.Equal(idCliente, gastoTest.IdCliente);
            Assert.Equal(nomeGasto, gastoTest.NomeGasto);
            Assert.Equal(valorGasto, gastoTest.Valor);
            Assert.Equal(qtdParcelas, gastoTest.QuantidadeParcelas);
            Assert.Equal(dataGasto, gastoTest.DataDoGasto);
            //Assert Gasto
            var gastoTest1 = response.Gastos.FirstOrDefault(g => g.Id == idGasto1);
            Assert.Equal(idGasto1, gastoTest1.Id);
            Assert.Equal(idCliente, gastoTest1.IdCliente);
            Assert.Equal(nomeGasto1, gastoTest1.NomeGasto);
            Assert.Equal(valorGasto1, gastoTest1.Valor);
            Assert.Equal(qtdParcelas, gastoTest1.QuantidadeParcelas);
            Assert.Equal(dataGasto1, gastoTest1.DataDoGasto);
        }

        [Fact]
        public async Task TelaInicialService_GastoMensal_Com_DataGasto_Futura()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idGasto = Guid.NewGuid();
            var idGasto1 = Guid.NewGuid();
            var valorGasto = 6000M;
            var valorGasto1 = 3000M;
            var nomeGasto = "Gasto";
            var nomeGasto1 = "Gasto1";
            var dataGasto1 = DateTime.Today.AddMonths(5);
            var dataGasto = DateTime.Today.AddMonths(10);
            var qtdParcelas = 0;
            var gasto = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var gasto1 = new Gasto { Id = idGasto1, IdCliente = idCliente, NomeGasto = nomeGasto1, Valor = valorGasto1, DataDoGasto = dataGasto1, QuantidadeParcelas = qtdParcelas };
            var listGasto = new List<Gasto>();
            listGasto.Add(gasto);
            listGasto.Add(gasto1);
            _gastoRepository.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            var services = InicializaServico();

            //Act
            var response = await services.GastoMensal(idCliente);

            //Assert
            Assert.Empty(response.Gastos);
            Assert.Equal(0,response.SaldoMensal);
        }


        [Fact]
        public async Task TelaInicialService_GastoMensal_Com_DataGasto_Futura_E_QuantidadeParcelas6()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idGasto = Guid.NewGuid();
            var idGasto1 = Guid.NewGuid();
            var valorGasto = 6000M;
            var valorGasto1 = 3000M;
            var nomeGasto = "Gasto";
            var nomeGasto1 = "Gasto1";
            var dataGasto1 = DateTime.Today.AddMonths(5);
            var dataGasto = DateTime.Today.AddMonths(9);
            var qtdParcelas = 6;
            var qtdParcelas1 = 0;
            var gasto = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var gasto1 = new Gasto { Id = idGasto1, IdCliente = idCliente, NomeGasto = nomeGasto1, Valor = valorGasto1, DataDoGasto = dataGasto1, QuantidadeParcelas = qtdParcelas1 };
            var listGasto = new List<Gasto>();
            listGasto.Add(gasto);
            listGasto.Add(gasto1);
            _gastoRepository.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            var services = InicializaServico();

            //Act
            var response = await services.GastoMensal(idCliente);

            //Assert
            Assert.Empty(response.Gastos);
            Assert.Equal(0, response.SaldoMensal);
        }

        [Fact]
        public async Task TelaInicialService_GastoMensal_Com_DataGasto_Passado_E_DataAtual_Com_QuantidadeParcelas6()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idGasto = Guid.NewGuid();
            var idGasto1 = Guid.NewGuid();
            var valorGasto = 6000M;
            var valorGasto1 = 3000M;
            var nomeGasto = "Gasto";
            var nomeGasto1 = "Gasto1";
            var dataGasto1 = DateTime.Today.AddMonths(-5);
            var dataGasto = DateTime.Today;
            var qtdParcelas = 6;
            var qtdParcelas1 = 0;
            var gasto = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var gasto1 = new Gasto { Id = idGasto1, IdCliente = idCliente, NomeGasto = nomeGasto1, Valor = valorGasto1, DataDoGasto = dataGasto1, QuantidadeParcelas = qtdParcelas1 };
            var gastoResponse = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = 1000, DataDoGasto = dataGasto, QuantidadeParcelas = 5 };
            var listGasto = new List<Gasto>();
            listGasto.Add(gasto);
            listGasto.Add(gasto1);
            _gastoRepository.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            _gastoRepository.AtualizarGasto(Arg.Any<Guid>(),Arg.Any<Gasto>()).Returns(gastoResponse);
            var services = InicializaServico();

            //Act
            var response = await services.GastoMensal(idCliente);

            //Assert
            Assert.Single(response.Gastos);
            Assert.Equal(1000, response.SaldoMensal);
            //Assert Gasto
            var gastoTest = response.Gastos.FirstOrDefault(g => g.Id == idGasto);
            Assert.Equal(idGasto, gastoTest.Id);
            Assert.Equal(idCliente, gastoTest.IdCliente);
            Assert.Equal(nomeGasto, gastoTest.NomeGasto);
            Assert.Equal(1000, gastoTest.Valor);
            Assert.Equal(qtdParcelas, gastoTest.QuantidadeParcelas);
            Assert.Equal(dataGasto, gastoTest.DataDoGasto);
        }

        [Fact]
        public async Task TelaInicialService_InvestimentoMensal_ComSucesso()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var valorInvestimento = 6000M;
            var valorInvestimento1 = 3000M;
            var nomeInvestimento = "Investimento";
            var nomeInvestimento1 = "Investimento1";
            var dataInvestimento1 = DateTime.Today.AddDays(4);
            var dataInvestimento = DateTime.Today;
            var Investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataInvestimento };
            var Investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento1, Valor = valorInvestimento1, DataAplicacao = dataInvestimento1 };
            var listInvestimento = new List<Investimento>();
            listInvestimento.Add(Investimento);
            listInvestimento.Add(Investimento1);
            _investimentoRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaServico();

            //Act
            var response = await services.InvestimentoMensal(idCliente);

            //Assert
            Assert.Equal(2,response.Investimentos.Count);
            Assert.Equal(9000, response.SaldoMensal);
            //Assert Investimento
            var investimentoTest = response.Investimentos.FirstOrDefault(g => g.Id == idInvestimento);
            Assert.Equal(idInvestimento, investimentoTest.Id);
            Assert.Equal(idCliente, investimentoTest.IdCliente);
            Assert.Equal(nomeInvestimento, investimentoTest.NomeInvestimento);
            Assert.Equal(valorInvestimento, investimentoTest.Valor);
            Assert.Equal(dataInvestimento, investimentoTest.DataAplicacao);
            //Assert Investimento 1
            var investimentoTest1 = response.Investimentos.FirstOrDefault(g => g.Id == idInvestimento1);
            Assert.Equal(idInvestimento1, investimentoTest1.Id);
            Assert.Equal(idCliente, investimentoTest1.IdCliente);
            Assert.Equal(nomeInvestimento1, investimentoTest1.NomeInvestimento);
            Assert.Equal(valorInvestimento1, investimentoTest1.Valor);
            Assert.Equal(dataInvestimento1, investimentoTest1.DataAplicacao);
        }

        [Fact]
        public async Task TelaInicialService_InvestimentoMensal_Com_DataInferior_MesAtual()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var valorInvestimento = 6000M;
            var valorInvestimento1 = 3000M;
            var nomeInvestimento = "Investimento";
            var nomeInvestimento1 = "Investimento1";
            var dataInvestimento1 = DateTime.Today.AddMonths(-4);
            var dataInvestimento = DateTime.Today.AddMonths(-2);
            var Investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataInvestimento };
            var Investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento1, Valor = valorInvestimento1, DataAplicacao = dataInvestimento1 };
            var listInvestimento = new List<Investimento>();
            listInvestimento.Add(Investimento);
            listInvestimento.Add(Investimento1);
            _investimentoRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaServico();

            //Act
            var response = await services.InvestimentoMensal(idCliente);

            //Assert
            Assert.Empty(response.Investimentos);
            Assert.Equal(0, response.SaldoMensal);
        }

        [Fact]
        public async Task TelaInicialService_InvestimentoMensal_Com_DataSuperior_MesAtual()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var valorInvestimento = 6000M;
            var valorInvestimento1 = 3000M;
            var nomeInvestimento = "Investimento";
            var nomeInvestimento1 = "Investimento1";
            var dataInvestimento1 = DateTime.Today.AddMonths(4);
            var dataInvestimento = DateTime.Today.AddMonths(2);
            var Investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataInvestimento };
            var Investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento1, Valor = valorInvestimento1, DataAplicacao = dataInvestimento1 };
            var listInvestimento = new List<Investimento>();
            listInvestimento.Add(Investimento);
            listInvestimento.Add(Investimento1);
            _investimentoRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaServico();

            //Act
            var response = await services.InvestimentoMensal(idCliente);

            //Assert
            Assert.Empty(response.Investimentos);
            Assert.Equal(0, response.SaldoMensal);
        }

        [Fact]
        public async Task TelaInicialService_InvestimentoMensal_Com_DataInferior_E_DataSuperior_MesAtual()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var valorInvestimento = 6000M;
            var valorInvestimento1 = 3000M;
            var nomeInvestimento = "Investimento";
            var nomeInvestimento1 = "Investimento1";
            var dataInvestimento1 = DateTime.Today.AddMonths(4);
            var dataInvestimento = DateTime.Today.AddMonths(-2);
            var Investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataInvestimento };
            var Investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento1, Valor = valorInvestimento1, DataAplicacao = dataInvestimento1 };
            var listInvestimento = new List<Investimento>();
            listInvestimento.Add(Investimento);
            listInvestimento.Add(Investimento1);
            _investimentoRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaServico();

            //Act
            var response = await services.InvestimentoMensal(idCliente);

            //Assert
            Assert.Empty(response.Investimentos);
            Assert.Equal(0, response.SaldoMensal);
        }

        [Fact]
        public async Task TelaInicialService_InvestimentoMensal_Com_DataInferior_DataAtual_DataSuperior()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var idInvestimento2 = Guid.NewGuid();
            var valorInvestimento = 6000M;
            var valorInvestimento1 = 3000M;
            var valorInvestimento2 = 4000M;
            var nomeInvestimento = "Investimento";
            var nomeInvestimento1 = "Investimento1";
            var nomeInvestimento2 = "Investimento2";
            var dataInvestimento = DateTime.Today.AddMonths(-4);
            var dataInvestimento1 = DateTime.Today.AddMonths(4);
            var dataInvestimento2 = DateTime.Today.AddDays(4);
            var Investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataInvestimento };
            var Investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento1, Valor = valorInvestimento1, DataAplicacao = dataInvestimento1 };
            var Investimento2 = new Investimento { Id = idInvestimento2, IdCliente = idCliente, NomeInvestimento = nomeInvestimento2, Valor = valorInvestimento2, DataAplicacao = dataInvestimento2 };
            var listInvestimento = new List<Investimento>();
            listInvestimento.Add(Investimento);
            listInvestimento.Add(Investimento1);
            listInvestimento.Add(Investimento2);
            _investimentoRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaServico();

            //Act
            var response = await services.InvestimentoMensal(idCliente);

            //Assert
            Assert.Single(response.Investimentos);
            Assert.Equal(4000, response.SaldoMensal);
            //Assert Investimento
            var investimentoTest = response.Investimentos.FirstOrDefault(g => g.Id == idInvestimento2);
            Assert.Equal(idInvestimento2, investimentoTest.Id);
            Assert.Equal(idCliente, investimentoTest.IdCliente);
            Assert.Equal(nomeInvestimento2, investimentoTest.NomeInvestimento);
            Assert.Equal(valorInvestimento2, investimentoTest.Valor);
            Assert.Equal(dataInvestimento2, investimentoTest.DataAplicacao);
        }

        [Fact]
        public async Task TelaInicialService_ResgateMensal_ComSucesso()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idResgate = Guid.NewGuid();
            var idResgate1 = Guid.NewGuid();
            var valorResgate = 6000M;
            var valorResgate1 = 3000M;
            var nomeResgate = "Resgate";
            var nomeResgate1 = "Resgate1";
            var dataResgate = DateTime.Today;
            var dataResgate1 = DateTime.Today;
            var Resgate = new Resgate { Id = idResgate, IdCliente = idCliente, MotivoResgate = nomeResgate, Valor = valorResgate, DataResgate = dataResgate };
            var Resgate1 = new Resgate { Id = idResgate1, IdCliente = idCliente, MotivoResgate = nomeResgate1, Valor = valorResgate1, DataResgate = dataResgate1 };
            var listResgate = new List<Resgate>();
            listResgate.Add(Resgate);
            listResgate.Add(Resgate1);
            _resgateRepository.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            var services = InicializaServico();

            //Act
            var response = await services.ResgateMensal(idCliente);

            //Assert
            Assert.Equal(2, response.Resgates.Count);
            Assert.Equal(9000, response.SaldoMensal);
            //Assert Resgate
            var ResgateTest = response.Resgates.FirstOrDefault(g => g.Id == idResgate);
            Assert.Equal(idResgate, ResgateTest.Id);
            Assert.Equal(idCliente, ResgateTest.IdCliente);
            Assert.Equal(nomeResgate, ResgateTest.MotivoResgate);
            Assert.Equal(valorResgate, ResgateTest.Valor);
            Assert.Equal(dataResgate, ResgateTest.DataResgate);
            //Assert Resgate 1
            var ResgateTest1 = response.Resgates.FirstOrDefault(g => g.Id == idResgate1);
            Assert.Equal(idResgate1, ResgateTest1.Id);
            Assert.Equal(idCliente, ResgateTest1.IdCliente);
            Assert.Equal(nomeResgate1, ResgateTest1.MotivoResgate);
            Assert.Equal(valorResgate1, ResgateTest1.Valor);
            Assert.Equal(dataResgate1, ResgateTest1.DataResgate);
        }

        [Fact]
        public async Task TelaInicialService_ResgateMensal_Com_DataAnterior_E_DataAtual()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idResgate = Guid.NewGuid();
            var idResgate1 = Guid.NewGuid();
            var valorResgate = 6000M;
            var valorResgate1 = 3000M;
            var nomeResgate = "Resgate";
            var nomeResgate1 = "Resgate1";
            var dataResgate = DateTime.Today.AddMonths(-5);
            var dataResgate1 = DateTime.Today;
            var Resgate = new Resgate { Id = idResgate, IdCliente = idCliente, MotivoResgate = nomeResgate, Valor = valorResgate, DataResgate = dataResgate };
            var Resgate1 = new Resgate { Id = idResgate1, IdCliente = idCliente, MotivoResgate = nomeResgate1, Valor = valorResgate1, DataResgate = dataResgate1 };
            var listResgate = new List<Resgate>();
            listResgate.Add(Resgate);
            listResgate.Add(Resgate1);
            _resgateRepository.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            var services = InicializaServico();

            //Act
            var response = await services.ResgateMensal(idCliente);

            //Assert
            Assert.Single(response.Resgates);
            Assert.Equal(3000, response.SaldoMensal);
            //Assert Resgate 
            var ResgateTest1 = response.Resgates.FirstOrDefault(g => g.Id == idResgate1);
            Assert.Equal(idResgate1, ResgateTest1.Id);
            Assert.Equal(idCliente, ResgateTest1.IdCliente);
            Assert.Equal(nomeResgate1, ResgateTest1.MotivoResgate);
            Assert.Equal(valorResgate1, ResgateTest1.Valor);
            Assert.Equal(dataResgate1, ResgateTest1.DataResgate);
        }

        [Fact]
        public async Task TelaInicialService_ResgateMensal_Com_DataAnterior()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idResgate = Guid.NewGuid();
            var idResgate1 = Guid.NewGuid();
            var valorResgate = 6000M;
            var valorResgate1 = 3000M;
            var nomeResgate = "Resgate";
            var nomeResgate1 = "Resgate1";
            var dataResgate = DateTime.Today.AddMonths(-5);
            var dataResgate1 = DateTime.Today.AddMonths(-6);
            var Resgate = new Resgate { Id = idResgate, IdCliente = idCliente, MotivoResgate = nomeResgate, Valor = valorResgate, DataResgate = dataResgate };
            var Resgate1 = new Resgate { Id = idResgate1, IdCliente = idCliente, MotivoResgate = nomeResgate1, Valor = valorResgate1, DataResgate = dataResgate1 };
            var listResgate = new List<Resgate>();
            listResgate.Add(Resgate);
            listResgate.Add(Resgate1);
            _resgateRepository.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            var services = InicializaServico();

            //Act
            var response = await services.ResgateMensal(idCliente);

            //Assert
            Assert.Empty(response.Resgates);
            Assert.Equal(0, response.SaldoMensal);
        }

        [Fact]
        public async Task TelaInicialService_ResgateMensal_Com_DataFutura()
        {
            //Arrange
            var idCliente = Guid.NewGuid();
            var idResgate = Guid.NewGuid();
            var idResgate1 = Guid.NewGuid();
            var valorResgate = 6000M;
            var valorResgate1 = 3000M;
            var nomeResgate = "Resgate";
            var nomeResgate1 = "Resgate1";
            var dataResgate = DateTime.Today.AddMonths(5);
            var dataResgate1 = DateTime.Today.AddMonths(6);
            var Resgate = new Resgate { Id = idResgate, IdCliente = idCliente, MotivoResgate = nomeResgate, Valor = valorResgate, DataResgate = dataResgate };
            var Resgate1 = new Resgate { Id = idResgate1, IdCliente = idCliente, MotivoResgate = nomeResgate1, Valor = valorResgate1, DataResgate = dataResgate1 };
            var listResgate = new List<Resgate>();
            listResgate.Add(Resgate);
            listResgate.Add(Resgate1);
            _resgateRepository.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            var services = InicializaServico();

            //Act
            var response = await services.ResgateMensal(idCliente);

            //Assert
            Assert.Empty(response.Resgates);
            Assert.Equal(0, response.SaldoMensal);
        }
    }
}
