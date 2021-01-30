using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Gasto;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Services;
using NFinance.Model.ClientesViewModel;
using NFinance.Model.GastosViewModel;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace NFinance.Tests.Service
{
    public class GastoServiceTests
    {
        private IClienteService _clienteService;
        private IGastosRepository _gastosRepository;

        public GastoServiceTests()
        {
            _clienteService = Substitute.For<IClienteService>();
            _gastosRepository = Substitute.For<IGastosRepository>();
        }

        public GastosService InicializaServico()
        {
            return new GastosService(_gastosRepository, _clienteService);
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
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
            Assert.Equal(valorTotal, response.ValorTotal);
            Assert.Equal(data, response.DataDoGasto);
            Assert.Equal(qtdParcelas, response.QuantidadeParcelas);
            Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComNomeGasto_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComNomeGasto_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = null, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = null, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComNomeGasto_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "  ";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComValorTotal_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 0;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<ValorGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComQuantidadeParcela_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 1004684;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<QuantidadeParcelaExpcetion>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComQuantidadeParcela_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 0;
            var data = DateTime.Today;
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<QuantidadeParcelaExpcetion>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComDataGasto_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 15;
            var data = DateTime.Today.AddYears(-120);
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_CadastrarGasto_ComDataGasto_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 15;
            var data = DateTime.Today.AddYears(+120);
            var gastoRequest = new CadastrarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.CadastrarGasto(Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.CadastrarGasto(gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(),Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
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
            Assert.Equal(valorTotal, response.ValorTotal);
            Assert.Equal(data, response.DataDoGasto);
            Assert.Equal(qtdParcelas, response.QuantidadeParcelas);
            Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComIdGasto_Invalido()
        {
            //Arrange
            var id = Guid.Empty;
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComNomeGasto_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(id,gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComNomeGasto_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = null, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = null, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComNomeGasto_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "  ";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<NomeGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComValorTotal_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 0;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<ValorGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComQuantidadeParcela_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 1004684;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<QuantidadeParcelaExpcetion>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComQuantidadeParcela_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 0;
            var data = DateTime.Today;
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<QuantidadeParcelaExpcetion>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComDataGasto_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 15;
            var data = DateTime.Today.AddYears(-120);
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_AtualizarGasto_ComDataGasto_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 1354851.144M;
            var qtdParcelas = 15;
            var data = DateTime.Today.AddYears(+120);
            var gastoRequest = new AtualizarGastoViewModel.Request() { IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            _gastosRepository.AtualizarGasto(Arg.Any<Guid>(), Arg.Any<Gastos>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<DataGastoException>(() => /*Act*/ services.AtualizarGasto(id, gastoRequest));
        }

        [Fact]
        public async void GastosService_ExcluirGasto_ComSucesso()
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
        public async void GastosService_ExcluirGasto_ComIdGasto_Invalido()
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
        public async void GastosService_ExcluirGasto_ComIdCliente_Invalido()
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
        public async void GastosService_ExcluirGasto_ComMotivoExclusao_Vazio()
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
        public async void GastosService_ExcluirGasto_ComMotivoExclusao_Nulo()
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
        public async void GastosService_ExcluirGasto_ComMotivoExclusao_EmBranco()
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
        public async void GastosService_ConsultarGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            _gastosRepository.ConsultarGasto(Arg.Any<Guid>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
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
            Assert.Equal(valorTotal, response.ValorTotal);
            Assert.Equal(data, response.DataDoGasto);
            Assert.Equal(qtdParcelas, response.QuantidadeParcelas);
            Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async void GastosService_ConsultarGasto_ComId_Invalido()
        {
            //Arrange
            var id = Guid.Empty;
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            _gastosRepository.ConsultarGasto(Arg.Any<Guid>()).Returns(new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data });
            _clienteService.ConsultarCliente(Arg.Any<Guid>()).Returns(new ConsultarClienteViewModel.Response() { Id = idCliente, Nome = nomeCliente });
            var services = InicializaServico();

            ///Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarGasto(id));
        }

        [Fact]
        public async void GastosService_ListarGasto_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var nomeGasto = "Teste@Sucesso";
            var nomeCliente = "Claudemir Salbisn";
            decimal valorTotal = 123871239.21M;
            var qtdParcelas = 15;
            var data = DateTime.Today;
            var gasto = new Gastos() { Id = id, IdCliente = idCliente, NomeGasto = nomeGasto, QuantidadeParcelas = qtdParcelas, ValorTotal = valorTotal, DataDoGasto = data };
            var cliente = new Cliente() { Id = idCliente , Nome = nomeCliente};
            var listaGasto = new List<Gastos>();
            var listaCliente = new List<Cliente>();
            listaGasto.Add(gasto);
            listaCliente.Add(cliente);
            var listarCliente = new ListarClientesViewModel.Response(listaCliente);
            _gastosRepository.ListarGastos().Returns(new List<Gastos>(listaGasto));
            _clienteService.ListarClientes().Returns(listarCliente);
            var services = InicializaServico();

            //Act
            var response = await services.ListarGastos();

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Find(g => g.Id == id).NomeGasto);
            Assert.False(response.Find(g => g.Id == id).QuantidadeParcelas <= 0);
            Assert.False(response.Find(g => g.Id == id).QuantidadeParcelas >= 1000);
            Assert.False(response.Find(g => g.Id == id).DataDoGasto.CompareTo(DateTime.MaxValue.AddYears(-7899)) > 0);
            Assert.False(response.Find(g => g.Id == id).DataDoGasto.CompareTo(DateTime.MinValue.AddYears(1949)) < 0);
            Assert.NotEqual(Guid.Empty, response.Find(g => g.Id == id).Id);
            Assert.NotEqual(Guid.Empty, response.Find(g => g.Id == id).IdCliente);
            Assert.Equal(id, response.Find(g => g.Id == id).Id);
            Assert.Equal(idCliente, response.Find(g => g.Id == id).IdCliente);
            Assert.Equal(nomeGasto, response.Find(g => g.Id == id).NomeGasto);
            Assert.Equal(valorTotal, response.Find(g => g.Id == id).ValorTotal);
            Assert.Equal(data, response.Find(g => g.Id == id).DataDoGasto);
            Assert.Equal(qtdParcelas, response.Find(g => g.Id == id).QuantidadeParcelas);
        }
    }
}
