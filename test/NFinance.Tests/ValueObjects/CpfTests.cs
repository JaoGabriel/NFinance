using NFinance.Domain.ObjetosDeValor;
using Xunit;

namespace NFinance.Tests.ValueObjects
{
    public class CpfTests
    {
        [Theory]
        [InlineData("57439086058")]
        [InlineData("47711884087")]
        [InlineData("561.300.780-21")]
        [InlineData("807.784.060-40")]
        [InlineData("11672475074")]
        public void CpfValido(string numero)
        {
            var cpf = new Cpf(numero);
            
            Assert.NotNull(cpf);
            Assert.Equal(cpf.Numero,numero.Replace(".", string.Empty).Replace("-", string.Empty));
        }
    }
}