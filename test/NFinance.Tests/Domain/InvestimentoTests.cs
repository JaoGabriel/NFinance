using System;
using System.Collections.Generic;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using Xunit;

namespace NFinance.Tests.Domain
{
    public class InvestimentoTests
    {
        private readonly Guid _idCliente = Guid.NewGuid();
        private readonly string _nome = "Salario";
        private readonly decimal _valorInvestimento = 1524.01M;
        private readonly DateTime _dataInvestimento = new(2021, 10, 05);

        public static IEnumerable<object[]> InvestimentosInvalidos => new List<object[]>
        {
            new object[] {Guid.NewGuid(),"",1000,DateTime.Today},
            new object[] {Guid.NewGuid()," ",1000,DateTime.Today},
            new object[] {Guid.NewGuid(),null,1000,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Investimento",decimal.MinValue,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Investimento",decimal.MinusOne,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Investimento",decimal.MaxValue,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Investimento",1000,DateTime.MaxValue},
            new object[] {Guid.NewGuid(),"Teste Investimento",1000,DateTime.MinValue},
        };
        
        public static IEnumerable<object[]> AtualizarInvestimentosInvalidos => new List<object[]>
        {
            new object[] {"",1000,DateTime.Today},
            new object[] {" ",1000,DateTime.Today},
            new object[] {null,1000,DateTime.Today},
            new object[] {"Teste Investimento",0,DateTime.Today},
            new object[] {"Teste Investimento",decimal.MinValue,DateTime.Today},
            new object[] {"Teste Investimento",decimal.MinusOne,DateTime.Today},
            new object[] {"Teste Investimento",decimal.MaxValue,DateTime.Today},
            new object[] {"Teste Investimento",1000,DateTime.MaxValue},
            new object[] {"Teste Investimento",1000,DateTime.MinValue}
        };

        [Fact]
        public void DadoUmInvestimento_QuandoCadastrarComSucesso_DeveRetornarInvestimento()
        {
            //Act
            var investimento = new Investimento(_idCliente, _nome, _valorInvestimento, _dataInvestimento);

            //Assert
            Assert.NotEqual(Guid.Empty, investimento.Id);
            Assert.Equal(_idCliente, investimento.IdCliente);
            Assert.Equal(_nome, investimento.NomeInvestimento);
            Assert.Equal(_valorInvestimento, investimento.Valor);
            Assert.Equal(_dataInvestimento, investimento.DataAplicacao);
        }

        [Theory]
        [MemberData(nameof(InvestimentosInvalidos))]
        public void DadoUmInvestimento_QuandoCadastrarEDadosForemInvalidos_DeveRetornarDomainException(Guid idCliente, string nome, decimal valorInvestimento, DateTime dataInvestimento)
        {
            //Arrange   
            Assert.Throws<DomainException>(() => new Investimento(idCliente, nome, valorInvestimento, dataInvestimento));
        }

        [Fact]
        public void DadoUmInvestimento_QuandoAtualizarComSucesso_DeveRetornarInvestimento()
        {
            //Arrange
            var investimento = new Investimento(_idCliente, _nome, _valorInvestimento, _dataInvestimento);
            
            //Act
            investimento.AtualizaInvestimento("Novo Nome",12345.11M,new DateTime(2021,10,6));

            //Assert
            Assert.NotEqual(_nome, investimento.NomeInvestimento);
            Assert.NotEqual(_valorInvestimento, investimento.Valor);
            Assert.NotEqual(_dataInvestimento, investimento.DataAplicacao);
        }

        [Theory]
        [MemberData(nameof(AtualizarInvestimentosInvalidos))]
        public void DadoUmInvestimento_QuandoAtualizarEDadosForemInvalidos_DeveRetornarDomainException(string nome, decimal valorInvestimento, DateTime dataInvestimento)
        {
            //Arrange
            var investimento = new Investimento(_idCliente, _nome, _valorInvestimento, _dataInvestimento);
            
            //Assert   
            Assert.Throws<DomainException>(() => investimento.AtualizaInvestimento(nome,valorInvestimento, dataInvestimento));
        }
    }
}