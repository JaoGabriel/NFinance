using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Ganho;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Services;
using NFinance.Domain.ViewModel.GanhoViewModel;
using NSubstitute;
using Xunit;

namespace NFinance.Tests.Service
{
    public class GanhoServiceTests
    {
        private readonly IGanhoRepository _ganhoRepository;

        public GanhoServiceTests()
        {
            _ganhoRepository = Substitute.For<IGanhoRepository>();
        }

        public GanhoService InicializaServico()
        {
            return new(_ganhoRepository);
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComSucesso()
        {
            //Arrage
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = "TEsteee";
            var dataGanho = DateTime.Today;
            var ganho = new Ganho {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = dataGanho };
            var ganhoRequest = new CadastrarGanhoViewModel.Request {IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = dataGanho };
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();
            //Act
            var response = await services.CadastrarGanho(ganhoRequest);
            //Assert
            Assert.IsType<CadastrarGanhoViewModel.Response>(response);
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGanho);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.NotEqual("00/00/00", response.DataDoGanho.ToString());
            Assert.Equal(id, response.Id);
            Assert.Equal(idCliente, response.IdCliente);
            Assert.Equal(nomeGanho, response.NomeGanho);
            Assert.Equal(valor, response.Valor);
            Assert.True(response.Recorrente);
            Assert.Equal(dataGanho, response.DataDoGanho);
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var valor = 120245.21M;
            var nomeGanho = "TEsteee";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new CadastrarGanhoViewModel.Request
                {IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.CadastrarGanho(ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComNomeGanho_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = "";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new CadastrarGanhoViewModel.Request
                {IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => /*Act*/ services.CadastrarGanho(ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComNomeGanho_Branco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = " ";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new CadastrarGanhoViewModel.Request
                {IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => /*Act*/ services.CadastrarGanho(ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComNomeGanho_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var ganho = new Ganho {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = null, Recorrente = true};
            var ganhoRequest = new CadastrarGanhoViewModel.Request
                {IdCliente = idCliente, Valor = valor, NomeGanho = null, Recorrente = true};
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => /*Act*/ services.CadastrarGanho(ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComValorGanho_Negativo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = -120245.21M;
            var nomeGanho = "dsuhahusdsuha";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new CadastrarGanhoViewModel.Request
                {IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorGanhoException>(() => /*Act*/ services.CadastrarGanho(ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComValorGanho_Zero()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 0;
            var nomeGanho = "dsuhahusdsuha";
            var ganho = new Ganho {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new CadastrarGanhoViewModel.Request {IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorGanhoException>(() => /*Act*/ services.CadastrarGanho(ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComDataAplicacao_Invalido_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 293128.123M;
            var nomeGanho = "dsuhahusdsuha";
            var data = DateTime.Today.AddYears(120);
            var ganho = new Ganho { Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = data };
            var ganhoRequest = new CadastrarGanhoViewModel.Request { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = data };
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => /*Act*/ services.CadastrarGanho(ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComDataAplicacao_Invalido_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 293128.123M;
            var nomeGanho = "dsuhahusdsuha";
            var data = DateTime.Today.AddYears(-120);
            var ganho = new Ganho { Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = data };
            var ganhoRequest = new CadastrarGanhoViewModel.Request { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = data };
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => /*Act*/ services.CadastrarGanho(ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComSucesso()
        {
            //Arrage
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = "TEsteee";
            var ganho = new Ganho {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new AtualizarGanhoViewModel.Request { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();
            //Act
            var response = await services.AtualizarGanho(id, ganhoRequest);
            //Assert
            Assert.IsType<AtualizarGanhoViewModel.Response>(response);
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGanho);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.Equal(id, response.Id);
            Assert.Equal(idCliente, response.IdCliente);
            Assert.Equal(nomeGanho, response.NomeGanho);
            Assert.Equal(valor, response.Valor);
            Assert.True(response.Recorrente);
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComIdGanho_Invalido()
        {
            //Arrange
            var id = Guid.Empty;
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = "TEsteee";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new AtualizarGanhoViewModel.Request
                {Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGanho(id, ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var valor = 120245.21M;
            var nomeGanho = "TEsteee";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new AtualizarGanhoViewModel.Request
                {Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGanho(id, ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComNomeGanho_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = "";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new AtualizarGanhoViewModel.Request
                { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => /*Act*/ services.AtualizarGanho(id, ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComNomeGanho_Branco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = " ";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new AtualizarGanhoViewModel.Request
                { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => /*Act*/ services.AtualizarGanho(id, ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComNomeGanho_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var ganho = new Ganho {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = null, Recorrente = true};
            var ganhoRequest = new AtualizarGanhoViewModel.Request
                { IdCliente = idCliente, Valor = valor, NomeGanho = null, Recorrente = true};
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => /*Act*/ services.AtualizarGanho(id, ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComValorGanho_Negativo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = -120245.21M;
            var nomeGanho = "dsuhahusdsuha";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new AtualizarGanhoViewModel.Request
                { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorGanhoException>(() => /*Act*/ services.AtualizarGanho(id, ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComValorGanho_Zero()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 0;
            var nomeGanho = "dsuhahusdsuha";
            var ganho = new Ganho {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganhoRequest = new AtualizarGanhoViewModel.Request { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorGanhoException>(() => /*Act*/ services.AtualizarGanho(id, ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComDataAplicacao_Invalido_Maior_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 293128.123M;
            var data = DateTime.Today.AddYears(120);
            var nomeGanho = "dsuhahusdsuha";
            var ganho = new Ganho { Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = data };
            var ganhoRequest = new AtualizarGanhoViewModel.Request { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true };
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => /*Act*/ services.AtualizarGanho(id,ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComDataAplicacao_Invalido_Menor_Permitido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 293128.123M;
            var nomeGanho = "dsuhahusdsuha";
            var data = DateTime.Today.AddYears(-120);
            var ganho = new Ganho { Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = data };
            var ganhoRequest = new AtualizarGanhoViewModel.Request { IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true, DataDoGanho = data };
            _ganhoRepository.AtualizarGanho(Arg.Any<Guid>(), Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => /*Act*/ services.AtualizarGanho(id,ganhoRequest));
        }

        [Fact]
        public async Task GanhoService_ConsultarGanho_ComSucesso()
        {
            //Arrage
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = "TEsteee";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.ConsultarGanho(Arg.Any<Guid>()).Returns(ganho);
            var services = InicializaServico();
            //Act
            var response = await services.ConsultarGanho(id);
            //Assert
            Assert.IsType<ConsultarGanhoViewModel.Response>(response);
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGanho);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.Equal(id, response.Id);
            Assert.Equal(idCliente, response.IdCliente);
            Assert.Equal(nomeGanho, response.NomeGanho);
            Assert.Equal(valor, response.Valor);
            Assert.True(response.Recorrente);
        }

        [Fact]
        public async Task GanhoService_ConsultarGanho_ComIdGanho_Invalido()
        {
            //Arrange
            var id = Guid.Empty;
            var idCliente = Guid.NewGuid();
            var valor = 17283893.123M;
            var nomeGanho = "dsuhahusdsuha";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            _ganhoRepository.ConsultarGanho(Arg.Any<Guid>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarGanho(id));
        }

        [Fact]
        public async Task GanhoService_ConsultarGanhos_ComSucesso()
        {
            //Arrage
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeGanho = "TEsteee";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganho1 = new Ganho
                {Id = id1, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false};
            var ganho2 = new Ganho
                {Id = id2, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false};
            var ganho3 = new Ganho
                {Id = id3, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false};
            var listGanho = new List<Ganho> {ganho, ganho1, ganho2, ganho3};
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();
            //Act
            var response = await services.ConsultarGanhos(idCliente);
            //Assert
            Assert.IsType<ConsultarGanhosViewModel.Response>(response);
            Assert.NotNull(response);
            Assert.Equal(4, response.Count);
            //Assert dos ganhos do cliente - ganho 0
            var responseTeste = response.FirstOrDefault(g => g.Id == id);
            Assert.IsType<GanhoViewModel>(responseTeste);
            Assert.Equal(id, responseTeste.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(nomeGanho, responseTeste.NomeGanho);
            Assert.Equal(valor, responseTeste.Valor);
            Assert.True(responseTeste.Recorrente);
            //Assert dos ganhos do cliente - ganho 1
            var responseTeste1 = response.FirstOrDefault(g => g.Id == id1);
            Assert.IsType<GanhoViewModel>(responseTeste1);
            Assert.Equal(id1, responseTeste1.Id);
            Assert.Equal(idCliente, responseTeste1.IdCliente);
            Assert.Equal(nomeGanho, responseTeste1.NomeGanho);
            Assert.Equal(valor, responseTeste1.Valor);
            Assert.False(responseTeste1.Recorrente);
            //Assert dos ganhos do cliente - ganho 2
            var responseTeste2 = response.FirstOrDefault(g => g.Id == id2);
            Assert.IsType<GanhoViewModel>(responseTeste2);
            Assert.Equal(id2, responseTeste2.Id);
            Assert.Equal(idCliente, responseTeste2.IdCliente);
            Assert.Equal(nomeGanho, responseTeste2.NomeGanho);
            Assert.Equal(valor, responseTeste2.Valor);
            Assert.False(responseTeste2.Recorrente);
            //Assert dos ganhos do cliente - ganho 3
            var responseTeste3 = response.FirstOrDefault(g => g.Id == id3);
            Assert.IsType<GanhoViewModel>(responseTeste3);
            Assert.Equal(id3, responseTeste3.Id);
            Assert.Equal(idCliente, responseTeste3.IdCliente);
            Assert.Equal(nomeGanho, responseTeste3.NomeGanho);
            Assert.Equal(valor, responseTeste3.Valor);
            Assert.False(responseTeste3.Recorrente);
        }

        [Fact]
        public async Task GanhoService_ConsultarGanhos_ComIdCliente_Invalido()
        {
            //Arrange
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var valor = 17283893.123M;
            var nomeGanho = "dsuhahusdsuha";
            var ganho = new Ganho
                {Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganho1 = new Ganho
                {Id = id1, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false};
            var ganho2 = new Ganho
                {Id = id2, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true};
            var ganho3 = new Ganho
                {Id = id3, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false};
            var listGanho = new List<Ganho> {ganho, ganho1, ganho2, ganho3};
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarGanhos(idCliente));
        }

        [Fact]
        public async Task GanhoService_ExcluirGanho_ComSucesso()
        {
            //Arrage
            var id = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var motivoExclusao = "ASDASDAD";
            var excludeRequest = new ExcluirGanhoViewModel.Request
                {IdCliente = idCliente, IdGanho = id, MotivoExclusao = motivoExclusao};
            _ganhoRepository.ExcluirGanho(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();
            //Act
            var response = await services.ExcluirGanho(excludeRequest);
            //Assert
            Assert.IsType<ExcluirGanhoViewModel.Response>(response);
            Assert.NotNull(response);
            Assert.NotNull(response.Mensagem);
            Assert.NotEqual(400, response.StatusCode);
            Assert.False(response.DataExclusao.CompareTo(DateTime.Today) == 0);
            Assert.Equal("Excluido Com Sucesso", response.Mensagem);
            Assert.Equal(200, response.StatusCode);
        }
    }
}