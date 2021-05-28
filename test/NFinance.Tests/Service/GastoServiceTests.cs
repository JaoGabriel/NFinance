using Xunit;
using System;
using NSubstitute;
using System.Linq;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Domain.Services;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Gasto;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.GastosViewModel;

namespace NFinance.Tests.Service
{
    public class GastoServiceTests
    {
        private readonly IGastoRepository _gastosRepository;

        public GastoServiceTests()
        {
            _gastosRepository = Substitute.For<IGastoRepository>();
        }

        public GastoService InicializaServico()
        {
            return new GastoService(_gastosRepository);
        }

        public static Gasto GerarGasto()
        {
            return new Gasto(Guid.NewGuid(), "GASDASASDA", 30811231.1293M, 10, DateTime.Today.AddDays(-10));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComSucesso()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Act
            var response = await services.CadastrarGasto(gasto);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGasto);
            Assert.False(response.QuantidadeParcelas <= 0);
            Assert.False(response.QuantidadeParcelas >= 1000);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MaxValue.AddYears(-7899)) > 0);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MinValue.AddYears(1949)) < 0);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.Equal(gasto.Id, response.Id);
            Assert.Equal(gasto.IdCliente, response.IdCliente);
            Assert.Equal(gasto.NomeGasto, response.NomeGasto);
            Assert.Equal(gasto.Valor, response.Valor);
            Assert.Equal(gasto.DataDoGasto, response.DataDoGasto);
            Assert.Equal(gasto.QuantidadeParcelas, response.QuantidadeParcelas);
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComNomeGasto_Vazio()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComNomeGasto_Nulo()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComNomeGasto_EmBranco()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComValorTotal_Invalido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorGastoException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComQuantidadeParcela_Maior_Permitido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<QuantidadeParcelaException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComQuantidadeParcela_Menor_Permitido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<QuantidadeParcelaException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComDataGasto_Menor_Permitido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComDataGasto_Maior_Permitido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.CadastrarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComSucesso()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Act
            var response = await services.AtualizarGasto(gasto);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGasto);
            Assert.False(response.QuantidadeParcelas <= 0);
            Assert.False(response.QuantidadeParcelas >= 1000);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MaxValue.AddYears(-7899)) > 0);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MinValue.AddYears(1949)) < 0);
            Assert.NotEqual(Guid.Empty, response.Id);
            //Assert.NotEqual(Guid.Empty, response.Cliente.Id);
            //Assert.NotNull(response.Cliente.Nome);
            //Assert.Equal(id, response.Id);
            //Assert.Equal(idCliente, response.Cliente.Id);
            //Assert.Equal(nomeGasto, response.NomeGasto);
            //Assert.Equal(valorTotal, response.Valor);
            //Assert.Equal(data, response.DataDoGasto);
            //Assert.Equal(qtdParcelas, response.QuantidadeParcelas);
            //Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComIdGasto_Invalido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComNomeGasto_Vazio()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComNomeGasto_Nulo()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComNomeGasto_EmBranco()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComValorTotal_Invalido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorGastoException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComQuantidadeParcela_Maior_Permitido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<QuantidadeParcelaException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComQuantidadeParcela_Menor_Permitido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<QuantidadeParcelaException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComDataGasto_Menor_Permitido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComDataGasto_Maior_Permitido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.AtualizarGasto(Arg.Any<Gasto>()).Returns(gasto);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.AtualizarGasto(gasto));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComSucesso()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            //Act
            var response = await services.ExcluirGasto(gasto);

            //Assert
            Assert.True(response);            
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComIdGasto_Invalido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ExcluirGasto(gasto));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ExcluirGasto(gasto));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComMotivoExclusao_Vazio()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.ExcluirGasto(gasto));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComMotivoExclusao_Nulo()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.ExcluirGasto(gasto));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComMotivoExclusao_EmBranco()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.ExcluirGasto(gasto));
        }

        [Fact]
        public async Task GastosService_ConsultarGasto_ComSucesso()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.ConsultarGasto(Arg.Any<Guid>()).Returns(gasto);
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarGasto(gasto.Id);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGasto);
            Assert.False(response.QuantidadeParcelas <= 0);
            Assert.False(response.QuantidadeParcelas >= 1000);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MaxValue.AddYears(-7899)) > 0);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MinValue.AddYears(1949)) < 0);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            //Assert.Equal(id, response.Id);
            //Assert.Equal(idCliente, response.Cliente.Id);
            //Assert.Equal(nomeGasto, response.NomeGasto);
            //Assert.Equal(valorTotal, response.Valor);
            //Assert.Equal(data, response.DataDoGasto);
            //Assert.Equal(qtdParcelas, response.QuantidadeParcelas);
            //Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async Task GastosService_ConsultarGasto_ComId_Invalido()
        {
            //Arrange
            var gasto = GerarGasto();
            _gastosRepository.ConsultarGasto(Arg.Any<Guid>()).Returns(gasto);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarGasto(gasto.Id));
        }

        [Fact]
        public async Task GastoService_ConsultarGastos_ComSucesso()
        {
            //Arrage
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGasto = "TEsteee";
            var dataDoGasto = DateTime.Today;
            var gasto = new Gasto { Id = id, IdCliente = idCliente, Valor = valor, NomeGasto = nomeGasto, QuantidadeParcelas = 2, DataDoGasto = dataDoGasto };
            var gasto1 = new Gasto { Id = id1, IdCliente = idCliente, Valor = valor, NomeGasto = nomeGasto, QuantidadeParcelas = 5, DataDoGasto = dataDoGasto };
            var gasto2 = new Gasto { Id = id2, IdCliente = idCliente, Valor = valor, NomeGasto = nomeGasto, QuantidadeParcelas = 7, DataDoGasto = dataDoGasto };
            var gasto3 = new Gasto { Id = id3, IdCliente = idCliente, Valor = valor, NomeGasto = nomeGasto, QuantidadeParcelas = 10, DataDoGasto = dataDoGasto };
            var listGastos = new List<Gasto> { gasto, gasto1, gasto2, gasto3 };
            _gastosRepository.ConsultarGastos(Arg.Any<Guid>()).Returns(listGastos);
            var services = InicializaServico();
            
            //Act
            var response = await services.ConsultarGastos(idCliente);
            
            //Assert
            Assert.NotNull(response);
            Assert.Equal(4, response.Count);
            
            //Assert dos ganhos do cliente - gasto 0
            var responseTeste = response.FirstOrDefault(g => g.Id == id);
            Assert.IsType<GastoViewModel>(responseTeste);
            Assert.Equal(id, responseTeste.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(nomeGasto, responseTeste.NomeGasto);
            Assert.Equal(valor, responseTeste.Valor);
            Assert.Equal(2,responseTeste.QuantidadeParcelas);
            Assert.Equal(dataDoGasto,responseTeste.DataDoGasto);

            //Assert dos ganhos do cliente - gasto 1
            var responseTeste1 = response.FirstOrDefault(g => g.Id == id1);
            Assert.IsType<GastoViewModel>(responseTeste1);
            Assert.Equal(id1, responseTeste1.Id);
            Assert.Equal(idCliente, responseTeste1.IdCliente);
            Assert.Equal(nomeGasto, responseTeste1.NomeGasto);
            Assert.Equal(valor, responseTeste1.Valor);
            Assert.Equal(5, responseTeste1.QuantidadeParcelas);
            Assert.Equal(dataDoGasto, responseTeste1.DataDoGasto);

            //Assert dos ganhos do cliente - gasto 2
            var responseTeste2 = response.FirstOrDefault(g => g.Id == id2);
            Assert.IsType<GastoViewModel>(responseTeste2);
            Assert.Equal(id2, responseTeste2.Id);
            Assert.Equal(idCliente, responseTeste2.IdCliente);
            Assert.Equal(nomeGasto, responseTeste2.NomeGasto);
            Assert.Equal(valor, responseTeste2.Valor);
            Assert.Equal(7, responseTeste2.QuantidadeParcelas);
            Assert.Equal(dataDoGasto, responseTeste2.DataDoGasto);

            //Assert dos ganhos do cliente - gasto 3
            var responseTeste3 = response.FirstOrDefault(g => g.Id == id3);
            Assert.IsType<GastoViewModel>(responseTeste3);
            Assert.Equal(id3, responseTeste3.Id);
            Assert.Equal(idCliente, responseTeste3.IdCliente);
            Assert.Equal(nomeGasto, responseTeste3.NomeGasto);
            Assert.Equal(valor, responseTeste3.Valor);
            Assert.Equal(10, responseTeste3.QuantidadeParcelas);
            Assert.Equal(dataDoGasto, responseTeste3.DataDoGasto);
        }

        [Fact]
        public async Task GastosService_ConsultarGastos_ComIdCliente_Invalido()
        {
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var valor = 120245.21M;
            var nomeGasto = "TEsteee";
            var dataDoGasto = DateTime.Today;
            var gasto = new Gasto { Id = id, IdCliente = idCliente, Valor = valor, NomeGasto = nomeGasto, QuantidadeParcelas = 2, DataDoGasto = dataDoGasto };
            var gasto1 = new Gasto { Id = id1, IdCliente = idCliente, Valor = valor, NomeGasto = nomeGasto, QuantidadeParcelas = 5, DataDoGasto = dataDoGasto };
            var gasto2 = new Gasto { Id = id2, IdCliente = idCliente, Valor = valor, NomeGasto = nomeGasto, QuantidadeParcelas = 7, DataDoGasto = dataDoGasto };
            var gasto3 = new Gasto { Id = id3, IdCliente = idCliente, Valor = valor, NomeGasto = nomeGasto, QuantidadeParcelas = 10, DataDoGasto = dataDoGasto };
            var listGanho = new List<Gasto> { gasto, gasto1, gasto2, gasto3 };
            _gastosRepository.ConsultarGastos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarGastos(idCliente));
        }
    }
}
