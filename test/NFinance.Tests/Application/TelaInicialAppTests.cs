using NFinance.Application;
using NFinance.Application.ViewModel.ClientesViewModel;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.Application
{
    public class TelaInicialAppTests
    {
        private readonly IClienteService _clienteService;
        private readonly IGanhoService _ganhoService;
        private readonly IGastoService _gastoService;
        private readonly IInvestimentoService _investimentoService;
        private readonly IResgateService _resgateService;

        public TelaInicialAppTests()
        {
            _clienteService = Substitute.For<IClienteService>();
            _gastoService = Substitute.For<IGastoService>();
            _ganhoService = Substitute.For<IGanhoService>();
            _investimentoService = Substitute.For<IInvestimentoService>();
            _resgateService = Substitute.For<IResgateService>();
        }

        public TelaInicialApp InicializaApplication()
        {
            return new TelaInicialApp(_ganhoService, _investimentoService, _gastoService, _resgateService, _clienteService);
        }

        public static Cliente GeraCliente()
        {
            return new Cliente("Claudiosmar Santos", "123.654.987-96", "teste@teste.com", "SenhaSuperForte");
        }

        public static List<Ganho> GeraListaGanho(Cliente cliente)
        {
            var ganho = new Ganho(cliente.Id, "Salario", 20931209.923810M, true, DateTime.Parse("25/04/2021"));
            var ganho2 = new Ganho(cliente.Id, "Aluguel", 2023109.990352098M, true, DateTime.Parse("20/04/2021"));
            var ganho3 = new Ganho(cliente.Id, "Dividendos", 20931231678.890718273M, true, DateTime.Parse("10/04/2021"));

            return new List<Ganho> { ganho, ganho2, ganho3 };
        }

        public static List<Gasto> GeraListaGasto(Cliente cliente)
        {
            var gasto = new Gasto(cliente.Id, "Comida", 2893192.898M, 4, DateTime.Today);
            var gasto2 = new Gasto(cliente.Id, "Sapato", 2856462.82318M, 16, DateTime.Today.AddDays(3));
            var gasto3 = new Gasto(cliente.Id, "Geladeira", 2854392.8123M, 32, DateTime.Today.AddDays(7));

            return new List<Gasto> { gasto, gasto2, gasto3 };
        }

        public static List<Resgate> GeraListaResgate(Cliente cliente, List<Investimento> investimentos)
        {
            var resgate = new Resgate(investimentos.FirstOrDefault(i => i.NomeInvestimento.Equals("Fundo CTT")).Id, cliente.Id, 1091238.8M, "Necessidade", DateTime.Today);
            var resgate2 = new Resgate(investimentos.FirstOrDefault(i => i.NomeInvestimento.Equals("Fundo C32")).Id, cliente.Id, 109887.8M, "Necessidade", DateTime.Today);
            var resgate3 = new Resgate(investimentos.FirstOrDefault(i => i.NomeInvestimento.Equals("Fundo AJ4")).Id, cliente.Id, 1012487.8M, "Necessidade", DateTime.Today);

            return new List<Resgate> { resgate, resgate2, resgate3 };
        }

        public static List<Investimento> GeraListaInvestimento(Cliente cliente)
        {
            var investimento = new Investimento(cliente.Id, "Fundo CTT", 1091238.8M, DateTime.Today.AddDays(-7));
            var investimento2 = new Investimento(cliente.Id, "Fundo C32", 109887.8M, DateTime.Today.AddDays(-15));
            var investimento3 = new Investimento(cliente.Id, "Fundo AJ4", 1012487.8M, DateTime.Today.AddDays(-1));

            return new List<Investimento> { investimento, investimento2, investimento3 };
        }

        [Fact]
        public async Task TelaInicialService_TelaInicial_ComSucesso()
        {
            //Arrange
            var cliente = GeraCliente();
            var listaGanhos = GeraListaGanho(cliente);
            var listaGastos = GeraListaGasto(cliente);
            var listaInvestimentos = GeraListaInvestimento(cliente);
            var listaResgate = GeraListaResgate(cliente, listaInvestimentos);
            _ganhoService.ConsultarGanhos(Arg.Any<Guid>()).Returns(listaGanhos);
            _gastoService.ConsultarGastos(Arg.Any<Guid>()).Returns(listaGastos);
            _resgateService.ConsultarResgates(Arg.Any<Guid>()).Returns(listaResgate);
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(cliente);
            _investimentoService.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listaInvestimentos);
            var services = InicializaApplication();

            //Act
            var response = await services.TelaInicial(cliente.Id);

            //Assert
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
            Assert.Equal(cliente.Id, response.Cliente.Id);
            Assert.Equal(cliente.Nome, response.Cliente.Nome);
            Assert.Equal(cliente.CPF, response.Cliente.CPF);
            Assert.Equal(cliente.Email, response.Cliente.Email);
            Assert.Equal(cliente.Senha, response.Cliente.Senha);
            //Assert Ganho
            //var ganhoTest = response.GanhoMensal.Ganhos.FirstOrDefault(g => g.Id == idGanho);
            //Assert.Equal(idGanho, ganhoTest.Id);
            //Assert.Equal(idCliente, ganhoTest.IdCliente);
            //Assert.Equal(nomeGanho, ganhoTest.NomeGanho);
            //Assert.Equal(valorGanho, ganhoTest.Valor);
            //Assert.Equal(recorrente, ganhoTest.Recorrente);
            //Assert.Equal(dataGanho, ganhoTest.DataDoGanho);
            ////Assert Gasto
            //var gastoTest = response.GastoMensal.Gastos.FirstOrDefault(g => g.Id == idGasto);
            //Assert.Equal(idGasto, gastoTest.Id);
            //Assert.Equal(idCliente, gastoTest.IdCliente);
            //Assert.Equal(nomeGasto, gastoTest.NomeGasto);
            //Assert.Equal(333.33333333333333333333333333M, gastoTest.Valor);
            //Assert.Equal(qtdParcelas, gastoTest.QuantidadeParcelas);
            //Assert.Equal(dataGasto, gastoTest.DataDoGasto);
            ////Assert Gasto 1
            //var gastoTest1 = response.GastoMensal.Gastos.FirstOrDefault(g => g.Id == idGasto1);
            //Assert.Equal(idGasto1, gastoTest1.Id);
            //Assert.Equal(idCliente, gastoTest1.IdCliente);
            //Assert.Equal(nomeGasto, gastoTest1.NomeGasto);
            //Assert.Equal(166.66666666666666666666666667M, gastoTest1.Valor);
            //Assert.Equal(qtdParcelas1, gastoTest1.QuantidadeParcelas);
            //Assert.Equal(dataGasto1, gastoTest1.DataDoGasto);
            ////Assert Investimento
            //var investimentoTest = response.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == idInvestimento);
            //Assert.Equal(idInvestimento, investimentoTest.Id);
            //Assert.Equal(idCliente, investimentoTest.IdCliente);
            //Assert.Equal(nomeInvestimento, investimentoTest.NomeInvestimento);
            //Assert.Equal(valorInvestimento, investimentoTest.Valor);
            //Assert.Equal(dataAplicacao, investimentoTest.DataAplicacao);
            ////Assert Investimento 1
            //var investimentoTest1 = response.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == idInvestimento1);
            //Assert.Equal(idInvestimento1, investimentoTest1.Id);
            //Assert.Equal(idCliente, investimentoTest1.IdCliente);
            //Assert.Equal(nomeInvestimento, investimentoTest1.NomeInvestimento);
            //Assert.Equal(valorInvestimento, investimentoTest1.Valor);
            //Assert.Equal(dataAplicacao1, investimentoTest1.DataAplicacao);
            ////Assert Resgate
            //var resgateTest = response.ResgateMensal.Resgates.FirstOrDefault(g => g.Id == idResgate);
            //Assert.Equal(idResgate, resgateTest.Id);
            //Assert.Equal(idCliente, resgateTest.IdCliente);
            //Assert.Equal(motivoResgate, resgateTest.MotivoResgate);
            //Assert.Equal(valorInvestimento, resgateTest.Valor);
            //Assert.Equal(dataResgate, resgateTest.DataResgate);
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
            listGanho.Add(ganho);
            listGanho.Add(ganho1);
            listGanho.Add(ganho2);
            listGasto.Add(gasto);
            listGasto.Add(gasto1);
            listInvestimento.Add(investimento);
            listInvestimento.Add(investimento1);
            listResgate.Add(resgate);
            _ganhoService.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            _gastoService.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            _resgateService.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new Cliente());
            _investimentoService.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaApplication();

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
            _ganhoService.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaApplication();

            //Act
            var response = await services.GanhoMensal(idCliente);

            //Assert
            Assert.Equal(9000, response.SaldoMensal);
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
            _ganhoService.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaApplication();

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
            _ganhoService.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaApplication();

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
            _ganhoService.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaApplication();

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
            _gastoService.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            var services = InicializaApplication();

            //Act
            var response = await services.GastoMensal(idCliente);

            //Assert
            Assert.Equal(2, response.Gastos.Count);
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
            _gastoService.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            var services = InicializaApplication();

            //Act
            var response = await services.GastoMensal(idCliente);

            //Assert
            Assert.Empty(response.Gastos);
            Assert.Equal(0, response.SaldoMensal);
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
            _gastoService.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            var services = InicializaApplication();

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
            _gastoService.ConsultarGastos(Arg.Any<Guid>()).Returns(listGasto);
            _gastoService.AtualizarGasto(Arg.Any<Gasto>()).Returns(gastoResponse);
            var services = InicializaApplication();

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
            _investimentoService.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaApplication();

            //Act
            var response = await services.InvestimentoMensal(idCliente);

            //Assert
            Assert.Equal(2, response.Investimentos.Count);
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
            _investimentoService.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaApplication();

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
            _investimentoService.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaApplication();

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
            _investimentoService.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaApplication();

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
            _investimentoService.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimento);
            var services = InicializaApplication();

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
            _resgateService.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            var services = InicializaApplication();

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
            _resgateService.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            var services = InicializaApplication();

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
            _resgateService.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            var services = InicializaApplication();

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
            _resgateService.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgate);
            var services = InicializaApplication();

            //Act
            var response = await services.ResgateMensal(idCliente);

            //Assert
            Assert.Empty(response.Resgates);
            Assert.Equal(0, response.SaldoMensal);
        }
    }
}
