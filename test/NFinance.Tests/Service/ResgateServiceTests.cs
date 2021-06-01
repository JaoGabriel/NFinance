using Xunit;
using System;
using System.Linq;
using NSubstitute;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Domain.Services;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Resgate;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Tests.Service
{
    public class ResgateServiceTests
    {
        private readonly IResgateRepository _resgateRepository;

        public ResgateServiceTests()
        {
            _resgateRepository = Substitute.For<IResgateRepository>();
        }

        public ResgateService InicializaServico()
        {
            return new ResgateService(_resgateRepository);
        }

        public Resgate GeraResgate()
        {
            return new Resgate(Guid.NewGuid(),Guid.NewGuid(),31827636178.1231289M,"Necessidade",DateTime.Today);
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComSucesso()
        {
            //Arrange
            var resgate = GeraResgate();
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(resgate);
            var services = InicializaServico();

            //Act
            var response = await services.RealizarResgate(resgate);

            //Assert
            Assert.NotNull(response.MotivoResgate);
            Assert.NotEqual("00/00/00", response.DataResgate.ToString());
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.NotEqual(Guid.Empty, response.IdInvestimento);
            Assert.Equal(resgate.Id, response.Id);
            Assert.Equal(resgate.IdCliente, response.IdCliente);
            Assert.Equal(resgate.MotivoResgate, response.MotivoResgate);
            Assert.Equal(resgate.Valor, response.Valor);
            Assert.Equal(resgate.DataResgate, response.DataResgate);
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComIdInvestimento_Vazio()
        {
            //Arrange
            var resgate = GeraResgate();
            resgate.IdInvestimento = Guid.Empty;
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(resgate);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.RealizarResgate(resgate));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComIdCliente_Vazio()
        {
            //Arrange
            var resgate = GeraResgate();
            resgate.IdCliente = Guid.Empty;
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(resgate);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.RealizarResgate(resgate));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ResgateService_RealizarResgate_ComMotivo_Invalido(string motivo)
        {
            //Arrange
            var resgate = GeraResgate();
            resgate.MotivoResgate = motivo;
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(resgate);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<MotivoResgateException>(() => /*Act*/ services.RealizarResgate(resgate));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1500)]
        public async Task ResgateService_RealizarResgate_ComValor_Invalido(decimal valor)
        {
            //Arrange
            var resgate = GeraResgate();
            resgate.Valor = valor;
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(resgate);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorException>(() => /*Act*/ services.RealizarResgate(resgate));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComDataResgate_Invalida_Menor_Minimo()
        {
            //Arrange
            var resgate = GeraResgate();
            resgate.DataResgate = DateTime.Today.AddYears(-120);
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(resgate);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataResgateException>(() => /*Act*/ services.RealizarResgate(resgate));
        }

        [Fact]
        public async Task ResgateService_RealizarResgate_ComDataResgate_Invalida_Maior_Maximo()
        {
            //Arrange
            var resgate = GeraResgate();
            resgate.DataResgate = DateTime.Today.AddYears(120);
            _resgateRepository.RealizarResgate(Arg.Any<Resgate>()).Returns(resgate);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataResgateException>(() => /*Act*/ services.RealizarResgate(resgate));
        }

        [Fact]
        public async Task ResgateService_ConsultarResgate_ComSucesso()
        {
            //Arrange
            var resgate = GeraResgate();
            _resgateRepository.ConsultarResgate(Arg.Any<Guid>()).Returns(resgate);
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarResgate(resgate.Id);

            //Assert
            Assert.NotNull(response.MotivoResgate);
            Assert.NotEqual("00/00/00", response.DataResgate.ToString());
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.NotEqual(Guid.Empty, response.IdInvestimento);
            Assert.Equal(resgate.Id, response.Id);
            Assert.Equal(resgate.IdCliente, response.IdCliente);
            Assert.Equal(resgate.MotivoResgate, response.MotivoResgate);
            Assert.Equal(resgate.Valor, response.Valor);
            Assert.Equal(resgate.DataResgate, response.DataResgate);
        }

        [Fact]
        public async Task ResgateService_ConsultarResgate_ComId_Vazio()
        {
            //Arrange
            var resgate = GeraResgate();
            resgate.Id = Guid.Empty;
            _resgateRepository.ConsultarResgate(Arg.Any<Guid>()).Returns(resgate);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarResgate(resgate.Id));
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
            Assert.NotNull(response);
            Assert.Equal(4, response.Count);

            //Assert dos ganhos do cliente - resgate 0
            var responseTeste = response.FirstOrDefault(g => g.Id == id);
            Assert.IsType<Resgate>(responseTeste);
            Assert.Equal(id, responseTeste.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(idInvestimento, responseTeste.IdInvestimento);
            Assert.Equal(motivoResgate, responseTeste.MotivoResgate);
            Assert.Equal(valor, responseTeste.Valor);
            Assert.Equal(dataResgate, responseTeste.DataResgate);

            //Assert dos ganhos do cliente - resgate 1
            var responseTeste1 = response.FirstOrDefault(g => g.Id == id1);
            Assert.IsType<Resgate>(responseTeste1);
            Assert.Equal(id1, responseTeste1.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(idInvestimento1, responseTeste1.IdInvestimento);
            Assert.Equal(motivoResgate, responseTeste1.MotivoResgate);
            Assert.Equal(valor, responseTeste1.Valor);
            Assert.Equal(dataResgate, responseTeste1.DataResgate);

            //Assert dos ganhos do cliente - resgate 2
            var responseTeste2 = response.FirstOrDefault(g => g.Id == id2);
            Assert.IsType<Resgate>(responseTeste2);
            Assert.Equal(id2, responseTeste2.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(idInvestimento2, responseTeste2.IdInvestimento);
            Assert.Equal(motivoResgate, responseTeste2.MotivoResgate);
            Assert.Equal(valor, responseTeste2.Valor);
            Assert.Equal(dataResgate, responseTeste2.DataResgate);

            //Assert dos ganhos do cliente - resgate 3
            var responseTeste3 = response.FirstOrDefault(g => g.Id == id3);
            Assert.IsType<Resgate>(responseTeste3);
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
