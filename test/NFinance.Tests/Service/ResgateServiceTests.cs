using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Resgate;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.ViewModel.InvestimentosViewModel;
using NFinance.ViewModel.ResgatesViewModel;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NFinance.Domain.ViewModel.ClientesViewModel;
using Xunit;
using System.Linq;

namespace NFinance.Tests.Service
{
    public class ResgateServiceTests
    {
        private readonly IResgateRepository _resgateRepository;
        private readonly IInvestimentoService _investimentosService;

        public ResgateServiceTests()
        {
            _resgateRepository = Substitute.For<IResgateRepository>();
            _investimentosService = Substitute.For<IInvestimentoService>();
        }

        public ResgateService InicializaServico()
        {
            return new ResgateService(_resgateRepository, _investimentosService);
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var cliente = new ClienteViewModel.SimpleResponse { Id = Guid.NewGuid(), Nome = "Claudemir Tester" };
            var investimentoResponse = new ConsultarInvestimentoViewModel.Response { Id = idInvestimento, DataAplicacao = DateTime.Today.AddDays(-5), Cliente = cliente, NomeInvestimento = "Investimento JOSDaSDA", Valor = valor };
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento , MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = cliente.Id };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate {Id = id , IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = cliente.Id });
            _investimentosService.ConsultarInvestimento(Arg.Any<Guid>()).Returns(investimentoResponse);
            var services = InicializaServico();

            //Act
            var response = await services.RealizarResgate(resgateRequest);

