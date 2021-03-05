using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Gasto;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.Model.GastosViewModel;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NFinance.Domain.ViewModel.ClientesViewModel;
using Xunit;
using System.Linq;

namespace NFinance.Tests.Service
{
    public class GastoServiceTests
    {
        private readonly IClienteService _clienteService;
        private readonly IGastoRepository _gastosRepository;

        public GastoServiceTests()
        {
            _clienteService = Substitute.For<IClienteService>();
            _gastosRepository = Substitute.For<IGastoRepository>();
        }

        public GastoService InicializaServico()
        {
            return new GastoService(_gastosRepository, _clienteService);
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            //Act
            var response = await services.CadastrarGasto(gastoRequest);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGasto);
            Assert.False(response.QuantidadeParcelas <= 0);
            Assert.False(response.QuantidadeParcelas >= 1000);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MaxValue.AddYears(-7899)) > 0);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MinValue.AddYears(1949)) < 0);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.Cliente.Id);
            Assert.NotNull(response.Cliente.Nome);
            Assert.Equal(id, response.Id);
            Assert.Equal(idCliente, response.Cliente.Id);
            Assert.Equal(nomeGasto, response.NomeGasto);
            Assert.Equal(valorTotal, response.Valor);
            Assert.Equal(data, response.DataDoGasto);
            Assert.Equal(qtdParcelas, response.QuantidadeParcelas);
            Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComNomeGasto_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComNomeGasto_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = null, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = null, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComNomeGasto_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "  ";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComValorTotal_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 0;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<ValorGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComQuantidadeParcela_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 1004684;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<QuantidadeParcelaException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComQuantidadeParcela_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 0;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<QuantidadeParcelaException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComDataGasto_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 15;
            var data = DateTime.Today.AddYears(-120);
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_CadastrarGasto_ComDataGasto_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 15;
            var data = DateTime.Today.AddYears(+120);
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(),Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            //Act
            var response = await services.AtualizarGasto(id,gastoRequest);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGasto);
            Assert.False(response.QuantidadeParcelas <= 0);
            Assert.False(response.QuantidadeParcelas >= 1000);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MaxValue.AddYears(-7899)) > 0);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MinValue.AddYears(1949)) < 0);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.Cliente.Id);
            Assert.NotNull(response.Cliente.Nome);
            Assert.Equal(id, response.Id);
            Assert.Equal(idCliente, response.Cliente.Id);
            Assert.Equal(nomeGasto, response.NomeGasto);
            Assert.Equal(valorTotal, response.Valor);
            Assert.Equal(data, response.DataDoGasto);
            Assert.Equal(qtdParcelas, response.QuantidadeParcelas);
            Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComIdGasto_Invalido()
        {
            //Arrange
            var id = Guid.Empty;
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComNomeGasto_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(id,gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComNomeGasto_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = null, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = null, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComNomeGasto_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "  ";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComValorTotal_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 0;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<ValorGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComQuantidadeParcela_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 1004684;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<QuantidadeParcelaException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComQuantidadeParcela_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = -1;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<QuantidadeParcelaException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComDataGasto_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 15;
            var data = DateTime.Today.AddYears(-120);
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_AtualizarGasto_ComDataGasto_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 15;
            var data = DateTime.Today.AddYears(+120);
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gasto>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var motivoExclusao = "Teste@Sucesso";
            var gastoRequest = new ExcluirGastoViewModel.Request() { IdCliente = idCliente, IdGasto = id , MotivoExclusao = motivoExclusao };
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            //Act
            var response = await services.ExcluirGasto(gastoRequest);

            //Assert
            Assert.NotNull(response);
            Assert.False(response.DataExclusao.CompareTo(DateTime.Today) == 0);
            Assert.Equal("Excluido Com Sucesso",response.Mensagem);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComIdGasto_Invalido()
        {
            //Arrange
            var id = Guid.Empty;
            var idCliente = Guid.NewGuid();
            var motivoExclusao = "Teste@Sucesso";
            var gastoRequest = new ExcluirGastoViewModel.Request() { IdCliente = idCliente, IdGasto = id, MotivoExclusao = motivoExclusao };
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ExcluirGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var motivoExclusao = "Teste@Sucesso";
            var gastoRequest = new ExcluirGastoViewModel.Request() { IdCliente = idCliente, IdGasto = id, MotivoExclusao = motivoExclusao };
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ExcluirGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComMotivoExclusao_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var motivoExclusao = "";
            var gastoRequest = new ExcluirGastoViewModel.Request() { IdCliente = idCliente, IdGasto = id, MotivoExclusao = motivoExclusao };
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.ExcluirGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComMotivoExclusao_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var gastoRequest = new ExcluirGastoViewModel.Request() { IdCliente = idCliente, IdGasto = id, MotivoExclusao = null };
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.ExcluirGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_ExcluirGasto_ComMotivoExclusao_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var motivoExclusao = "  ";
            var gastoRequest = new ExcluirGastoViewModel.Request() { IdCliente = idCliente, IdGasto = id, MotivoExclusao = motivoExclusao };
            _gastosRepository.ExcluirGasto(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.ExcluirGasto(gastoRequest));
        }

        [Fact]
        public async Task GastosService_ConsultarGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            _gastosRepository.ConsultarGasto(Arg.Any<Guid>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarGasto(id);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGasto);
            Assert.False(response.QuantidadeParcelas <= 0);
            Assert.False(response.QuantidadeParcelas >= 1000);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MaxValue.AddYears(-7899)) > 0);
            Assert.False(response.DataDoGasto.CompareTo(DateTime.MinValue.AddYears(1949)) < 0);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.Cliente.Id);
            Assert.NotNull(response.Cliente.Nome);
            Assert.Equal(id, response.Id);
            Assert.Equal(idCliente, response.Cliente.Id);
            Assert.Equal(nomeGasto, response.NomeGasto);
            Assert.Equal(valorTotal, response.Valor);
            Assert.Equal(data, response.DataDoGasto);
            Assert.Equal(qtdParcelas, response.QuantidadeParcelas);
            Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async Task GastosService_ConsultarGasto_ComId_Invalido()
        {
            //Arrange
            var id = Guid.Empty;
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            _gastosRepository.ConsultarGasto(Arg.Any<Guid>()).Returns(new Gasto() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, Valor = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarGasto(id));
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
            Assert.IsType<ConsultarGastosViewModel.Response>(response);
            Assert.NotNull(response);
            Assert.Equal(4, response.Count);
            
            //Assert dos ganhos do cliente - gasto 0
            var responseTeste = response.FirstOrDefault(g => g.Id == id);
            Assert.IsType<GastoViewModel.Response>(responseTeste);
            Assert.Equal(id, responseTeste.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(nomeGasto, responseTeste.NomeGasto);
            Assert.Equal(valor, responseTeste.Valor);
            Assert.Equal(2,responseTeste.QuantidadeParcelas);
            Assert.Equal(dataDoGasto,responseTeste.DataDoGasto);

            //Assert dos ganhos do cliente - gasto 1
            var responseTeste1 = response.FirstOrDefault(g => g.Id == id1);
            Assert.IsType<GastoViewModel.Response>(responseTeste1);
            Assert.Equal(id1, responseTeste1.Id);
            Assert.Equal(idCliente, responseTeste1.IdCliente);
            Assert.Equal(nomeGasto, responseTeste1.NomeGasto);
            Assert.Equal(valor, responseTeste1.Valor);
            Assert.Equal(5, responseTeste1.QuantidadeParcelas);
            Assert.Equal(dataDoGasto, responseTeste1.DataDoGasto);

            //Assert dos ganhos do cliente - gasto 2
            var responseTeste2 = response.FirstOrDefault(g => g.Id == id2);
            Assert.IsType<GastoViewModel.Response>(responseTeste2);
            Assert.Equal(id2, responseTeste2.Id);
            Assert.Equal(idCliente, responseTeste2.IdCliente);
            Assert.Equal(nomeGasto, responseTeste2.NomeGasto);
            Assert.Equal(valor, responseTeste2.Valor);
            Assert.Equal(7, responseTeste2.QuantidadeParcelas);
            Assert.Equal(dataDoGasto, responseTeste2.DataDoGasto);

            //Assert dos ganhos do cliente - gasto 3
            var responseTeste3 = response.FirstOrDefault(g => g.Id == id3);
            Assert.IsType<GastoViewModel.Response>(responseTeste3);
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
