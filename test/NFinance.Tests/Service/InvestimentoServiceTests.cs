using Xunit;
using System;
using System.Linq;
using NSubstitute;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Domain.Services;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Exceptions.Investimento;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.Tests.Service
{
    public class InvestimentoServiceTests
    {
        private readonly IInvestimentoRepository _investimentosRepository;

        public InvestimentoServiceTests()
        {
            _investimentosRepository = Substitute.For<IInvestimentoRepository>();
        }

        public InvestimentoService InicializaServico()
        {
            return new InvestimentoService(_investimentosRepository);
        }

        public Investimento GeraInvestimento()
        {
            return new Investimento(Guid.NewGuid(),"CDB AAAA",378129837.1823M,DateTime.Today);
        }

        [Fact]
        public async Task InvestimentoService_RealizarInvestimento_ComSucesso()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.RealizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Act
            var response = await services.RealizarInvestimento(investimento);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeInvestimento);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual("00/00/00", response.DataAplicacao.ToString());
            //Assert.Equal(id, response.Id);
            //Assert.Equal(nomeInvestimento, response.NomeInvestimento);
            //Assert.Equal(data, response.DataAplicacao);
            //Assert.Equal(valorInvestido, response.Valor);
            //Assert.Equal(idCliente, response.Cliente.Id);
            //Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComSucesso()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Act
            var response = await services.AtualizarInvestimento(investimento);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeInvestimento);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual("00/00/00", response.DataAplicacao.ToString());
            //Assert.Equal(id, response.Id);
            //Assert.Equal(nomeInvestimento, response.NomeInvestimento);
            //Assert.Equal(data, response.DataAplicacao);
            //Assert.Equal(valorInvestido, response.Valor);
            //Assert.Equal(idCliente, response.Cliente.Id);
            //Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async Task InvestimentoService_RealizarInvestimento_ComIdCliente_Invalido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.RealizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.RealizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_RealizarInvestimento_ComNomeInvestimento_Vazio()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.RealizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeInvestimentoException>(() => /*Act*/ services.RealizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_RealizarInvestimento_ComNomeInvestimento_EmBranco()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.RealizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeInvestimentoException>(() => /*Act*/ services.RealizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_RealizarInvestimento_ComNomeInvestimento_Nulo()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.RealizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeInvestimentoException>(() => /*Act*/ services.RealizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_RealizarInvestimento_ComValor_Invalido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.RealizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorInvestimentoException>(() => /*Act*/ services.RealizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_RealizarInvestimento_ComDataAplicacao_Invalido_Maior_Permitido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.RealizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataInvestimentoException>(() => /*Act*/ services.RealizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_RealizarInvestimento_ComDataAplicacao_Invalido_Menor_Permitido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.RealizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);           
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataInvestimentoException>(() => /*Act*/ services.RealizarInvestimento(investimento));
        }


        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComIdInvestimento_Invalido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComIdCliente_Invalido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComNomeInvestimento_Nulo()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeInvestimentoException>(() => /*Act*/ services.AtualizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComNomeInvestimento_Vazio()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeInvestimentoException>(() => /*Act*/ services.AtualizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComNomeInvestimento_EmBranco()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<NomeInvestimentoException>(() => /*Act*/ services.AtualizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComValor_Invalido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<ValorInvestimentoException>(() => /*Act*/ services.AtualizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComDataAplicacao_Menor_Permitido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataInvestimentoException>(() => /*Act*/ services.AtualizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_AtualizarInvestimento_ComDataAplicacao_Maior_Permitido()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.AtualizarInvestimento(Arg.Any<Investimento>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<DataInvestimentoException>(() => /*Act*/ services.AtualizarInvestimento(investimento));
        }

        [Fact]
        public async Task InvestimentoService_ConsultarInvestimento_ComSucesso()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.ConsultarInvestimento(Arg.Any<Guid>()).Returns(investimento);
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarInvestimento(investimento.Id);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.NomeInvestimento);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotEqual(0, response.Valor);
            Assert.NotEqual("00/00/00", response.DataAplicacao.ToString());
            //Assert.Equal(id, response.Id);
            //Assert.Equal(nomeInvestimento, response.NomeInvestimento);
            //Assert.Equal(data, response.DataAplicacao);
            //Assert.Equal(valorInvestido, response.Valor);
            //Assert.Equal(idCliente, response.Cliente.Id);
            //Assert.Equal(nomeCliente, response.Cliente.Nome);
        }

        [Fact]
        public async Task InvestimentoService_ConsultarInvestimento_ComId_Vazio()
        {
            //Arrange
            var investimento = GeraInvestimento();
            _investimentosRepository.ConsultarInvestimento(Arg.Any<Guid>()).Returns(investimento);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarInvestimento(investimento.Id));
        }

        [Fact]
        public async Task InvestimetoService_ConsultarInvestimentos_ComSucesso()
        {
            //Arrage
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var idCliente = Guid.NewGuid();
            var valor = 120245.21M;
            var nomeInvestimento = "TEsteee";
            var dataAplicacao = DateTime.Today;
            var investimento = new Investimento { Id = id, IdCliente = idCliente, Valor = valor, NomeInvestimento = nomeInvestimento, DataAplicacao = dataAplicacao };
            var investimento1 = new Investimento { Id = id1, IdCliente = idCliente, Valor = valor, NomeInvestimento = nomeInvestimento, DataAplicacao = dataAplicacao };
            var investimento2 = new Investimento { Id = id2, IdCliente = idCliente, Valor = valor, NomeInvestimento = nomeInvestimento, DataAplicacao = dataAplicacao };
            var investimento3 = new Investimento { Id = id3, IdCliente = idCliente, Valor = valor, NomeInvestimento = nomeInvestimento, DataAplicacao = dataAplicacao };
            var listInvestimentos = new List<Investimento> { investimento, investimento1, investimento2, investimento3 };
            _investimentosRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimentos);
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarInvestimentos(idCliente);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(4, response.Count);

            //Assert dos ganhos do cliente - investimento 0
            var responseTeste = response.FirstOrDefault(g => g.Id == id);
            Assert.IsType<InvestimentoViewModel>(responseTeste);
            Assert.Equal(id, responseTeste.Id);
            Assert.Equal(idCliente, responseTeste.IdCliente);
            Assert.Equal(nomeInvestimento, responseTeste.NomeInvestimento);
            Assert.Equal(valor, responseTeste.Valor);
            Assert.Equal(dataAplicacao, responseTeste.DataAplicacao);

            //Assert dos ganhos do cliente - investimento 1
            var responseTeste1 = response.FirstOrDefault(g => g.Id == id1);
            Assert.IsType<InvestimentoViewModel>(responseTeste1);
            Assert.Equal(id1, responseTeste1.Id);
            Assert.Equal(idCliente, responseTeste1.IdCliente);
            Assert.Equal(nomeInvestimento, responseTeste1.NomeInvestimento);
            Assert.Equal(valor, responseTeste1.Valor);
            Assert.Equal(dataAplicacao, responseTeste1.DataAplicacao);

            //Assert dos ganhos do cliente - investimento 2
            var responseTeste2 = response.FirstOrDefault(g => g.Id == id2);
            Assert.IsType<InvestimentoViewModel>(responseTeste2);
            Assert.Equal(id2, responseTeste2.Id);
            Assert.Equal(idCliente, responseTeste2.IdCliente);
            Assert.Equal(nomeInvestimento, responseTeste2.NomeInvestimento);
            Assert.Equal(valor, responseTeste2.Valor);
            Assert.Equal(dataAplicacao, responseTeste2.DataAplicacao);

            //Assert dos ganhos do cliente - investimento 3
            var responseTeste3 = response.FirstOrDefault(g => g.Id == id3);
            Assert.IsType<InvestimentoViewModel>(responseTeste3);
            Assert.Equal(id3, responseTeste3.Id);
            Assert.Equal(idCliente, responseTeste3.IdCliente);
            Assert.Equal(nomeInvestimento, responseTeste3.NomeInvestimento);
            Assert.Equal(valor, responseTeste3.Valor);
            Assert.Equal(dataAplicacao, responseTeste3.DataAplicacao);
        }

        [Fact]
        public async Task InvestimetoService_ConsultarInvestimentos_ComIdCliente_Invalido()
        {
            //Arrage
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var idCliente = Guid.Empty;
            var valor = 120245.21M;
            var nomeInvestimento = "TEsteee";
            var dataAplicacao = DateTime.Today;
            var investimento = new Investimento { Id = id, IdCliente = idCliente, Valor = valor, NomeInvestimento = nomeInvestimento, DataAplicacao = dataAplicacao };
            var investimento1 = new Investimento { Id = id1, IdCliente = idCliente, Valor = valor, NomeInvestimento = nomeInvestimento, DataAplicacao = dataAplicacao };
            var investimento2 = new Investimento { Id = id2, IdCliente = idCliente, Valor = valor, NomeInvestimento = nomeInvestimento, DataAplicacao = dataAplicacao };
            var investimento3 = new Investimento { Id = id3, IdCliente = idCliente, Valor = valor, NomeInvestimento = nomeInvestimento, DataAplicacao = dataAplicacao };
            var listInvestimentos = new List<Investimento> { investimento, investimento1, investimento2, investimento3 };
            _investimentosRepository.ConsultarInvestimentos(Arg.Any<Guid>()).Returns(listInvestimentos);
            var services = InicializaServico();

            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarInvestimentos(idCliente));
        }
    }
}
