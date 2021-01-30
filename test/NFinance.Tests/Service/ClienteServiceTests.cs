using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Cliente;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Services;
using NFinance.Model.ClientesViewModel;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.Service
{
    public class ClienteServiceTests
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteServiceTests()
        {
            _clienteRepository = Substitute.For<IClienteRepository>();
        }

        public ClienteService InicializaServico()
        {
            return new ClienteService(_clienteRepository);
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComSucesso()
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
        public async Task ClienteService_CadastrarCliente_ComNome_Nulo()
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

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNome_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            decimal rendaMensal = 758792.311M;
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = "", RendaMensal = rendaMensal };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = "", RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNome_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            decimal rendaMensal = 753232.111M;
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = " ", RendaMensal = rendaMensal };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = " ", RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComRendaMensal_Zero()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            decimal rendaMensal = 0;
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome, RendaMensal = rendaMensal };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = nome, RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<RendaMensalException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            decimal rendaMensal = 753232.111M;
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = "  ", RendaMensal = rendaMensal };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = "  ", RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            decimal rendaMensal = 753232.111M;
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = null, RendaMensal = rendaMensal };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = null, RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(id, clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            decimal rendaMensal = 753232.111M;
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = "", RendaMensal = rendaMensal };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = "", RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(id, clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComId_Vazio()
        {
            //Arrange
            var id = Guid.Empty;
            var nome = "AtualizaTeste";
            decimal rendaMensal = 753232.111M;
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome, RendaMensal = rendaMensal };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = nome, RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComRendaMensal_Zero()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "AtualizaTeste";
            decimal rendaMensal = 0;
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome, RendaMensal = rendaMensal };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = nome, RendaMensal = rendaMensal });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<RendaMensalException>(() => /*Act*/ services.AtualizarCliente(id, clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "AtualizaTeste@Sucesso";
            decimal rendaMensal = 7532.111M;
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome, RendaMensal = rendaMensal };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(),Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = nome, RendaMensal = rendaMensal });
            var services = InicializaServico();
            
            //Act
            var response = await services.AtualizarCliente(id,clienteRequest);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotEqual(0, response.RendaMensal);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(id, response.Id);
            Assert.Equal(nome, response.Nome);
            Assert.Equal(rendaMensal, response.RendaMensal);
        }

        [Fact]
        public async Task ClienteService_ConsultarCliente_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(new Cliente() { Id = id, Nome = "ConsultaTeste@Sucesso", RendaMensal = 90342.11M });
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarCliente(id);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotEqual(0, response.RendaMensal);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(id, response.Id);
        }

        [Fact]
        public async Task ClienteService_ConsultarCliente_ComId_Vazio()
        {
            //Arrange
            var id = Guid.Empty;
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(new Cliente() { Id = id, Nome = "ConsultaCliente", RendaMensal = 756487.21M });
            var services = InicializaServico();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.ConsultarCliente(id));
        }

        [Fact]
        public async Task ClienteService_ListarClientes_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Claudinho Teste";
            decimal rendaMensal = 31728317923.13M;
            var listaClientes = new List<Cliente>();
            var cliente = new Cliente() { Id = id, Nome = nome, RendaMensal = rendaMensal};
            listaClientes.Add(cliente);
            _clienteRepository.ListarClientes().Returns(new List<Cliente>(listaClientes));
            var services = InicializaServico();

            //Act
            var response = await services.ListarClientes();

            //Assert
            Assert.True(response.Count > 0);
            Assert.NotNull(response.Find(c => c.Id == id).Nome);
            Assert.NotEqual(0, response.Find(c => c.Id == id).RendaMensal);
            Assert.NotEqual(Guid.Empty, response.Find(c => c.Id == id).Id);
            Assert.Equal(id, response.Find(c => c.Id == id).Id);
            Assert.Equal(nome, response.Find(c => c.Id == id).Nome);
            Assert.Equal(rendaMensal, response.Find(c => c.Id == id).RendaMensal);
        }
    }
}
