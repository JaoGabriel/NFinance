using NFinance.Domain;
using NFinance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using Xunit;

namespace NFinance.Tests.Domain
{
    public class GanhoTests
    {
        private readonly Guid _idCliente = Guid.NewGuid();
        private readonly string _nome = "Salario";
        private readonly decimal _valorGanho = 1524.01M;
        private readonly DateTime _dataGanho = new(2021, 10, 05);

        public static IEnumerable<object[]> GanhosInvalidos => new List<object[]>
        {
            new object[] {Guid.Empty,"Nome Ganho",1000,DateTime.Today},
            new object[] {Guid.NewGuid(),"",1000,DateTime.Today},
            new object[] {Guid.NewGuid()," ",1000,DateTime.Today},
            new object[] {Guid.NewGuid(),null,1000,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Ganho",0,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Ganho",decimal.MinValue,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Ganho",decimal.MinusOne,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Ganho",decimal.MaxValue,DateTime.Today},
            new object[] {Guid.NewGuid(),"Teste Ganho",1000,DateTime.MaxValue},
            new object[] {Guid.NewGuid(),"Teste Ganho",1000,DateTime.MinValue}
        };
        
        public static IEnumerable<object[]> AtualizarGanhosInvalidos => new List<object[]>
        {
            new object[] {true,"",1000,DateTime.Today},
            new object[] {false," ",1000,DateTime.Today},
            new object[] {true,null,1000,DateTime.Today},
            new object[] {true,"Teste Ganho",0,DateTime.Today},
            new object[] {false,"Teste Ganho",decimal.MinValue,DateTime.Today},
            new object[] {true,"Teste Ganho",decimal.MinusOne,DateTime.Today},
            new object[] {false,"Teste Ganho",decimal.MaxValue,DateTime.Today},
            new object[] {false,"Teste Ganho",1000,DateTime.MaxValue},
            new object[] {true,"Teste Ganho",1000,DateTime.MinValue}
        };

        [Fact]
        public void DadoUmGanho_QuandoCadastrarComSucesso_DeveRetornarGanho()
        {
            //Act
            var ganho = new Ganho(_idCliente, _nome, _valorGanho, true, _dataGanho);

            //Assert
            Assert.NotEqual(Guid.Empty, ganho.Id);
            Assert.Equal(_idCliente, ganho.IdCliente);
            Assert.Equal(_nome, ganho.NomeGanho);
            Assert.Equal(_valorGanho, ganho.Valor);
            Assert.Equal(_dataGanho, ganho.DataDoGanho);
            Assert.True(ganho.Recorrente);
        }

        [Theory]
        [MemberData(nameof(GanhosInvalidos))]
        public void DadoUmGanho_QuandoCadastrarEDadosForemInvalidos_DeveRetornarDomainException(Guid idCliente, string nome, decimal valorGanho, DateTime dataGanho)
        {
            //Arrange   
            Assert.Throws<DomainException>(() => new Ganho(idCliente, nome, valorGanho, false, dataGanho));
        }

        [Fact]
        public void DadoUmGanho_QuandoAtualizarComSucesso_DeveRetornarGanho()
        {
            //Arrange
            var ganho = new Ganho(_idCliente, _nome, _valorGanho, true, _dataGanho);
            
            //Act
            ganho.AtualizaGanho("Novo Nome",12345.11M,false,new DateTime(2021,10,6));

            //Assert
            Assert.NotEqual(_nome, ganho.NomeGanho);
            Assert.NotEqual(_valorGanho, ganho.Valor);
            Assert.NotEqual(_dataGanho, ganho.DataDoGanho);
            Assert.False(ganho.Recorrente);
        }

        [Theory]
        [MemberData(nameof(AtualizarGanhosInvalidos))]
        public void DadoUmGanho_QuandoAtualizarEDadosForemInvalidos_DeveRetornarDomainException(bool recorrencia, string nome, decimal valorGanho, DateTime dataGanho)
        {
            //Arrange
            var ganho = new Ganho(_idCliente, _nome, _valorGanho, true, _dataGanho);
            
            //Assert   
            Assert.Throws<DomainException>(() => ganho.AtualizaGanho(nome,valorGanho,recorrencia,dataGanho));
        }
    }
}