            //Assert
            Assert.NotNull(response.MotivoResgate);
            Assert.NotEqual("00/00/00", response.DataResgate.ToString());
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.NotNull(response.Investimento);
            Assert.Equal(id, response.Id);
            Assert.Equal(cliente.Id, response.IdCliente);
            Assert.Equal(motivo, response.MotivoResgate);
            Assert.Equal(valor, response.Valor);
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComIdInvestimento_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.Empty;
            var motivo = "Teste@Sucesso";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComMotivo_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<MotivoResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComMotivo_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "  ";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<MotivoResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComMotivo_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento, MotivoResgate = null, Valor = valor, DataResgate = data, IdCliente = idCliente };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = null, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<MotivoResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComValor_Zero()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 0;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComDataResgate_Invalida_Menor_Minimo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 13546547.215M;
            var data = DateTime.Today.AddYears(-100);
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComDataResgate_Invalida_Maior_Maximo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 13546547.215M;
            var data = DateTime.Today.AddYears(100);
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 13546547.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_ConsultarResgate_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgate = new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente };
            var cliente = new ClienteViewModel.SimpleResponse { Id = idCliente, Nome = "Claudemir Tester" };
            var investimentoResponse = new ConsultarInvestimentoViewModel.Response { Id = idInvestimento, DataAplicacao = DateTime.Today.AddDays(-5), Cliente = cliente, NomeInvestimento = "Investimento JOSDaSDA", Valor = valor };
            _resgateRepository.ConsultarResgate(Arg.Any<Guid>()).Returns(resgate);
            _investimentosService.ConsultarInvestimento(Arg.Any<Guid>()).Returns(investimentoResponse);
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarResgate(id);

            //Assert
            Assert.NotNull(response.MotivoResgate);
            Assert.NotEqual("00/00/00", response.DataResgate.ToString());
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.NotNull(response.Investimento);
            Assert.Equal(id, response.Id);
            Assert.Equal(idCliente, response.IdCliente);
            Assert.Equal(motivo, response.MotivoResgate);
            Assert.Equal(valor, response.Valor);
        }

        [Fact]
        public async Task ResgateService_ConsultarResgate_ComId_Vazio()
        {
            //Arrange
            var id = Guid.Empty;
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            _resgateRepository.ConsultarResgate(Arg.Any<Guid>()).Returns(new Resgate { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data, IdCliente = idCliente });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarResgate(id));
        }

        [Fact]
        public async Task ResgateService_ConsultarResgates_ComSucesso()
        {
            //Arrage
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var idInvestimento2 = Guid.NewGuid();
            var idInvestimento3 = Guid.NewGuid();
            var valor = 120245.21M;
            var motivoResgate = "TEsteee";
            var dataResgate = DateTime.Today;
            var resgate = new Resgate { Id = id, IdInvestimento = idInvestimento, Valor = valor,  MotivoResgate = motivoResgate, DataResgate = dataResgate, IdCliente = idCliente };
            var resgate1 = new Resgate { Id = id1, IdInvestimento = idInvestimento1, Valor = valor, MotivoResgate = motivoResgate, DataResgate = dataResgate, IdCliente = idCliente };
            var resgate2 = new Resgate { Id = id2, IdInvestimento = idInvestimento2, Valor = valor, MotivoResgate = motivoResgate, DataResgate = dataResgate, IdCliente = idCliente };
            var resgate3 = new Resgate { Id = id3, IdInvestimento = idInvestimento3, Valor = valor, MotivoResgate = motivoResgate, DataResgate = dataResgate, IdCliente = idCliente };
            var listResgates = new List<Resgate> { resgate, resgate1, resgate2, resgate3 };
            _resgateRepository.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgates);
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarResgates(idCliente);

            //Assert
            Assert.IsType<ConsultarResgatesViewModel.Response>(response);
            Assert.NotNull(response);
            Assert.Equal(4, response.Count);

            //Assert dos ganhos do cliente - resgate 0
            var responseTeste = response.FirstOrDefault(g => g.Id == id);
            Assert.IsType<ResgateViewModel.Response>(responseTeste);
            Assert.Equal(id, responseTeste.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(idInvestimento, responseTeste.IdInvestimento);
            Assert.Equal(motivoResgate, responseTeste.MotivoResgate);
            Assert.Equal(valor, responseTeste.Valor);
            Assert.Equal(dataResgate, responseTeste.DataResgate);

            //Assert dos ganhos do cliente - resgate 1
            var responseTeste1 = response.FirstOrDefault(g => g.Id == id1);
            Assert.IsType<ResgateViewModel.Response>(responseTeste1);
            Assert.Equal(id1, responseTeste1.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(idInvestimento1, responseTeste1.IdInvestimento);
            Assert.Equal(motivoResgate, responseTeste1.MotivoResgate);
            Assert.Equal(valor, responseTeste1.Valor);
            Assert.Equal(dataResgate, responseTeste1.DataResgate);

            //Assert dos ganhos do cliente - resgate 2
            var responseTeste2 = response.FirstOrDefault(g => g.Id == id2);
            Assert.IsType<ResgateViewModel.Response>(responseTeste2);
            Assert.Equal(id2, responseTeste2.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(idInvestimento2, responseTeste2.IdInvestimento);
            Assert.Equal(motivoResgate, responseTeste2.MotivoResgate);
            Assert.Equal(valor, responseTeste2.Valor);
            Assert.Equal(dataResgate, responseTeste2.DataResgate);

            //Assert dos ganhos do cliente - resgate 3
            var responseTeste3 = response.FirstOrDefault(g => g.Id == id3);
            Assert.IsType<ResgateViewModel.Response>(responseTeste3);
            Assert.Equal(id3, responseTeste3.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(idInvestimento3, responseTeste3.IdInvestimento);
            Assert.Equal(motivoResgate, responseTeste3.MotivoResgate);
            Assert.Equal(valor, responseTeste3.Valor);
            Assert.Equal(dataResgate, responseTeste3.DataResgate);
        }

        [Fact]
        public async Task ResgateService_ConsultarResgates_ComIdCliente_Invalido()
        {
            //Arrage
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var idInvestimento = Guid.NewGuid();
            var idInvestimento1 = Guid.NewGuid();
            var idInvestimento2 = Guid.NewGuid();
            var idInvestimento3 = Guid.NewGuid();
            var valor = 120245.21M;
            var motivoResgate = "TEsteee";
            var dataResgate = DateTime.Today;
            var resgate = new Resgate { Id = id, IdInvestimento = idInvestimento, Valor = valor, MotivoResgate = motivoResgate, DataResgate = dataResgate, IdCliente = idCliente };
            var resgate1 = new Resgate { Id = id1, IdInvestimento = idInvestimento1, Valor = valor, MotivoResgate = motivoResgate, DataResgate = dataResgate, IdCliente = idCliente };
            var resgate2 = new Resgate { Id = id2, IdInvestimento = idInvestimento2, Valor = valor, MotivoResgate = motivoResgate, DataResgate = dataResgate, IdCliente = idCliente };
            var resgate3 = new Resgate { Id = id3, IdInvestimento = idInvestimento3, Valor = valor, MotivoResgate = motivoResgate, DataResgate = dataResgate, IdCliente = idCliente };
            var listResgates = new List<Resgate> { resgate, resgate1, resgate2, resgate3 };
            _resgateRepository.ConsultarResgates(Arg.Any<Guid>()).Returns(listResgates);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarResgates(idCliente));
        }
    }
}
