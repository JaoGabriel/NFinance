using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Cliente;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NFinance.Domain.ViewModel.ClientesViewModel;
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
            var cpf = "123.654.987-96";
            var email = "teste@teste.com";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome, Cpf = cpf,Email = email};
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = nome, CPF = cpf,Email = email });
            var services = InicializaServico();

            //Act
            var response = await services.CadastrarCliente(clienteRequest);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotNull(response.Cpf);
            Assert.NotNull(response.Email);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(id, response.Id);
            Assert.Equal(nome, response.Nome);
            Assert.Equal(cpf, response.Cpf);
            Assert.Equal(email, response.Email);
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNome_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var cpf = "123.654.987-96";
            var email = "teste@teste.com";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = null, Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = null, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNome_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var cpf = "123.654.987-96";
            var email = "teste@teste.com";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = "", Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = "", CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComNome_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var cpf = "123.654.987-96";
            var email = "teste@teste.com";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = " ", Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = " ", CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }

        [Fact]
        public async Task ClienteService_CadastrarCliente_ComCPF_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "";
            var email = "teste@teste.com";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComCPF_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "  ";
            var email = "teste@teste.com";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComCPF_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var email = "teste@teste.com";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome,  Cpf = null,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = null,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComEmail_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "123.654.987-96";
            var email = "";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComEmail_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "123.654.987-96";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = null };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = null });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_CadastrarCliente_ComEmail_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "123.654.987-96";
            var email = "  ";
            var clienteRequest = new CadastrarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.CadastrarCliente(clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var cpf = "123.654.987-96";
            var email = "josevaldo@teste.com";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = "  ", Cpf = cpf,Email = email };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = "  ", CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var cpf = "123.654.987-96";
            var email = "josevaldo@teste.com";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = null, Cpf = cpf,Email = email };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = null, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<NomeClienteException>(() => /*Act*/ services.AtualizarCliente(id, clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComNome_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var cpf = "123.654.987-96";
            var email = "josevaldo@teste.com";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = "", Cpf = cpf,Email = email };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = "", CPF = cpf,Email = email });
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
            var cpf = "123.654.987-96";
            var email = "josevaldo@teste.com";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome, Cpf = cpf,Email = email };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(), Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = nome, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<IdException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComCPF_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "";
            var email = "teste@teste.com";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComCPF_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "  ";
            var email = "teste@teste.com";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComCPF_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var email = "teste@teste.com";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome,  Cpf = null,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = null,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<CpfClienteException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComEmail_Vazio()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "123.654.987-96";
            var email = "";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComEmail_Nulo()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "123.654.987-96";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = null };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = null });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }
        
        [Fact]
        public async Task ClienteService_AtualizarCliente_ComEmail_EmBranco()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "Jubileu da silva";
            var cpf = "123.654.987-96";
            var email = "  ";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome,  Cpf = cpf,Email = email };
            _clienteRepository.CadastrarCliente(Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, CPF = cpf,Email = email });
            var services = InicializaServico();
            //Assert
            await Assert.ThrowsAsync<EmailClienteException>(() => /*Act*/ services.AtualizarCliente(id,clienteRequest));
        }

        [Fact]
        public async Task ClienteService_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var nome = "AtualizaTeste@Sucesso";
            var cpf = "123.654.987-96";
            var email = "carlistes@teste.com";
            var clienteRequest = new AtualizarClienteViewModel.Request() { Nome = nome, Cpf = cpf,Email = email };
            _clienteRepository.AtualizarCliente(Arg.Any<Guid>(),Arg.Any<Cliente>()).Returns(new Cliente() { Id = id, Nome = nome, CPF = cpf,Email = email });
            var services = InicializaServico();
            
            //Act
            var response = await services.AtualizarCliente(id,clienteRequest);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotNull(response.Cpf);
            Assert.NotNull(response.Email);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(id, response.Id);
            Assert.Equal(nome, response.Nome);
            Assert.Equal(cpf, response.Cpf);
            Assert.Equal(email, response.Email);
        }

        [Fact]
        public async Task ClienteService_ConsultarCliente_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var cpf = "123.123.123.11";
            var email = "claudisney@teste.com";
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(new Cliente() { Id = id, Nome = "ConsultaTeste@Sucesso", CPF = cpf, Email = email});
            var services = InicializaServico();

            //Act
            var response = await services.ConsultarCliente(id);

            //Assert
            Assert.NotNull(response.Nome);
            Assert.NotNull(response.Cpf);
            Assert.NotNull( response.Email);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(id, response.Id);
            Assert.Equal(cpf, response.Cpf);
            Assert.Equal(email, response.Email);
        }

        [Fact]
        public async Task ClienteService_ConsultarCliente_ComId_Vazio()
        {
            //Arrange
            var id = Guid.Empty;
            var cpf = "123.123.123.11";
            var email = "claudisney@teste.com";
            _clienteRepository.ConsultarCliente(Arg.Any<Guid>()).Returns(new Cliente() { Id = id, Nome = "ConsultaCliente", CPF = cpf, Email = email });
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
            var cpf = "123.123.123.11";
            var email = "claudisney@teste.com";
            var listaClientes = new List<Cliente>();
            var cliente = new Cliente() { Id = id, Nome = nome, CPF = cpf, Email = email };
            listaClientes.Add(cliente);
            _clienteRepository.ListarClientes().Returns(new List<Cliente>(listaClientes));
            var services = InicializaServico();

            //Act
            var response = await services.ListarClientes();

            //Assert
            Assert.True(response.Count > 0);
            Assert.NotNull(response.Find(c => c.Id == id).Nome);
            Assert.NotNull(response.Find(c => c.Id == id).Cpf);
            Assert.NotNull(response.Find(c => c.Id == id).Email);
            Assert.NotEqual(Guid.Empty, response.Find(c => c.Id == id).Id);
            Assert.Equal(id, response.Find(c => c.Id == id).Id);
            Assert.Equal(nome, response.Find(c => c.Id == id).Nome);
            Assert.Equal(cpf, response.Find(c => c.Id == id).Cpf);
            Assert.Equal(email, response.Find(c => c.Id == id).Email);
        }
    }
}
