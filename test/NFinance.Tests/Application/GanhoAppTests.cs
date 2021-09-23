using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using NFinance.Application;
using System.Threading.Tasks;
using Moq;
using NFinance.Application.ViewModel.GanhoViewModel;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Ganho;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Tests.Application
{
    public class GanhoAppTests
    {
        private readonly Mock<IGanhoRepository> _ganhoRepository;
        private readonly Guid _id = Guid.NewGuid();
        private readonly Guid _idCliente = Guid.NewGuid();
        private readonly string _nomeGanho = "ganho";
        private readonly decimal _valor = 12312.123M;
        private readonly bool _recorrente = false;
        private readonly DateTime _dataGanho = DateTime.Today;

        public GanhoAppTests()
        {
            _ganhoRepository = new Mock<IGanhoRepository>();
        }
        
        private GanhoApp IniciaApplication()
        {
            return new(_ganhoRepository.Object);
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
            _ganhoRepository.Setup(x => x.CadastrarGanho(It.IsAny<Ganho>())).ReturnsAsync(ganho);
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
        
        [Fact]
        public async Task GanhoApp_AtualizaGanho_ComSucesso()
        {
            //Arrange
            var ganho = new Ganho(_idCliente, _nomeGanho, _valor, _recorrente, _dataGanho);
            var atualizaGanhoVm = new AtualizarGanhoViewModel.Request(ganho);
            _ganhoRepository.Setup(x => x.AtualizarGanho(It.IsAny<Ganho>())).ReturnsAsync(ganho);
            var app = IniciaApplication();
            
            //Act
            var response = await app.AtualizarGanho(_id,atualizaGanhoVm);

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
        public async Task GanhoApp_AtualizaGanho_ComIdGanho_Invalido()
        {
            //Arrange
            var ganho = new Ganho(_idCliente, _nomeGanho, _valor, _recorrente, _dataGanho);
            var atualizaGanhoVm = new AtualizarGanhoViewModel.Request(ganho);
            _ganhoRepository.Setup(x => x.AtualizarGanho(It.IsAny<Ganho>())).ReturnsAsync(ganho);
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => app.AtualizarGanho(Guid.Empty, atualizaGanhoVm));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GanhoApp_AtualizaGanho_ComNomeGanho_Invalido(string nome)
        {
            //Arrange
            var atualizaGanhoVm = new AtualizarGanhoViewModel.Request { IdCliente = _idCliente, NomeGanho = nome,Valor = _valor,Recorrente = false, DataDoGanho = _dataGanho};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<NomeGanhoException>(() => app.AtualizarGanho(_id, atualizaGanhoVm));
        }
        
        [Theory]
        [MemberData(nameof(Valor))]
        public async Task GanhoApp_AtualizaGanho_ComValor_Invalido(decimal valor)
        {
            //Arrange
            var atualizaGanhoVm = new AtualizarGanhoViewModel.Request { IdCliente = _idCliente, NomeGanho = _nomeGanho,Valor = valor,Recorrente = false, DataDoGanho = _dataGanho};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<ValorGanhoException>(() => app.AtualizarGanho(_id, atualizaGanhoVm));
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public async Task GanhoApp_AtualizaGanho_ComData_Invalido(DateTime data)
        {
            //Arrange
            var atualizaGanhoVm = new AtualizarGanhoViewModel.Request { IdCliente = _idCliente, NomeGanho = _nomeGanho,Valor = _valor,Recorrente = false, DataDoGanho = data};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<DataGanhoException>(() => app.AtualizarGanho(_id, atualizaGanhoVm));
        }
        
        [Fact]
        public async Task GanhoApp_AtualizaGanho_ComIdCliente_Invalido()
        {
            //Arrange
            var atualizaGanhoVm = new AtualizarGanhoViewModel.Request { IdCliente = Guid.Empty, NomeGanho = _nomeGanho,Valor = _valor,Recorrente = false, DataDoGanho = _dataGanho};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => app.AtualizarGanho(_id, atualizaGanhoVm));
        }
        
        [Fact]
        public async Task GanhoApp_ConsultaGanho_ComIdCliente_Invalido()
        {
            //Arrange
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => app.ConsultarGanho(Guid.Empty));
        }
        
        [Fact]
        public async Task GanhoApp_ConsultaGanho_ComSucesso()
        {
            //Arrange
            var ganho = new Ganho(_idCliente, _nomeGanho, _valor, _recorrente, _dataGanho);
            _ganhoRepository.Setup(x => x.ConsultarGanho(It.IsAny<Guid>())).ReturnsAsync(ganho);
            var app = IniciaApplication();
            
            //Act
            var response = await app.ConsultarGanho(_id);

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
        public async Task GanhoApp_ConsultaGanhos_ComSucesso()
        {
            //Arrange
            var ganho = new Ganho(_idCliente, _nomeGanho, _valor, _recorrente, _dataGanho);
            _ganhoRepository.Setup(x => x.ConsultarGanhos(It.IsAny<Guid>())).ReturnsAsync(new List<Ganho> { ganho });
            var app = IniciaApplication();
            
            //Act
            var response = await app.ConsultarGanhos(_id);

            //Assert
            Assert.NotNull(response);
            var gasto = response.FirstOrDefault();
            Assert.Equal(ganho.Id,gasto.Id);
            Assert.Equal(ganho.IdCliente,gasto.IdCliente);
            Assert.Equal(ganho.NomeGanho,gasto.NomeGanho);
            Assert.Equal(ganho.Valor,gasto.Valor);
            Assert.Equal(ganho.Recorrente,gasto.Recorrente);
            Assert.Equal(ganho.DataDoGanho,gasto.DataDoGanho);
        }
        
        [Fact]
        public async Task GanhoApp_ConsultaGanhos_ComIdCliente_Invalido()
        {
            //Arrange
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<IdException>(() => app.ConsultarGanhos(Guid.Empty));
        }
        
        [Fact]
        public async Task GanhoApp_ExcluirGanho_ComSucesso()
        {
            //Arrange
            var ganho = new Ganho(_id,_idCliente,"Teste");
            var excluirGanhoVm = new ExcluirGanhoViewModel.Request(ganho);
            _ganhoRepository.Setup(x => x.ExcluirGanho(It.IsAny<Guid>())).ReturnsAsync(true);
            var app = IniciaApplication();
            
            //Act
            var response = await app.ExcluirGanho(excluirGanhoVm);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(200,response.StatusCode);
            Assert.Equal("Excluido Com Sucesso",response.Mensagem);
        }
    }
}