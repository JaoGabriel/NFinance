using Xunit;
using System;
using NFinance.Domain;
using System.Threading.Tasks;
using Moq;
using NFinance.Application;
using NFinance.Application.ViewModel.ClientesViewModel;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Identidade;
using NFinance.Application.Exceptions;

namespace NFinance.Tests.Application
{
    public class ClienteAppTests
    {
        private readonly Mock<IClienteRepository> _clienteRepository;
        private readonly Mock<IUsuarioRepository> _usuarioRepository;
        private readonly Guid _id = Guid.NewGuid();
        private readonly string _nome = "joaquin da zils";
        private readonly string _cpf = "07944856930";
        private readonly string _email = "teste@teste.com";
        private readonly string _senha = "12391ukla";
        private readonly string _celular = "41984654879";

        public ClienteAppTests()
        {
            _clienteRepository = new Mock<IClienteRepository>();
            _usuarioRepository = new Mock<IUsuarioRepository>();
        }

        public ClienteApp IniciaApplication()
        {
            return new(_clienteRepository.Object, _usuarioRepository.Object);
        }

        private static Usuario GerarUsuario()
        {
            return new() { Id = Guid.NewGuid(), Email = "teste@teste.com", PasswordHash = "123456" };
        }

        [Fact]
        public async Task ClienteApp_CadastroCliente_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(_nome,_cpf,_email, _celular);
            var cadastroClienteVm = new CadastrarClienteViewModel.Request(cliente);
            _usuarioRepository.Setup(x => x.CadastrarUsuario(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(GerarUsuario());
            _clienteRepository.Setup(x => x.CadastrarCliente(It.IsAny<Cliente>())).ReturnsAsync(cliente);
            var app = IniciaApplication();

            //Act
            var response = await app.CadastrarCliente(cadastroClienteVm);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<CadastrarClienteViewModel.Response>(response);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.Cpf.ToString(), response.Cpf);
            Assert.Equal(cliente.Email.ToString(), response.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_CadastroCliente_ComNome_Invalido(string nome)
        {
            //Arrange
            var cadastroClienteVm = new CadastrarClienteViewModel.Request { Id = _id,Nome = nome, Cpf = _cpf,Email = _email, Senha = _senha };
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarCliente(cadastroClienteVm));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_CadastroCliente_ComCPF_Invalido(string cpf)
        {
            //Arrange
            var cadastroClienteVM = new CadastrarClienteViewModel.Request { Id = _id,Nome = _nome, Cpf = cpf,Email = _email, Senha = _senha };
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_CadastroCliente_ComEmail_Invalido(string email)
        {
            //Arrange
            var cadastroClienteVM = new CadastrarClienteViewModel.Request { Id = _id,Nome = _nome, Cpf = _cpf,Email = email, Senha = _senha };
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_CadastroCliente_ComSenha_Invalida(string senha)
        {
            //Arrange
            var cadastroClienteVM = new CadastrarClienteViewModel.Request { Id = _id,Nome = _nome, Cpf = _cpf,Email = _email, Senha = senha };
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarCliente(cadastroClienteVM));
        }
        
        [Fact]
        public async Task ClienteApp_AtualizarCliente_ComSucesso()
        {
            //Arrange
            var cliente = new Cliente(_nome, _cpf, _email, _celular);
            var atualizarClienteVM = new AtualizarClienteViewModel.Request(cliente);
            _clienteRepository.Setup(x => x.AtualizarCliente(It.IsAny<Cliente>())).ReturnsAsync(cliente);
            var app = IniciaApplication();

            //Act
            var response = await app.AtualizarDadosCadastrais(cliente.Id,atualizarClienteVM);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<AtualizarClienteViewModel.Response>(response);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.Cpf.ToString(), response.Cpf);
            Assert.Equal(cliente.Email.ToString(), response.Email);
        }
        
        [Fact]
        public async Task ClienteApp_AtualizarCliente_ComIdCliente_Invalido()
        {
            //Arrange
            var atualizarClienteVM = new AtualizarClienteViewModel.Request { Id = Guid.Empty,Nome = _nome, Cpf = _cpf,Email = _email, Senha = _senha };
            var app = IniciaApplication();

            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.AtualizarDadosCadastrais(atualizarClienteVM.Id, atualizarClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComNome_Invalido(string nome)
        {
            //Arrange
            var atualizarClienteVM = new AtualizarClienteViewModel.Request { Id = _id,Nome = nome, Cpf = _cpf,Email = _email, Senha = _senha };
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<DomainException>(() => app.AtualizarDadosCadastrais(atualizarClienteVM.Id, atualizarClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComCPF_Invalido(string cpf)
        {
            //Arrange
            var atualizarClienteVM = new AtualizarClienteViewModel.Request { Id = _id,Nome = _nome, Cpf = cpf,Email = _email, Senha = _senha };
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<DomainException>(() => app.AtualizarDadosCadastrais(atualizarClienteVM.Id, atualizarClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComEmail_Invalido(string email)
        {
            //Arrange
            var atualizarClienteVM = new AtualizarClienteViewModel.Request { Id = _id,Nome = _nome, Cpf = _cpf,Email = email, Senha = _senha };
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<DomainException>(() => app.AtualizarDadosCadastrais(atualizarClienteVM.Id, atualizarClienteVM));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task ClienteApp_AtualizarCliente_ComSenha_Invalida(string senha)
        {
            //Arrange
            var atualizarClienteVM = new AtualizarClienteViewModel.Request { Id = _id,Nome = _nome, Cpf = _cpf,Email = _email, Senha = senha };
            var app = IniciaApplication();

            //Act
            await Assert.ThrowsAsync<DomainException>(() => app.AtualizarDadosCadastrais(atualizarClienteVM.Id, atualizarClienteVM));
        }

        [Fact]
        public async Task ClienteApp_ConsultaCliente_ComSucesso()
        {
            //Arrage
            var cliente = new Cliente(_nome,_cpf,_email, _celular);
            _clienteRepository.Setup(x => x.ConsultarCliente(It.IsAny<Guid>())).ReturnsAsync(cliente);
            var app = IniciaApplication();
            
            //Act
            var response = await app.ConsultaCliente(cliente.Id);
            
            //Assert
            Assert.NotNull(response);
            Assert.IsType<ConsultarClienteViewModel.Response>(response);
            Assert.Equal(cliente.Nome, response.Nome);
            Assert.Equal(cliente.Cpf.ToString(), response.Cpf);
            Assert.Equal(cliente.Email.ToString(), response.Email);
        }

        [Fact]
        public async Task ClienteApp_ConsultaCliente_ComId_Invalido()
        {
            //Arrage
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<ClienteException>(() => app.ConsultaCliente(Guid.Empty));
        }
    }
}
