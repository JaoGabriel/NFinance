using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFinance.Tests
{
    public class ValueObjectsTests
    {
        [Fact]
        public void DeveRetornarTrue_AoCompararDoisValoresIguais_ComEquals()
        {
            var valor1 = new TesteValueObject { Teste = "1" };
            var valor2 = new TesteValueObject { Teste = "1" };

            Assert.True(valor1.Equals(valor2));
        }

        [Fact]
        public void DeveRetornarTrue_AoCompararDoisValoresIguais_ComEqualOperator()
        {
            var valor1 = new TesteValueObject { Teste = "1" };
            var valor2 = new TesteValueObject { Teste = "1" };

            Assert.True(valor1 == valor2);
        }

        [Fact]
        public void DeveRetornarTrue_AoCompararDoisValoresIguais_ComNotEqualOperator()
        {
            var valor1 = new TesteValueObject { Teste = "1" };
            var valor2 = new TesteValueObject { Teste = "2" };

            Assert.True(valor1 != valor2);
        }

        [Fact]
        public void DeveRetornarFalse_AoCompararUmValorENulo_ComEqualsOperator()
        {
            var valor1 = new TesteValueObject { Teste = "1" };
            TesteValueObject valor2 = null;

            Assert.False(valor1 == valor2);
        }

        [Fact]
        public void DeveRetornarFalse_AoCompararUmValorENulo_ComEquals()
        {
            var valor1 = new TesteValueObject { Teste = "1" };

            Assert.False(valor1.Equals(null));
        }

        [Fact]
        public void DeveRetornarTrue_AoCompararDoisValores_ComGetHashCode()
        {
            var valor1 = new TesteValueObject { Teste = "1", Teste2 = "2" };
            var valor2 = new TesteValueObject { Teste = "1", Teste2 = "2" };

            Assert.True(valor1.GetHashCode() == valor2.GetHashCode());
        }
    }

    public class TesteValueObject : ValueObject
    {
        public string Teste { get; set; }
        public string Teste2 { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Teste;
            yield return Teste2;
        }
    }
}
}
