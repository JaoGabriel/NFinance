using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Resgate;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.Model.ClientesViewModel;
using NFinance.Model.InvestimentosViewModel;
using NFinance.Model.ResgatesViewModel;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.Service
{
    public class ResgateServiceTests
    {
        private readonly IResgateRepository _resgateRepository;
        private readonly IInvestimentosService _investimentosService;

        public ResgateServiceTests()
        {
            _resgateRepository = Substitute.For<IResgateRepository>();
            _investimentosService = Substitute.For<IInvestimentosService>();
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
            var cliente = new ClienteViewModel.Response() { Id = Guid.NewGuid(), Nome = "Claudemir Tester" };
            var investimentoResponse = new ConsultarInvestimentoViewModel.Response() { Id = idInvestimento, DataAplicacao = DateTime.Today.AddDays(-5), Cliente = cliente, NomeInvestimento = "Investimento JOSDaSDA", Valor = valor };
            var resgateRequest = new RealizarResgateViewModel.Request() { IdInvestimento = idInvestimento , MotivoResgate = motivo, Valor = valor, DataResgate = data };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate() {Id = id , IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            _investimentosService.ConsultarInvestimento(Arg.Any<Guid>()).Returns(investimentoResponse);
            var services = InicializaServico();

            //Act
            var response = await services.RealizarResgate(resgateRequest);

            //Assert
            Assert.NotNull(response.MotivoResgate);
            Assert.NotEqual("00/00/00", response.DataResgate.ToString());
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotNull(response.Investimento);
            Assert.Equal(id, response.Id);
            Assert.Equal(motivo, response.MotivoResgate);
            Assert.Equal(valor, response.Valor);
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComIdInvestimento_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.Empty;
            var motivo = "Teste@Sucesso";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request() { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComMotivo_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request() { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<MotivoResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComMotivo_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "  ";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request() { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<MotivoResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComMotivo_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request() { IdInvestimento = idInvestimento, MotivoResgate = null, Valor = valor, DataResgate = data };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = null, Valor = valor, DataResgate = data });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<MotivoResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComValor_Zero()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 0;
            var data = DateTime.Today;
            var resgateRequest = new RealizarResgateViewModel.Request() { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComDataResgate_Invalida_Menor_Minimo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 13546547.215M;
            var data = DateTime.Today.AddYears(-100);
            var resgateRequest = new RealizarResgateViewModel.Request() { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComDataResgate_Invalida_Maior_Maximo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 13546547.215M;
            var data = DateTime.Today.AddYears(100);
            var resgateRequest = new RealizarResgateViewModel.Request() { IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data };
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataResgateException>(() => /*Act*/ services.RealizarResgate(resgateRequest));
        }

        [Fact]
        public async Task ResgateService_ConsultarResgate_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var cliente = new ClienteViewModel.Response() { Id = Guid.NewGuid(), Nome = "Claudemir Tester" };
            var investimentoResponse = new ConsultarInvestimentoViewModel.Response() { Id = idInvestimento, DataAplicacao = DateTime.Today.AddDays(-5), Cliente = cliente, NomeInvestimento = "Investimento JOSDaSDA", Valor = valor };
            _resgateRepository.ConsultarResgate(Arg.Any<Guid>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            _investimentosService.ConsultarInvestimento(Arg.Any<Guid>()).Returns(investimentoResponse);
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarResgate(id);

            //Assert
            Assert.NotNull(response.MotivoResgate);
            Assert.NotEqual("00/00/00", response.DataResgate.ToString());
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotNull(response.Investimento);
            Assert.Equal(id, response.Id);
            Assert.Equal(motivo, response.MotivoResgate);
            Assert.Equal(valor, response.Valor);
        }

        [Fact]
        public async Task ResgateService_ConsultarResgate_ComId_Vazio()
        {
            //Arrange
            var id = Guid.Empty;
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            _resgateRepository.ConsultarResgate(Arg.Any<Guid>()).Returns(new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data });
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarResgate(id));
        }

        [Fact]
        public async Task ResgateService_ListarResgates_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivo = "Teste@Sucesso";
            decimal valor = 21245.215M;
            var data = DateTime.Today;
            var listaResgates = new List<Resgate>();
            var listaInvestimentos = new List<Investimentos>();
            var resgate = new Resgate() { Id = id, IdInvestimento = idInvestimento, MotivoResgate = motivo, Valor = valor, DataResgate = data };
            listaResgates.Add(resgate);
            var investimento = new Investimentos() { Id = idInvestimento, DataAplicacao = DateTime.Today.AddDays(-5), IdCliente = Guid.NewGuid(), NomeInvestimento = "Investimento JOSDaSDA", Valor = valor };
            listaInvestimentos.Add(investimento);
            var investimentoResponse = new ListarInvestimentosViewModel.Response(listaInvestimentos);
            _resgateRepository.ListarResgates().Returns(new List<Resgate>(listaResgates));
            _investimentosService.ListarInvestimentos().Returns(investimentoResponse);
            var services = InicializaServico();

            //Act
            
            var response = await services.ListarResgates();

            //Assert
            Assert.NotNull(response.Find(r => r.Id == id).MotivoResgate);
            Assert.NotEqual("00/00/00", response.Find(r => r.Id == id).DataResgate.ToString());
            Assert.NotEqual(0, response.Find(r => r.Id == id).Valor);
            Assert.NotEqual(Guid.Empty, response.Find(r => r.Id == id).Id);
            Assert.NotEqual(Guid.Empty, response.Find(r => r.Id == id).IdInvestimento);
            Assert.Equal(id, response.Find(r => r.Id == id).Id);
            Assert.Equal(motivo, response.Find(r => r.Id == id).MotivoResgate);
            Assert.Equal(valor, response.Find(r => r.Id == id).Valor);
            Assert.Equal(data, response.Find(r => r.Id == id).DataResgate);
            Assert.Equal(idInvestimento, response.Find(r => r.Id == id).IdInvestimento);
        }
    }
}
