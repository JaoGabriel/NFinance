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
        private readonly DateTimeOffset _dataResgate = new(2021, 10, 05, 0, 0, 0, new(0, 0, 0));

        public static IEnumerable<object[]> ResgatesInvalidos => new List<object[]>
        {
            new object[] { Guid.Empty, Guid.NewGuid(), "teste",1000,DateTimeOffset.Now.DateTime},
            new object[] { Guid.NewGuid(), Guid.Empty, "teste",1000,DateTimeOffset.Now.DateTime},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "",1000,DateTimeOffset.Now.DateTime},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), " ",1000,DateTimeOffset.Now.DateTime},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), null,1000,DateTimeOffset.Now.DateTime},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",decimal.MinValue,DateTimeOffset.Now.DateTime},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",decimal.MinusOne,DateTimeOffset.Now.DateTime},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",decimal.MaxValue,DateTimeOffset.Now.DateTime},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",1000,DateTimeOffset.MaxValue},
            new object[] { Guid.NewGuid(), Guid.NewGuid(), "Teste Resgate",1000,DateTimeOffset.MinValue},
        };
        
        public static IEnumerable<object[]> AtualizarResgatesInvalidos => new List<object[]>
        {
            new object[] {"",1000,DateTimeOffset.Now.DateTime},
            new object[] {" ",1000,DateTimeOffset.Now.DateTime},
            new object[] {null,1000,DateTimeOffset.Now.DateTime},
            new object[] {"Teste Resgate",0,DateTimeOffset.Now.DateTime},
            new object[] {"Teste Resgate",decimal.MinValue,DateTimeOffset.Now.DateTime},
            new object[] {"Teste Resgate",decimal.MinusOne,DateTimeOffset.Now.DateTime},
            new object[] {"Teste Resgate",decimal.MaxValue,DateTimeOffset.Now.DateTime},
            new object[] {"Teste Resgate",1000,DateTimeOffset.MaxValue},
            new object[] {"Teste Resgate",1000,DateTimeOffset.MinValue}
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
        public void DadoUmResgate_QuandoCadastrarEDadosForemInvalidos_DeveRetornarDomainException(Guid idInvestimento,Guid idCliente, string motivo, decimal valorResgate, DateTimeOffset dataResgate)
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
            resgate.AtualizaResgate(12345.11M,"Motivo",new DateTimeOffset(2021, 10, 6, 0, 0, 0, new(0, 0, 0)));

            //Assert
            Assert.NotEqual(_motivo, resgate.MotivoResgate);
            Assert.NotEqual(_valorResgate, resgate.Valor);
            Assert.NotEqual(_dataResgate, resgate.DataResgate);
        }

        [Theory]
        [MemberData(nameof(AtualizarResgatesInvalidos))]
        public void DadoUmResgate_QuandoAtualizarEDadosForemInvalidos_DeveRetornarDomainException(string motivo, decimal valorResgate, DateTimeOffset dataResgate)
        {
            //Arrange
            var resgate = new Resgate(_idInvestimentos,_idCliente, _valorResgate, _motivo ,_dataResgate);
            
            //Assert   
            Assert.Throws<DomainException>(() => resgate.AtualizaResgate(valorResgate, motivo, dataResgate));
        }
    }
}