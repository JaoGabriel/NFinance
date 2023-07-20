using System;
using System.Collections.Generic;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using Xunit;

namespace NFinance.Tests.Domain
{
    public class GastoTests
    {
        private readonly Guid _idCliente = Guid.NewGuid();
        private readonly string _nome = "Salario";
        private readonly decimal _valorGasto = 1524.01M;
        private readonly DateTimeOffset _dataGasto = new(2021, 10, 05, 0, 0, 0, new(0, 0, 0));

        public static IEnumerable<object[]> GastosInvalidos => new List<object[]>
        {
            new object[] {Guid.Empty,"Nome Gasto",1000,DateTimeOffset.Now.DateTime, 15},
            new object[] {Guid.NewGuid(),"",1000,DateTimeOffset.Now.DateTime, 15},
            new object[] {Guid.NewGuid()," ",1000,DateTimeOffset.Now.DateTime, 15},
            new object[] {Guid.NewGuid(),null,1000,DateTimeOffset.Now.DateTime, 15},
            new object[] {Guid.NewGuid(),"Teste Gasto",decimal.MinValue,DateTimeOffset.Now.DateTime, 15},
            new object[] {Guid.NewGuid(),"Teste Gasto",decimal.MinusOne,DateTimeOffset.Now.DateTime, 15},
            new object[] {Guid.NewGuid(),"Teste Gasto",decimal.MaxValue,DateTimeOffset.Now.DateTime, 15},
            new object[] {Guid.NewGuid(),"Teste Gasto",1000,DateTimeOffset.Now.DateTime, -1 },
            new object[] {Guid.NewGuid(),"Teste Gasto",1000,DateTimeOffset.MaxValue, 15},
            new object[] {Guid.NewGuid(),"Teste Gasto",1000,DateTimeOffset.MinValue, 15},
        };
        
        public static IEnumerable<object[]> AtualizarGastosInvalidos => new List<object[]>
        {
            new object[] {"",1000,DateTimeOffset.Now.DateTime, 15},
            new object[] {" ",1000,DateTimeOffset.Now.DateTime, 15},
            new object[] {null,1000,DateTimeOffset.Now.DateTime, 15},
            new object[] {"Teste Gasto",0,DateTimeOffset.Now.DateTime, 15},
            new object[] {"Teste Gasto",decimal.MinValue,DateTimeOffset.Now.DateTime, 15},
            new object[] {"Teste Gasto",decimal.MinusOne,DateTimeOffset.Now.DateTime, 15},
            new object[] {"Teste Gasto",decimal.MaxValue,DateTimeOffset.Now.DateTime, 15},
            new object[] {"Teste Gasto",1000,DateTimeOffset.Now.DateTime, -5 },
            new object[] {"Teste Gasto",1000,DateTimeOffset.MaxValue, 15},
            new object[] {"Teste Gasto",1000,DateTimeOffset.MinValue, 15}
        };

        [Fact]
        public void DadoUmGasto_QuandoCadastrarComSucesso_DeveRetornarGasto()
        {
            //Act
            var gasto = new Gasto(_idCliente, _nome, _valorGasto, 15, _dataGasto);

            //Assert
            Assert.NotEqual(Guid.Empty, gasto.Id);
            Assert.Equal(_idCliente, gasto.IdCliente);
            Assert.Equal(_nome, gasto.NomeGasto);
            Assert.Equal(_valorGasto, gasto.Valor);
            Assert.Equal(_dataGasto, gasto.DataDoGasto);
            Assert.Equal(15, gasto.QuantidadeParcelas);
        }

        [Theory]
        [MemberData(nameof(GastosInvalidos))]
        public void DadoUmGasto_QuandoCadastrarEDadosForemInvalidos_DeveRetornarDomainException(Guid idCliente, string nome, decimal valorGasto, DateTimeOffset dataGasto, int parcelas)
        {
            //Arrange   
            Assert.Throws<DomainException>(() => new Gasto(idCliente, nome, valorGasto, parcelas, dataGasto));
        }

        [Fact]
        public void DadoUmGasto_QuandoAtualizarComSucesso_DeveRetornarGasto()
        {
            //Arrange
            var gasto = new Gasto(_idCliente, _nome, _valorGasto, 1, _dataGasto);
            
            //Act
            gasto.AtualizaGasto("Novo Nome",12345.11M,12,new DateTimeOffset(2021, 10, 6, 0, 0, 0, new(0, 0, 0)));

            //Assert
            Assert.NotEqual(_nome, gasto.NomeGasto);
            Assert.NotEqual(_valorGasto, gasto.Valor);
            Assert.NotEqual(_dataGasto, gasto.DataDoGasto);
            Assert.Equal(12, gasto.QuantidadeParcelas);
        }

        [Theory]
        [MemberData(nameof(AtualizarGastosInvalidos))]
        public void DadoUmGasto_QuandoAtualizarEDadosForemInvalidos_DeveRetornarDomainException(string nome, decimal valorGasto, DateTimeOffset dataGasto, int parcelas)
        {
            //Arrange
            var gasto = new Gasto(_idCliente, _nome, _valorGasto, 1, _dataGasto);
            
            //Assert   
            Assert.Throws<DomainException>(() => gasto.AtualizaGasto(nome,valorGasto, parcelas, dataGasto));
        }
    }
}