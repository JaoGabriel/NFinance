using System;
using System.Collections.Generic;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using Xunit;

namespace NFinance.Tests.Domain
{
    public class ResgateTests
    {
        private readonly Guid _idCliente = Guid.NewGuid();
        private readonly Guid _idInvestimentos = Guid.NewGuid();
        private readonly string _motivo = "Necessidade";
        private readonly decimal _valorResgate = 1524.01M;
        private readonly DateTime _dataResgate = new(2021, 10, 05);

        public static IEnumerable<object[]> ResgatesInvalidos => new List<object[]>
        {
            new object[] { Guid.Empty, Guid.NewGuid(), "teste",1000,DateTime.Today},
            new object[] { Guid.NewGuid(), Guid.Empty, "teste",1000,DateTime.Today},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "",1000,DateTime.Today},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), " ",1000,DateTime.Today},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), null,1000,DateTime.Today},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",decimal.MinValue,DateTime.Today},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",decimal.MinusOne,DateTime.Today},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",decimal.MaxValue,DateTime.Today},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",1000,DateTime.MaxValue},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",1000,DateTime.MinValue},
        };
        
        public static IEnumerable<object[]> AtualizarResgatesInvalidos => new List<object[]>
        {
            new object[] {"",1000,DateTime.Today},
            new object[] {" ",1000,DateTime.Today},
            new object[] {null,1000,DateTime.Today},
            new object[] {"Teste Resgate",0,DateTime.Today},
            new object[] {"Teste Resgate",decimal.MinValue,DateTime.Today},
            new object[] {"Teste Resgate",decimal.MinusOne,DateTime.Today},
            new object[] {"Teste Resgate",decimal.MaxValue,DateTime.Today},
            new object[] {"Teste Resgate",1000,DateTime.MaxValue},
            new object[] {"Teste Resgate",1000,DateTime.MinValue}
        };

        [Fact]
        public void DadoUmResgate_QuandoCadastrarComSucesso_DeveRetornarResgate()
        {
            //Act
            var resgate = new Resgate(_idInvestimentos,_idCliente, _valorResgate, _motivo ,_dataResgate);

            //Assert
            Assert.NotEqual(Guid.Empty, resgate.Id);
            Assert.Equal(_idCliente, resgate.IdCliente);
            Assert.Equal(_motivo, resgate.MotivoResgate);
            Assert.Equal(_valorResgate, resgate.Valor);
            Assert.Equal(_dataResgate, resgate.DataResgate);
        }

        [Theory]
        [MemberData(nameof(ResgatesInvalidos))]
        public void DadoUmResgate_QuandoCadastrarEDadosForemInvalidos_DeveRetornarDomainException(Guid idInvestimento,Guid idCliente, string motivo, decimal valorResgate, DateTime dataResgate)
        {
            //Arrange   
            Assert.Throws<DomainException>(() => new Resgate(idInvestimento, idCliente, valorResgate, motivo, dataResgate));
        }

        [Fact]
        public void DadoUmResgate_QuandoAtualizarComSucesso_DeveRetornarResgate()
        {
            //Arrange
            var resgate = new Resgate(_idInvestimentos,_idCliente, _valorResgate, _motivo ,_dataResgate);
            
            //Act
            resgate.AtualizaResgate(12345.11M,"Motivo",new DateTime(2021,10,6));

            //Assert
            Assert.NotEqual(_motivo, resgate.MotivoResgate);
            Assert.NotEqual(_valorResgate, resgate.Valor);
            Assert.NotEqual(_dataResgate, resgate.DataResgate);
        }

        [Theory]
        [MemberData(nameof(AtualizarResgatesInvalidos))]
        public void DadoUmResgate_QuandoAtualizarEDadosForemInvalidos_DeveRetornarDomainException(string motivo, decimal valorResgate, DateTime dataResgate)
        {
            //Arrange
            var resgate = new Resgate(_idInvestimentos,_idCliente, _valorResgate, _motivo ,_dataResgate);
            
            //Assert   
            Assert.Throws<DomainException>(() => resgate.AtualizaResgate(valorResgate, motivo, dataResgate));
        }
    }
}