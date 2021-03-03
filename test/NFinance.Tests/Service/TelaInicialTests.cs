using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.Domain.ViewModel.ClientesViewModel;
using NFinance.Domain.ViewModel.TelaInicialViewModel;
using NFinance.Infra.Repository;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.Service
{
    public class TelaInicialTests
    {
        private readonly IClienteService _clienteService;
        private readonly IGanhoRepository _ganhoRepository;
        private readonly IGastoRepository _gastoRepository;
        private readonly IInvestimentoRepository _investimentoRepository;
        private readonly IResgateRepository _resgateRepository;

        public TelaInicialTests()
        {
            _clienteService = Substitute.For<IClienteService>();
            _gastoRepository = Substitute.For<IGastoRepository>();
            _ganhoRepository = Substitute.For<IGanhoRepository>();
            _investimentoRepository = Substitute.For<IInvestimentoRepository>();
            _resgateRepository = Substitute.For<IResgateRepository>();
        }

        public TelaInicialService InicializaServico()
        {
            return new TelaInicialService(_ganhoRepository, _investimentoRepository, _gastoRepository, _resgateRepository, _clienteService );
        }

        [Fact]
        public async Task TelaInicialService_ExibirDados_ComSucesso()
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
            Assert.Equal(2000, response.GastoMensal.SaldoMensal);
            Assert.Equal(10000, response.ResgateMensal.SaldoMensal);
            Assert.Equal(20000, response.InvestimentoMensal.SaldoMensal);
            Assert.Equal(3000, response.ResumoMensal);
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
            Assert.Equal(valorGasto, gastoTest.Valor);
            Assert.Equal(qtdParcelas, gastoTest.QuantidadeParcelas);
            Assert.Equal(dataGasto, gastoTest.DataDoGasto);
            //Assert Gasto 1
            var gastoTest1 = response.GastoMensal.Gastos.FirstOrDefault(g => g.Id == idGasto1);
            Assert.Equal(idGasto1, gastoTest1.Id);
            Assert.Equal(idCliente, gastoTest1.IdCliente);
            Assert.Equal(nomeGasto, gastoTest1.NomeGasto);
            Assert.Equal(valorGasto, gastoTest1.Valor);
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
            Assert.Equal(dataAplicacao, resgateTest.DataResgate);
        }
    }
}
