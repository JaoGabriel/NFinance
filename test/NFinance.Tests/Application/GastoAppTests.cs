using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Threading.Tasks;
using Moq;
using NFinance.Application;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Exceptions;
using NFinance.Application.ViewModel.GastosViewModel;
using NFinance.Application.Exceptions;

namespace NFinance.Tests.Application
{
    public class GastoAppTests
    {
        private readonly Mock<IGastoRepository> _gastoRepository;
        private readonly Guid _id = Guid.NewGuid();
        private readonly Guid _idCliente = Guid.NewGuid();
        private readonly string _nomeGasto = "gasto";
        private readonly decimal _valor = 12312.123M;
        private readonly int _quantidadeParcelas = 3;
        private readonly DateTimeOffset _dataGasto = DateTimeOffset.Now;

        public GastoAppTests()
        {
            _gastoRepository = new Mock<IGastoRepository>();
        }
        
        private GastoApp IniciaApplication()
        {
            return new(_gastoRepository.Object);
        }

        public static IEnumerable<object[]> Valor =>
            new List<object[]>
            {
                new object[] { decimal.Zero },
                new object[] { decimal.MaxValue },
                new object[] { decimal.MinValue },
                new object[] { decimal.MinusOne },
            };

        public static IEnumerable<object[]> QuantidadeParcelas =>
            new List<object[]>
            {
                new object[] { int.MinValue },
                new object[] { -1 },
            };

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { DateTimeOffset.MaxValue },
                new object[] { DateTimeOffset.MinValue }
            };
        
        [Fact]
        public async Task GastoApp_CadastraGasto_ComSucesso()
        {
            //Arrange
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            var cadastrarGastoVm = new CadastrarGastoViewModel.Request(gasto);
            _gastoRepository.Setup(x => x.CadastrarGasto(It.IsAny<Gasto>())).ReturnsAsync(gasto);
            var app = IniciaApplication();
            
            //Act
            var response = await app.CadastrarGasto(cadastrarGastoVm);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(gasto.Id,response.Id);
            Assert.Equal(gasto.IdCliente,response.IdCliente);
            Assert.Equal(gasto.NomeGasto,response.NomeGasto);
            Assert.Equal(gasto.Valor,response.Valor);
            Assert.Equal(gasto.QuantidadeParcelas,response.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto,response.DataDoGasto);
        }

