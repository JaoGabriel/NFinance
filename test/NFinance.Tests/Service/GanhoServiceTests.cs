using Xunit;
using System;
using System.Linq;
using NSubstitute;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Domain.Services;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Ganho;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.GanhoViewModel;

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

        public static Ganho GeraGanho()
        {
            return new Ganho(Guid.NewGuid(), "Ganho 1", 102923.123M, false, DateTime.Today);
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComSucesso()
        {
            //Arrage
            var ganho = GeraGanho();
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();
            //Act
            var response = await services.CadastrarGanho(ganho);
            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGanho);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.NotEqual("00/00/00", response.DataDoGanho.ToString());
            Assert.Equal(ganho.Id, response.Id);
            Assert.Equal(ganho.IdCliente, response.IdCliente);
            Assert.Equal(ganho.NomeGanho, response.NomeGanho);
            Assert.Equal(ganho.Valor, response.Valor);
            Assert.False(response.Recorrente);
            Assert.Equal(ganho.DataDoGanho, response.DataDoGanho);
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComIdCliente_Invalido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.IdCliente = Guid.Empty;
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.CadastrarGanho(ganho));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GanhoService_CadastrarGanho_ComNomeGanho_Invalido(string nomeGanho)
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.NomeGanho = nomeGanho;
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => /*Act*/ services.CadastrarGanho(ganho));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-151231)]
        public async Task GanhoService_CadastrarGanho_ComValorGanho_Invalido(decimal valor)
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.Valor = valor;
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorGanhoException>(() => /*Act*/ services.CadastrarGanho(ganho));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComDataAplicacao_Invalida_Maior_Permitido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.DataDoGanho = DateTime.Today.AddYears(120);
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => /*Act*/ services.CadastrarGanho(ganho));
        }

        [Fact]
        public async Task GanhoService_CadastrarGanho_ComDataAplicacao_Invalida_Menor_Permitido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.DataDoGanho = DateTime.Today.AddYears(-120);
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => /*Act*/ services.CadastrarGanho(ganho));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComSucesso()
        {
            //Arrage
            var ganho = GeraGanho();
            ganho.Recorrente = true;
            _ganhoRepository.AtualizarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();
            //Act
            var response = await services.AtualizarGanho(ganho);
            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGanho);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.Equal(ganho.DataDoGanho, response.DataDoGanho);
            Assert.Equal(ganho.Id, response.Id);
            Assert.Equal(ganho.IdCliente, response.IdCliente);
            Assert.Equal(ganho.NomeGanho, response.NomeGanho);
            Assert.Equal(ganho.Valor, response.Valor);
            Assert.True(response.Recorrente);
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComIdGanho_Invalido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.Id = Guid.Empty;
            _ganhoRepository.AtualizarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGanho(ganho));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComIdCliente_Invalido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.IdCliente = Guid.Empty;
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarGanho(ganho));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GanhoService_AtualizarGanho_ComNomeGanho_Invalido(string nome)
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.NomeGanho = nome;
            _ganhoRepository.AtualizarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => /*Act*/ services.AtualizarGanho(ganho));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-15445321)]
        public async Task GanhoService_AtualizarGanho_ComValorGanho_Negativo(decimal valor)
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.Valor = valor;
            _ganhoRepository.AtualizarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorGanhoException>(() => /*Act*/ services.AtualizarGanho(ganho));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComDataAplicacao_Invalido_Maior_Permitido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.DataDoGanho = DateTime.Today.AddYears(120);
            _ganhoRepository.AtualizarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => /*Act*/ services.AtualizarGanho(ganho));
        }

        [Fact]
        public async Task GanhoService_AtualizarGanho_ComDataAplicacao_Invalido_Menor_Permitido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.DataDoGanho = DateTime.Today.AddYears(-120);
            _ganhoRepository.AtualizarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => /*Act*/ services.AtualizarGanho(ganho));
        }

        [Fact]
        public async Task GanhoService_ConsultarGanho_ComSucesso()
        {
            //Arrage
            var ganho = GeraGanho();
            _ganhoRepository.ConsultarGanho(Arg.Any<Guid>()).Returns(ganho);
            var services = InicializaServico();
            //Act
            var response = await services.ConsultarGanho(ganho.Id);
            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeGanho);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(Guid.Empty, response.IdCliente);
            Assert.Equal(ganho.Id, response.Id);
            Assert.Equal(ganho.IdCliente, response.IdCliente);
            Assert.Equal(ganho.NomeGanho, response.NomeGanho);
            Assert.Equal(ganho.Valor, response.Valor);
            Assert.False(response.Recorrente);
        }

        [Fact]
        public async Task GanhoService_ConsultarGanho_ComIdGanho_Invalido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.Id = Guid.Empty;
            _ganhoRepository.ConsultarGanho(Arg.Any<Guid>()).Returns(ganho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarGanho(ganho.Id));
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
            var ganho = new Ganho { Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true };
            var ganho1 = new Ganho { Id = id1, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false };
            var ganho2 = new Ganho { Id = id2, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false };
            var ganho3 = new Ganho { Id = id3, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false };
            var listGanho = new List<Ganho> { ganho, ganho1, ganho2, ganho3 };
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();
            //Act
            var response = await services.ConsultarGanhos(idCliente);
            //Assert
            Assert.NotNull(response);
            Assert.Equal(4, response.Count);
            //Assert dos ganhos do cliente - ganho 0
            var responseTeste = response.FirstOrDefault(g => g.Id == id);
            Assert.IsType<Ganho>(responseTeste);
            Assert.Equal(id, responseTeste.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(nomeGanho, responseTeste.NomeGanho);
            Assert.Equal(valor, responseTeste.Valor);
            Assert.True(responseTeste.Recorrente);
            //Assert dos ganhos do cliente - ganho 1
            var responseTeste1 = response.FirstOrDefault(g => g.Id == id1);
            Assert.IsType<Ganho>(responseTeste1);
            Assert.Equal(id1, responseTeste1.Id);
            Assert.Equal(idCliente, responseTeste1.IdCliente);
            Assert.Equal(nomeGanho, responseTeste1.NomeGanho);
            Assert.Equal(valor, responseTeste1.Valor);
            Assert.False(responseTeste1.Recorrente);
            //Assert dos ganhos do cliente - ganho 2
            var responseTeste2 = response.FirstOrDefault(g => g.Id == id2);
            Assert.IsType<Ganho>(responseTeste2);
            Assert.Equal(id2, responseTeste2.Id);
            Assert.Equal(idCliente, responseTeste2.IdCliente);
            Assert.Equal(nomeGanho, responseTeste2.NomeGanho);
            Assert.Equal(valor, responseTeste2.Valor);
            Assert.False(responseTeste2.Recorrente);
            //Assert dos ganhos do cliente - ganho 3
            var responseTeste3 = response.FirstOrDefault(g => g.Id == id3);
            Assert.IsType<Ganho>(responseTeste3);
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
            var ganho = new Ganho { Id = id, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true };
            var ganho1 = new Ganho { Id = id1, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false };
            var ganho2 = new Ganho { Id = id2, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = true };
            var ganho3 = new Ganho { Id = id3, IdCliente = idCliente, Valor = valor, NomeGanho = nomeGanho, Recorrente = false };
            var listGanho = new List<Ganho> { ganho, ganho1, ganho2, ganho3 };
            _ganhoRepository.ConsultarGanhos(Arg.Any<Guid>()).Returns(listGanho);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarGanhos(idCliente));
        }

        [Fact]
        public async Task GanhoService_ExcluirGanho_ComSucesso()
        {
            //Arrage
            var ganho = GeraGanho();
            _ganhoRepository.ExcluirGanho(Arg.Any<Guid>()).Returns(true);
            var services = InicializaServico();
            
            //Act
            var response = await services.ExcluirGanho(ganho);
            
            //Assert
            Assert.True(response);
        }

        [Fact]
        public async Task GanhoService_ExcluirGanhos_ComId_Invalido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.Id = Guid.Empty;
            _ganhoRepository.ExcluirGanho(Arg.Any<Guid>()).Returns(false);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ExcluirGanho(ganho));
        }

        [Fact]
        public async Task GanhoService_ExcluirGanhos_ComIdCliente_Invalido()
        {
            //Arrange
            var ganho = GeraGanho();
            ganho.IdCliente = Guid.Empty;
            _ganhoRepository.ExcluirGanho(Arg.Any<Guid>()).Returns(false);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ExcluirGanho(ganho));
        }
    }
}