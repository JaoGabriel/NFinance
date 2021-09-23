using NFinance.Domain.ObjetosDeValor;
using Xunit;

namespace NFinance.Tests.ValueObjects
{
    public class EmailTests
    {
        [Theory]
        [InlineData("joey.carter93@gmail.com")]
        [InlineData("bud_bahringer40@yahoo.com")]
        [InlineData("vilma43@hotmail.com")]
        [InlineData("norris_jaskolski40@gmail.com")]
        [InlineData("alguem@orgao.uf.gov.br")]
        public void DeveAtribuirUmEndereco_QuandoEnderecoForValido(string endereco)
        {
            var email = new Email(endereco);
            
            Assert.Equal(endereco,email.Endereco);
        }

        [Theory]
        [InlineData("teste")]
        [InlineData("@teste.com")]
        [InlineData("@testecom")]
        [InlineData("teste.teste.com")]
        [InlineData("@teste.com.br")]
        public void DeveLancarUmaDomainException_QuandoEmailForInvalido(string endereco)
        {
            Assert.Throws<EmailException>(() => new Email(endereco));
        }
        
        [Fact]
        public void Igualdade()
        {
            var endereco1 = "teste@teste.com";
            var endereco2 = "teste@teste.com";

            var email1 =  new Email(endereco1);
            var email2 =  new Email(endereco2);
            var emailsIguais = email1 == email2;

            Assert.True(emailsIguais);
            Assert.True(email1.Equals(email2));
            Assert.True(email1.GetHashCode() == email2.GetHashCode());
        }
    }
}