        [Fact]
        public async Task GastoApp_CadastraGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var cadastrarGastoVm = new CadastrarGastoViewModel.Request { IdCliente = Guid.Empty, QuantidadeParcelas = _quantidadeParcelas,Valor = _valor, NomeGasto = _nomeGasto, DataDoGasto = _dataGasto};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarGasto(cadastrarGastoVm));
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GastoApp_CadastraGasto_ComNomeGasto_Invalido(string nomeGasto)
        {
            //Arrange
            var cadastrarGastoVm = new CadastrarGastoViewModel.Request { IdCliente = _idCliente, QuantidadeParcelas = _quantidadeParcelas,Valor = _valor, NomeGasto = nomeGasto, DataDoGasto = _dataGasto};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarGasto(cadastrarGastoVm));
        }
        
        [Theory]
        [MemberData(nameof(Valor))]
        public async Task GastoApp_CadastraGasto_ComValor_Invalido(decimal valor)
        {
            //Arrange
            var cadastrarGastoVm = new CadastrarGastoViewModel.Request { IdCliente = _idCliente, QuantidadeParcelas = _quantidadeParcelas,Valor = valor, NomeGasto = _nomeGasto, DataDoGasto = _dataGasto};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarGasto(cadastrarGastoVm));
        }

        [Theory]
        [MemberData(nameof(QuantidadeParcelas))]
        public async Task GastoApp_CadastraGasto_ComQuantidadeParcelas_Invalido(int quantidadeParcelas)
        {
            //Arrange
            var cadastrarGastoVm = new CadastrarGastoViewModel.Request { IdCliente = _idCliente, QuantidadeParcelas = quantidadeParcelas, Valor = _valor, NomeGasto = _nomeGasto, DataDoGasto = _dataGasto };
            var app = IniciaApplication();

            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarGasto(cadastrarGastoVm));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task GastoApp_CadastraGasto_ComData_Invalida(DateTimeOffset data)
        {
            //Arrange
            var cadastrarGastoVm = new CadastrarGastoViewModel.Request { IdCliente = _idCliente, QuantidadeParcelas = _quantidadeParcelas,Valor = _valor, NomeGasto = _nomeGasto, DataDoGasto = data};
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.CadastrarGasto(cadastrarGastoVm));
        }
        
        [Fact]
        public async Task GastoApp_AtualizaGasto_ComSucesso()
        {
            //Arrange
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            var atualizaGastoVm = new AtualizarGastoViewModel.Request(gasto);
            _gastoRepository.Setup(x => x.ConsultarGasto(It.IsAny<Guid>())).ReturnsAsync(gasto);
            _gastoRepository.Setup(x => x.AtualizarGasto(It.IsAny<Gasto>())).ReturnsAsync(gasto);
            var app = IniciaApplication();
            
            //Act
            var response = await app.AtualizarGasto(_id,atualizaGastoVm);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(gasto.Id,response.Id);
            Assert.Equal(gasto.IdCliente,response.IdCliente);
            Assert.Equal(gasto.NomeGasto,response.NomeGasto);
            Assert.Equal(gasto.Valor,response.Valor);
            Assert.Equal(gasto.QuantidadeParcelas,response.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto,response.DataDoGasto);
        }

        [Fact]
        public async Task GastoApp_AtualizaGasto_ComIdGasto_Invalido()
        {
            //Arrange
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            var atualizaGastoVm = new AtualizarGastoViewModel.Request(gasto);
            _gastoRepository.Setup(x => x.AtualizarGasto(It.IsAny<Gasto>())).ReturnsAsync(gasto);
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<GastoException>(() => app.AtualizarGasto(Guid.Empty, atualizaGastoVm));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GastoApp_AtualizaGasto_ComNomeGasto_Invalido(string nome)
        {
            //Arrange
            var atualizaGastoVm = new AtualizarGastoViewModel.Request { IdCliente = _idCliente, NomeGasto = nome,Valor = _valor, QuantidadeParcelas = 2, DataDoGasto = _dataGasto};
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            _gastoRepository.Setup(x => x.ConsultarGasto(It.IsAny<Guid>())).ReturnsAsync(gasto);
            var app = IniciaApplication();

            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.AtualizarGasto(_id, atualizaGastoVm));
        }
        
        [Theory]
        [MemberData(nameof(Valor))]
        public async Task GastoApp_AtualizaGasto_ComValor_Invalido(decimal valor)
        {
            //Arrange
            var atualizaGastoVm = new AtualizarGastoViewModel.Request { IdCliente = _idCliente, NomeGasto = _nomeGasto,Valor = valor, QuantidadeParcelas = 2, DataDoGasto = _dataGasto};
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            _gastoRepository.Setup(x => x.ConsultarGasto(It.IsAny<Guid>())).ReturnsAsync(gasto);
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.AtualizarGasto(_id, atualizaGastoVm));
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public async Task GastoApp_AtualizaGasto_ComData_Invalido(DateTimeOffset data)
        {
            //Arrange
            var atualizaGastoVm = new AtualizarGastoViewModel.Request { IdCliente = _idCliente, NomeGasto = _nomeGasto,Valor = _valor, QuantidadeParcelas = 2, DataDoGasto = data};
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            _gastoRepository.Setup(x => x.ConsultarGasto(It.IsAny<Guid>())).ReturnsAsync(gasto);
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<DomainException>(() => app.AtualizarGasto(_id, atualizaGastoVm));
        }
        
        [Fact]
        public async Task GastoApp_AtualizaGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var atualizaGastoVm = new AtualizarGastoViewModel.Request { IdCliente = Guid.Empty, NomeGasto = _nomeGasto,Valor = _valor, QuantidadeParcelas = 2, DataDoGasto = _dataGasto};
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<GastoException>(() => app.AtualizarGasto(_id, atualizaGastoVm));
        }
        
        [Fact]
        public async Task GastoApp_ConsultaGasto_ComIdCliente_Invalido()
        {
            //Arrange
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<GastoException>(() => app.ConsultarGasto(Guid.Empty));
        }
        
        [Fact]
        public async Task GastoApp_ConsultaGasto_ComSucesso()
        {
            //Arrange
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            _gastoRepository.Setup(x => x.ConsultarGasto(It.IsAny<Guid>())).ReturnsAsync(gasto);
            var app = IniciaApplication();
            
            //Act
            var response = await app.ConsultarGasto(_id);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(gasto.Id,response.Id);
            Assert.Equal(gasto.IdCliente,response.IdCliente);
            Assert.Equal(gasto.NomeGasto,response.NomeGasto);
            Assert.Equal(gasto.Valor,response.Valor);
            Assert.Equal(gasto.QuantidadeParcelas,response.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto,response.DataDoGasto);
        }
        
        [Fact]
        public async Task GastoApp_ConsultaGastos_ComSucesso()
        {
            //Arrange
            var gasto = new Gasto(_idCliente, _nomeGasto, _valor, _quantidadeParcelas, _dataGasto);
            _gastoRepository.Setup(x => x.ConsultarGastos(It.IsAny<Guid>())).ReturnsAsync(new List<Gasto> { gasto });
            var app = IniciaApplication();
            
            //Act
            var response = await app.ConsultarGastos(_id);

            //Assert
            Assert.NotNull(response);
            var gastoResponse = response.FirstOrDefault();
            Assert.Equal(gasto.Id, gastoResponse.Id);
            Assert.Equal(gasto.IdCliente, gastoResponse.IdCliente);
            Assert.Equal(gasto.NomeGasto, gastoResponse.NomeGasto);
            Assert.Equal(gasto.Valor, gastoResponse.Valor);
            Assert.Equal(gasto.QuantidadeParcelas, gastoResponse.QuantidadeParcelas);
            Assert.Equal(gasto.DataDoGasto, gastoResponse.DataDoGasto);
        }
        
        [Fact]
        public async Task GastoApp_ConsultaGastos_ComIdCliente_Invalido()
        {
            //Arrange
            var app = IniciaApplication();
            
            //Assert
            await Assert.ThrowsAsync<GastoException>(() => app.ConsultarGastos(Guid.Empty));
        }
        
        [Fact]
        public async Task GastoApp_ExcluirGasto_ComSucesso()
        {
            //Arrange
            var gasto = new Gasto(_idCliente,"Teste",1000, 23,DateTime.Now);
            var excluirGastoVm = new ExcluirGastoViewModel.Request { IdGasto = gasto.Id, MotivoExclusao = "teste" };
            _gastoRepository.Setup(x => x.ExcluirGasto(It.IsAny<Guid>())).ReturnsAsync(true);
            var app = IniciaApplication();
            
            //Act
            var response = await app.ExcluirGasto(excluirGastoVm);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(200,response.StatusCode);
            Assert.Equal("Excluido Com Sucesso",response.Mensagem);
        }
    }
}