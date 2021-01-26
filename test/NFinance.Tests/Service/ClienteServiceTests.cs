using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Services;
using NFinance.Model.ClientesViewModel;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.Service
{
    public class ClienteServiceTests
    {
        private IClienteRepository _clienteRepository;

        public ClienteServiceTests()
        {
            _clienteRepository = Substitute.For<IClienteRepository>();
        }

        public ClienteService InicializaServico()
        {
            return new ClienteService(_clienteRepository);
        }

        [Fact]
        public async void ClienteService_CadastrarCliente_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Teste@Sucesso";
            decimal rendaMensal = 5000;
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome, RendaMensal = rendaMensal };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = nome, RendaMensal = rendaMensal });
            var services = InicializaServico();

            //Act
            var response = await services.CadastrarCliente(clienteRequest);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotEqual(0,response.RendaMensal);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(id, response.Id);
            Assert.Equal(nome, response.Nome);
            Assert.Equal(rendaMensal, response.RendaMensal);
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNomeInexistente()
        {
            //Arrange
            var id = Guid.NewGuid();
            decimal rendaMensal = 7523.11M;
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = null, RendaMensal = rendaMensal };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = null, RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }
    }
}
