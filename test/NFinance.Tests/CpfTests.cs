using NFinance.Domain.ObjetosDeValor;
using Xunit;

namespace NFinance.Tests
{
    public class CpfTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DeveLancarUmaDomainException_QuandoNumeroCpfVazioNullOuEspacoEmBranco(string numeroCpf)
        {
            Assert.Throws<CpfException>(() => new Cpf(numeroCpf));
        }

        [Theory]
        [InlineData("123456789101")]
        [InlineData("1234567891")]
        public void DeveLancarUmaDomainException_QuandoForDiferenteDeOnzeCaracteres(string numeroCpf)
        {
            Assert.Throws<CpfException>(() => new Cpf(numeroCpf));
        }

        [Fact]
        public void DeveLancarUmaDomainException_QuandoForUmNumeroCpfInvalido()
        {
            var numeroCpfInvalido = "11111111111";
            Assert.Throws<CpfException>(() => new Cpf(numeroCpfInvalido));
        }

        [Theory]
        [InlineData("07944856930")]
        [InlineData("08031256900")]
        [InlineData("09857236707")]
        [InlineData("34915389049")]
        [InlineData("29787353840")]
        [InlineData("32314160860")]
        [InlineData("79956718904")]
        [InlineData("75529459900")]
        public void DeveAtribuirNumero_QuandoForUmNumeroCpfValido(string numeroCpfValido)
        {
            var cpf = new Cpf(numeroCpfValido);

            Assert.Equal(numeroCpfValido, cpf.Numero);
        }

        [Fact]
        public void DeveLancarUmaDomainException_QuandoPrimeiroDigitoVerificadorDoCpfInvalido()
        {
            var cpfComPrimeiroDigitoVerificadorInvalido = "32312312312";
            var cpfComPrimeiroDigitoVerificadorInvalidoComRestoMaiorQueUm = "04522139928";

            Assert.Throws<CpfException>(() => new Cpf(cpfComPrimeiroDigitoVerificadorInvalido));
            Assert.Throws<CpfException>(() => new Cpf(cpfComPrimeiroDigitoVerificadorInvalidoComRestoMaiorQueUm));
        }

        [Fact]
        public void DeveLancarUmaDomainException_QuandoSegundoDigitoVerificadorDoCpfInvalido()
        {
            var cpfComSegundoDigitoVerificadorInvalido = "48855856511";
            var cpfComSegundoDigitoVerificadorInvalidoComRestoMaiorQueUm = "04522139910";

            Assert.Throws<CpfException>(() => new Cpf(cpfComSegundoDigitoVerificadorInvalido));
            Assert.Throws<CpfException>(() => new Cpf(cpfComSegundoDigitoVerificadorInvalidoComRestoMaiorQueUm));
        }

        [Fact]
        public void DeveAtribuirNumero_QuandoForUmNumeroCpfFormato()
        {
            var numeroCpfFormatado = "311.164.321-29";
            var numeroCpfSemFormatacao = "31116432129";

            var cpf = new Cpf(numeroCpfFormatado);

            Assert.Equal(numeroCpfSemFormatacao, cpf.Numero);
        }

        [Fact]
        public void DeveLancarUmaDomainException_QuandoForUmaSequenciaDeNumeros()
        {
            var cpfComSequenciaDeNumeros = "12345678909";

            Assert.Throws<CpfException>(() => new Cpf(cpfComSequenciaDeNumeros));
        }

        [Fact]
        public void Igualdade()
        {
            var numeroCpf1 = "31116432129";
            var numeroCpf2 = "31116432129";

            var cpf1 = new Cpf(numeroCpf1);
            var cpf2 = new Cpf(numeroCpf2);
            var cpfIguais = cpf1 == cpf2;

            Assert.True(cpfIguais);
            Assert.True(cpf1.Equals(cpf2));
            Assert.True(cpf1.GetHashCode() == cpf2.GetHashCode());
        }
    }
}