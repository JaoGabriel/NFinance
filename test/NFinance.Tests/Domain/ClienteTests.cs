using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Identidade;
using Xunit;

namespace NFinance.Tests.Domain
{
    public class ClienteTests
    {
        private readonly string _cpf;
        private readonly string _email;
        private readonly string _celular;
        private readonly string _nome;
        
        public ClienteTests()
        {
            _nome = "Vitória Daniela da Mata";
            _cpf = "245.227.196-98";
            _email = "vitoriadanieladamata@live.co.uk";
            _celular = "(86) 98241-7606";
        }

        [Theory]
        [InlineData("Vitória Daniela da Mata","245.227.196-98",null,"(86) 98241-7606")]
        [InlineData("Vitória Daniela da Mata","245.227.196-98","vitoriadanieladamata@live.co.uk",null)]
        [InlineData("Vitória Daniela da Mata",null,"vitoriadanieladamata@live.co.uk","(86) 98241-7606")]
        [InlineData(" ","245.227.196-98","vitoriadanieladamata@live.co.uk","(86) 98241-7606")]
        [InlineData("","245.227.196-98","vitoriadanieladamata@live.co.uk","(86) 98241-7606")]
        [InlineData(null,"245.227.196-98","vitoriadanieladamata@live.co.uk","(86) 98241-7606")]
        public void DadoUmClienteComDadosInvalidos_QuandoCadastrarCliente_DeveRetornarException(string nome,string cpf,string email, string celular)
        {
            //Arrange
            var cliente = new Cliente();

            //Assert
            Assert.Throws<DomainException>(() => cliente.CadastrarCliente(nome, cpf, email, celular));
        }

        [Fact]
        public void DadoUmClienteComDadosValidos_QuandoCadastrarCliente_DeveCadastrarClienteComsucesso()
        {
            //Arrange
            var cliente = new Cliente();

            //Act
            cliente.CadastrarCliente(_nome, _cpf, _email, _celular);
            
            //Assert
            Assert.NotNull(cliente);
            Assert.Equal(_nome,cliente.Nome);
            Assert.Equal(_cpf,cliente.Cpf.ToString());
            Assert.Equal(_email,cliente.Email.ToString());
            Assert.Equal(_celular,cliente.Celular.ToString());
        }

        [Fact]
        public void DadoUmClienteInvalido_QuandoAtribuirUsuario_DeveRetornarException()
        {
            //Arrange
            var cliente = new Cliente();
            
            //Assert
            Assert.Throws<DomainException>(() => cliente.AtruibiUsuarioCliente(null));
        }
        
        [Fact]
        public void DadoUmClienteValido_QuandoAtribuirUsuario_DeveAtribuirUsuarioComsucesso()
        {
            //Arrange
            var cliente = new Cliente();
            var usuario = new Usuario(cliente);
            cliente.CadastrarCliente(_nome, _cpf, _email, _celular);

            //Act
            cliente.AtruibiUsuarioCliente(usuario);
            
            //Assert
            Assert.NotNull(cliente);
            Assert.NotNull(cliente.Usuario);
        }
        
        [Theory]
        [InlineData("Vitória Daniela da Mata","245.227.196-98",null,"(86) 98241-7606")]
        [InlineData("Vitória Daniela da Mata","245.227.196-98","vitoriadanieladamata@live.co.uk",null)]
        [InlineData("Vitória Daniela da Mata",null,"vitoriadanieladamata@live.co.uk","(86) 98241-7606")]
        [InlineData(" ","245.227.196-98","vitoriadanieladamata@live.co.uk","(86) 98241-7606")]
        [InlineData("","245.227.196-98","vitoriadanieladamata@live.co.uk","(86) 98241-7606")]
        [InlineData(null,"245.227.196-98","vitoriadanieladamata@live.co.uk","(86) 98241-7606")]
        public void DadoUmClienteComDadosInvalidos_QuandoAtualizarDados_DeveRetornarException(string nome,string cpf,string email, string celular)
        {
            //Arrange
            var cliente = new Cliente();

            //Assert
            Assert.Throws<DomainException>(() => cliente.CadastrarCliente(nome, cpf, email, celular));
        }

        [Fact]
        public void DadoUmClienteComDadosValidos_QuandoAtualizarDados_DeveCadastrarClienteComsucesso()
        {
            //Arrange
            var novoNome = "Letícia Isabel Joana Moura";
            var novoEmail = "leticiaisabeljoanamoura@truran.com.br";
            var novoCpf = "531.042.868-25";
            var novoCelular = "(82) 99324-3188";
            var cliente = new Cliente();
            cliente.CadastrarCliente(_nome, _cpf, _email, _celular);

            //Act
            cliente.AtualizarDadosCliente(novoNome,novoCpf,novoEmail,novoCelular);
            
            //Assert
            Assert.NotNull(cliente);
            Assert.NotEqual(_nome,cliente.Nome);
            Assert.NotEqual(_cpf,cliente.Cpf.ToString());
            Assert.NotEqual(_email,cliente.Email.ToString());
            Assert.NotEqual(_celular,cliente.Celular.ToString());
        }
    }
}