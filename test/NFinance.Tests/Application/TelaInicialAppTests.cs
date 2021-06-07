using NFinance.Application;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var ganho = new Ganho(cliente.Id, "Salario", 5000M, true, DateTime.Today);
            var ganho2 = new Ganho(cliente.Id, "Aluguel", 3000M, false, DateTime.Today.AddDays(-1));
            var ganho3 = new Ganho(cliente.Id, "Dividendos", 10000M, true, DateTime.Today.AddDays(-2));

            return new List<Ganho> { ganho, ganho2, ganho3 };
        }

        public static List<Gasto> GeraListaGasto(Cliente cliente)
        {
            var gasto = new Gasto(cliente.Id, "Comida", 2000M, 4, DateTime.Today);
            var gasto2 = new Gasto(cliente.Id, "Sapato", 1500M, 16, DateTime.Today.AddDays(-3));
            var gasto3 = new Gasto(cliente.Id, "Geladeira", 25000M, 32, DateTime.Today.AddDays(-7));

            return new List<Gasto> { gasto, gasto2, gasto3 };
        }

        public static List<Resgate> GeraListaResgate(Cliente cliente, List<Investimento> investimentos)
        {
            var resgate = new Resgate(investimentos.FirstOrDefault(i => i.NomeInvestimento.Equals("Fundo CTT")).Id, cliente.Id, 5000M, "Necessidade", DateTime.Today);
            var resgate2 = new Resgate(investimentos.FirstOrDefault(i => i.NomeInvestimento.Equals("Fundo C32")).Id, cliente.Id, 1000M, "Necessidade", DateTime.Today);
            var resgate3 = new Resgate(investimentos.FirstOrDefault(i => i.NomeInvestimento.Equals("Fundo AJ4")).Id, cliente.Id, 1200M, "Necessidade", DateTime.Today);

            return new List<Resgate> { resgate, resgate2, resgate3 };
        }

        public static List<Investimento> GeraListaInvestimento(Cliente cliente)
        {
            var investimento = new Investimento(cliente.Id, "Fundo CTT", 10000M, DateTime.Today.AddDays(-4));
            var investimento2 = new Investimento(cliente.Id, "Fundo C32", 150000M, DateTime.Today.AddDays(-3));
            var investimento3 = new Investimento(cliente.Id, "Fundo AJ4", 35000M, DateTime.Today.AddDays(-1));

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
            Assert.NotEmpty(response.GanhoMensal.Ganhos);
            Assert.NotEmpty(response.GastoMensal.Gastos);
            Assert.NotEmpty(response.ResgateMensal.Resgates);
            Assert.NotEmpty(response.InvestimentoMensal.Investimentos);
            Assert.Equal(18000M, response.GanhoMensal.SaldoMensal);
            Assert.Equal(1375.00M, response.GastoMensal.SaldoMensal);
            Assert.Equal(7200M, response.ResgateMensal.SaldoMensal);
            Assert.Equal(195000M, response.InvestimentoMensal.SaldoMensal);
            Assert.Equal(16625.00M, response.ResumoMensal);
            //Assert Cliente
            Assert.Equal(cliente.Id, response.Cliente.Id);
            Assert.Equal(cliente.Nome, response.Cliente.Nome);
            Assert.Equal(cliente.CPF, response.Cliente.CPF);
            Assert.Equal(cliente.Email, response.Cliente.Email);
            Assert.Equal(cliente.Senha, response.Cliente.Senha);
            //Assert Ganho
            var ganho = listaGanhos[0];
            var ganhoTest = response.GanhoMensal.Ganhos.FirstOrDefault(g => g.Id == ganho.Id);
            Assert.Equal(ganho.Id, ganhoTest.Id);
            Assert.Equal(ganho.IdCliente, ganhoTest.IdCliente);
            Assert.Equal(ganho.NomeGanho, ganhoTest.NomeGanho);
            Assert.Equal(ganho.Valor, ganhoTest.Valor);
            Assert.Equal(ganho.Recorrente, ganhoTest.Recorrente);
            Assert.Equal(ganho.DataDoGanho, ganhoTest.DataDoGanho);
            //Assert Ganho1
            var ganho1 = listaGanhos[1];
            var ganhoTest1 = response.GanhoMensal.Ganhos.FirstOrDefault(g => g.Id == ganho1.Id);
            Assert.Equal(ganho1.Id, ganhoTest1.Id);
            Assert.Equal(ganho1.IdCliente, ganhoTest1.IdCliente);
            Assert.Equal(ganho1.NomeGanho, ganhoTest1.NomeGanho);
            Assert.Equal(ganho1.Valor, ganhoTest1.Valor);
            Assert.Equal(ganho1.Recorrente, ganhoTest1.Recorrente);
            Assert.Equal(ganho1.DataDoGanho, ganhoTest1.DataDoGanho);
            //Assert Ganho2
            var ganho2 = listaGanhos[2];
            var ganhoTest2 = response.GanhoMensal.Ganhos.FirstOrDefault(g => g.Id == ganho2.Id);
            Assert.Equal(ganho2.Id, ganhoTest2.Id);
            Assert.Equal(ganho2.IdCliente, ganhoTest2.IdCliente);
            Assert.Equal(ganho2.NomeGanho, ganhoTest2.NomeGanho);
            Assert.Equal(ganho2.Valor, ganhoTest2.Valor);
            Assert.Equal(ganho2.Recorrente, ganhoTest2.Recorrente);
            Assert.Equal(ganho2.DataDoGanho, ganhoTest2.DataDoGanho);
            //Assert Gasto
            var gasto = listaGastos[0];
            var gastoTest = response.GastoMensal.Gastos.FirstOrDefault(g => g.Id == gasto.Id);
            Assert.Equal(gasto.Id, gastoTest.Id);
            Assert.Equal(gasto.IdCliente, gastoTest.IdCliente);
            Assert.Equal(gasto.NomeGasto, gastoTest.NomeGasto);
            Assert.Equal(gasto.Valor, gastoTest.Valor);
            Assert.Equal(gasto.QuantidadeParcelas, gastoTest.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto, gastoTest.DataDoGasto);
            //Assert Gasto 1
            var gasto1 = listaGastos[1];
            var gastoTest1 = response.GastoMensal.Gastos.FirstOrDefault(g => g.Id == gasto1.Id);
            Assert.Equal(gasto1.Id, gastoTest1.Id);
            Assert.Equal(gasto1.IdCliente, gastoTest1.IdCliente);
            Assert.Equal(gasto1.NomeGasto, gastoTest1.NomeGasto);
            Assert.Equal(gasto1.Valor, gastoTest1.Valor);
            Assert.Equal(gasto1.QuantidadeParcelas, gastoTest1.QuantidadeParcelas);
            Assert.Equal(gasto1.DataDoGasto, gastoTest1.DataDoGasto);
            //Assert Gasto 2
            var gasto2 = listaGastos[2];
            var gastoTest2 = response.GastoMensal.Gastos.FirstOrDefault(g => g.Id == gasto2.Id);
            Assert.Equal(gasto2.Id, gastoTest2.Id);
            Assert.Equal(gasto2.IdCliente, gastoTest2.IdCliente);
            Assert.Equal(gasto2.NomeGasto, gastoTest2.NomeGasto);
            Assert.Equal(gasto2.Valor, gastoTest2.Valor);
            Assert.Equal(gasto2.QuantidadeParcelas, gastoTest2.QuantidadeParcelas);
            Assert.Equal(gasto2.DataDoGasto, gastoTest2.DataDoGasto);
            //Assert Investimento
            var investimento = listaInvestimentos[0];
            var investimentoTest = response.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == investimento.Id);
            Assert.Equal(investimento.Id, investimentoTest.Id);
            Assert.Equal(investimento.IdCliente, investimentoTest.IdCliente);
            Assert.Equal(investimento.NomeInvestimento, investimentoTest.NomeInvestimento);
            Assert.Equal(investimento.Valor, investimentoTest.Valor);
            Assert.Equal(investimento.DataAplicacao, investimentoTest.DataAplicacao);
            //Assert Investimento 1
            var investimento1 = listaInvestimentos[1];
            var investimentoTest1 = response.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == investimento1.Id);
            Assert.Equal(investimento1.Id, investimentoTest1.Id);
            Assert.Equal(investimento1.IdCliente, investimentoTest1.IdCliente);
            Assert.Equal(investimento1.NomeInvestimento, investimentoTest1.NomeInvestimento);
            Assert.Equal(investimento1.Valor, investimentoTest1.Valor);
            Assert.Equal(investimento1.DataAplicacao, investimentoTest1.DataAplicacao);
            //Assert Investimento 2
            var investimento2 = listaInvestimentos[2];
            var investimentoTest2 = response.InvestimentoMensal.Investimentos.FirstOrDefault(g => g.Id == investimento2.Id);
            Assert.Equal(investimento2.Id, investimentoTest2.Id);
            Assert.Equal(investimento2.IdCliente, investimentoTest2.IdCliente);
            Assert.Equal(investimento2.NomeInvestimento, investimentoTest2.NomeInvestimento);
            Assert.Equal(investimento2.Valor, investimentoTest2.Valor);
            Assert.Equal(investimento2.DataAplicacao, investimentoTest2.DataAplicacao);
            //Assert Resgate
            var resgate = listaResgate[0];
            var resgateTest = response.ResgateMensal.Resgates.FirstOrDefault(g => g.Id == resgate.Id);
            Assert.Equal(resgate.Id, resgateTest.Id);
            Assert.Equal(resgate.IdCliente, resgateTest.IdCliente);
            Assert.Equal(resgate.MotivoResgate, resgateTest.MotivoResgate);
            Assert.Equal(resgate.Valor, resgateTest.Valor);
            Assert.Equal(resgate.DataResgate, resgateTest.DataResgate);
            //Assert Resgate 1
            var resgate1 = listaResgate[1];
            var resgateTest1 = response.ResgateMensal.Resgates.FirstOrDefault(g => g.Id == resgate1.Id);
            Assert.Equal(resgate1.Id, resgateTest1.Id);
            Assert.Equal(resgate1.IdCliente, resgateTest1.IdCliente);
            Assert.Equal(resgate1.MotivoResgate, resgateTest1.MotivoResgate);
            Assert.Equal(resgate1.Valor, resgateTest1.Valor);
            Assert.Equal(resgate1.DataResgate, resgateTest1.DataResgate);
            //Assert Resgate 2
            var resgate2 = listaResgate[2];
            var resgateTest2 = response.ResgateMensal.Resgates.FirstOrDefault(g => g.Id == resgate2.Id);
            Assert.Equal(resgate2.Id, resgateTest2.Id);
            Assert.Equal(resgate2.IdCliente, resgateTest2.IdCliente);
            Assert.Equal(resgate2.MotivoResgate, resgateTest2.MotivoResgate);
            Assert.Equal(resgate2.Valor, resgateTest2.Valor);
            Assert.Equal(resgate2.DataResgate, resgateTest2.DataResgate);
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
            var dataGanho1 = DateTime.Today.AddDays(-3);
            var recorrente = false;
            var recorrente1 = true;
            var ganho = new Ganho { Id = idGanho, IdCliente = idCliente, Valor = valorGanho, NomeGanho = nomeGanho, DataDoGanho = dataGanho, Recorrente = recorrente };
            var ganho1 = new Ganho { Id = idGanho1, IdCliente = idCliente, Valor = valorGanho1, NomeGanho = nomeGanho1, DataDoGanho = dataGanho1, Recorrente = recorrente1 };
            var listGanho = new List<Ganho> { ganho, ganho1};
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
            var listGanho = new List<Ganho> { ganho, ganho1 };
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
            var listGanho = new List<Ganho> { ganho, ganho1 };
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
            var listGanho = new List<Ganho> { ganho, ganho1 };

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
            var dataGasto1 = DateTime.Today;
            var dataGasto = DateTime.Today.AddDays(-1);
            var qtdParcelas = 0;
            var gasto = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var gasto1 = new Gasto { Id = idGasto1, IdCliente = idCliente, NomeGasto = nomeGasto1, Valor = valorGasto1, DataDoGasto = dataGasto1, QuantidadeParcelas = qtdParcelas };
            var listGasto = new List<Gasto> { gasto, gasto1 };
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
            var listGasto = new List<Gasto> { gasto, gasto1 };
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
            var listGasto = new List<Gasto> { gasto, gasto1 };
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
            var dataGasto1 = DateTime.Today.AddMonths(-5).AddDays(-5);
            var dataGasto = DateTime.Today;
            var qtdParcelas = 6;
            var qtdParcelas1 = 0;
            var gasto = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = valorGasto, DataDoGasto = dataGasto, QuantidadeParcelas = qtdParcelas };
            var gasto1 = new Gasto { Id = idGasto1, IdCliente = idCliente, NomeGasto = nomeGasto1, Valor = valorGasto1, DataDoGasto = dataGasto1, QuantidadeParcelas = qtdParcelas1 };
            var gastoResponse = new Gasto { Id = idGasto, IdCliente = idCliente, NomeGasto = nomeGasto, Valor = 1000, DataDoGasto = dataGasto, QuantidadeParcelas = 5 };
            var listGasto = new List<Gasto> { gasto, gasto1 };
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
            var dataInvestimento1 = DateTime.Today.AddDays(-4);
            var dataInvestimento = DateTime.Today;
            var Investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataInvestimento };
            var Investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento1, Valor = valorInvestimento1, DataAplicacao = dataInvestimento1 };
            var listInvestimento = new List<Investimento> { Investimento, Investimento1 };
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
            var listInvestimento = new List<Investimento> { Investimento,Investimento1 };
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
            var listInvestimento = new List<Investimento> { Investimento, Investimento1 };
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
            var listInvestimento = new List<Investimento> { Investimento, Investimento1 };
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
            var dataInvestimento2 = DateTime.Today;
            var Investimento = new Investimento { Id = idInvestimento, IdCliente = idCliente, NomeInvestimento = nomeInvestimento, Valor = valorInvestimento, DataAplicacao = dataInvestimento };
            var Investimento1 = new Investimento { Id = idInvestimento1, IdCliente = idCliente, NomeInvestimento = nomeInvestimento1, Valor = valorInvestimento1, DataAplicacao = dataInvestimento1 };
            var Investimento2 = new Investimento { Id = idInvestimento2, IdCliente = idCliente, NomeInvestimento = nomeInvestimento2, Valor = valorInvestimento2, DataAplicacao = dataInvestimento2 };
            var listInvestimento = new List<Investimento> { Investimento, Investimento1, Investimento2 };
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
            var listResgate = new List<Resgate> { Resgate, Resgate1 };
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
            var listResgate = new List<Resgate> { Resgate, Resgate1 };
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
            var listResgate = new List<Resgate> { Resgate, Resgate1 };
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
            var listResgate = new List<Resgate> { Resgate, Resgate1 };
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
