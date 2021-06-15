using System;
using System.Collections.Generic;
using Xunit;
using NSubstitute;
using NFinance.Application;
using System.Threading.Tasks;
using NFinance.Application.ViewModel.GanhoViewModel;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Ganho;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Tests.Application
{
    public class GanhoAppTests
    {
        private readonly IGanhoRepository _ganhoRepository;
        private readonly Guid _id = Guid.NewGuid();
        private readonly Guid _idCliente = Guid.NewGuid();
        private readonly string _nomeGanho = "ganho";
        private readonly decimal _valor = 12312.123M;
        private readonly bool _recorrente = false;
        private readonly DateTime _dataGanho = DateTime.Today;

        public GanhoAppTests()
        {
            _ganhoRepository = Substitute.For<IGanhoRepository>();
        }
        
        private GanhoApp IniciaApplication()
        {
            return new(_ganhoRepository);
        }

        public static IEnumerable<object[]> Valor =>
            new List<object[]>
            {
                new object[] { decimal.Zero },
                new object[] { decimal.MaxValue },
                new object[] { decimal.MinValue },
                new object[] { decimal.MinusOne },
            };
        
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { DateTime.MaxValue },
                new object[] { DateTime.MinValue }
            };
        
        [Fact]
        public async Task GanhoApp_CadastraGanho_ComSucesso()
        {
            //Arrange
            var ganho = new Ganho(_idCliente, _nomeGanho, _valor, _recorrente, _dataGanho);
            var cadastrarGanhoVm = new CadastrarGanhoViewModel.Request(ganho);
            _ganhoRepository.CadastrarGanho(Arg.Any<Ganho>()).Returns(ganho);
            var app = IniciaApplication();
            
            //Act
            var response = await app.CadastrarGanho(cadastrarGanhoVm);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(ganho.Id,response.Id);
            Assert.Equal(ganho.IdCliente,response.IdCliente);
            Assert.Equal(ganho.NomeGanho,response.NomeGanho);
            Assert.Equal(ganho.Valor,response.Valor);
            Assert.Equal(ganho.Recorrente,response.Recorrente);
            Assert.Equal(ganho.DataDoGanho,response.DataDoGanho);
        }

        [Fact]
        public async Task GanhoApp_CadastraGanho_ComIdCliente_Invalido()
        {
            //Arrange
            var cadastrarGanhoVm = new CadastrarGanhoViewModel.Request { IdCliente = Guid.Empty, Recorrente = _recorrente,Valor = _valor, NomeGanho = _nomeGanho, DataDoGanho = _dataGanho};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => app.CadastrarGanho(cadastrarGanhoVm));
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GanhoApp_CadastraGanho_ComNomeGanho_Invalido(string nomeGanho)
        {
            //Arrange
            var cadastrarGanhoVm = new CadastrarGanhoViewModel.Request { IdCliente = _idCliente, Recorrente = _recorrente,Valor = _valor, NomeGanho = nomeGanho, DataDoGanho = _dataGanho};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => app.CadastrarGanho(cadastrarGanhoVm));
        }
        
        [Theory]
        [MemberData(nameof(Valor))]
        public async Task GanhoApp_CadastraGanho_ComValor_Invalido(decimal valor)
        {
            //Arrange
            var cadastrarGanhoVm = new CadastrarGanhoViewModel.Request { IdCliente = _idCliente, Recorrente = _recorrente,Valor = valor, NomeGanho = _nomeGanho, DataDoGanho = _dataGanho};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<ValorGanhoException>(() => app.CadastrarGanho(cadastrarGanhoVm));
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public async Task GanhoApp_CadastraGanho_ComData_Invalida(DateTime data)
        {
            //Arrange
            var cadastrarGanhoVm = new CadastrarGanhoViewModel.Request { IdCliente = _idCliente, Recorrente = _recorrente,Valor = _valor, NomeGanho = _nomeGanho, DataDoGanho = data};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => app.CadastrarGanho(cadastrarGanhoVm));
        }
        
    }
}