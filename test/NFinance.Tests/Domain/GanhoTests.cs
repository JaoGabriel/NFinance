using NFinance.Domain;
using System;
using Xunit;

namespace NFinance.Tests.Domain
{
    public class GanhoTests
    {
        private readonly Guid _idCliente = Guid.NewGuid();
        private readonly string _nome = "Salario";
        private readonly decimal _valorGanho = 1524.01M;
        private readonly DateTime _dataGanho = new(2021,10,05);

        [Fact]
        public void DadoUmGanho_QuandoCadastrarComSucesso_DeveRetornarGanho()
        {
            //Act
            var ganho = new Ganho(_idCliente,_nome,_valorGanho,true,_dataGanho);

            //Assert
            Assert.NotEqual(Guid.Empty,ganho.Id);
            Assert.Equal(_idCliente, ganho.IdCliente);
            Assert.Equal(_nome,ganho.NomeGanho);
            Assert.Equal(_valorGanho,ganho.Valor);
            Assert.Equal(_dataGanho,ganho.DataDoGanho);
            Assert.True(ganho.Recorrente);
        }
    }
}