using NFinance.Domain.ObjetosDeValor;
using Xunit;

namespace NFinance.Tests.ValueObjects
{
    public class CelularTests
    {
        [Theory]
        [InlineData("67994078898")]
        [InlineData("(23) 95856-6398")]
        [InlineData("(20)98437-0469")]
        [InlineData("(38) 1248-9328")]
        [InlineData("(36) 2096-1837")]
        public void DeveAtruibuirNumero_QuandoForUmNumeroDeTelefone(string numero)
        {
            var telefone = new Celular(numero);

            var numeroFormatado = numero
                .Replace(" ", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("-", string.Empty);

            var ddd = numeroFormatado.Substring(0, 2);
            string numeroTelefone;

            if (numeroFormatado.Length == 10)
                numeroTelefone = "9" + numeroFormatado.Remove(0, 2);
            else
                numeroTelefone = numeroFormatado.Remove(0, 2);

            Assert.Equal(telefone.Ddd, ddd);
            Assert.Equal(telefone.Numero, numeroTelefone);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void DeveLancarUmaDomainExcption_QuandoNumeroForBrancoNuloOuVazio(string numero)
        {
            Assert.Throws<CelularException>(() => new Celular(numero));
        }

        [Fact]
        public void Igualdade()
        {
            var numeroTelefone1 = "67994078898";
            var numeroTelefone2 = "67994078898";

            var numero1 = new Celular(numeroTelefone1);
            var numero2 = new Celular(numeroTelefone2);
            var numeroIgual = numero1 == numero2;

            Assert.True(numeroIgual);
            Assert.True(numero1.Equals(numero2));
            Assert.True(numero1.GetHashCode() == numero2.GetHashCode());
        }

        [Fact]
        public void DeveAtribuirNumero_QuandoForPassadoDddENumero()
        {
            var ddd = "41";
            var numero = "987456321";
            var telefone = new Celular(ddd, numero);

            Assert.Equal(telefone.Ddd, ddd);
            Assert.Equal(telefone.Numero, numero);
        }
    }
}