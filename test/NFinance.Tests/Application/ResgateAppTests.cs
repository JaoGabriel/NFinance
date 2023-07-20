using Moq;
using NFinance.Application;
using NFinance.Application.Exceptions;
using NFinance.Application.ViewModel.GastosViewModel;
using NFinance.Application.ViewModel.ResgatesViewModel;
using NFinance.Domain;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NFinance.Tests.Application;

public class ResgateAppTests
{
    private readonly Mock<IResgateRepository> _resgateRepository;
    private readonly Guid _id = Guid.NewGuid();
    private readonly Guid _idCliente = Guid.NewGuid();
    private readonly Guid _idInvestimento = Guid.NewGuid();
    private readonly string _motivoResgate = "imprevisto";
    private readonly decimal _valor = 12312.123M;
    private readonly DateTimeOffset _data = DateTimeOffset.Now;

    public ResgateAppTests()
    {
        _resgateRepository = new Mock<IResgateRepository>();
    }

    private ResgateApp IniciaApplication()
    {
        return new(_resgateRepository.Object);
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
                new object[] { DateTimeOffset.MaxValue },
                new object[] { DateTimeOffset.MinValue }
        };

    [Fact]
    public async Task ResgateApp_ConsultarResgate_ComSucesso()
    {
        //Arrange
        var resgate = new Resgate(_idInvestimento, _idCliente, _valor, _motivoResgate, _data);
        _resgateRepository.Setup(x => x.ConsultarResgate(It.IsAny<Guid>())).ReturnsAsync(resgate);
        var app = IniciaApplication();

        //Act
        var response = await app.ConsultarResgate(_id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(resgate.Id, response.Id);
        Assert.Equal(resgate.IdCliente, response.IdCliente);
        Assert.Equal(resgate.IdInvestimento, response.IdInvestimento);
        Assert.Equal(resgate.Valor, response.Valor);
        Assert.Equal(resgate.DataResgate, response.DataResgate);
        Assert.Equal(resgate.MotivoResgate, response.MotivoResgate);
    }

    [Fact]
    public async Task ResgateApp_ConsultarResgate_ComId_Invalido()
    {
        //Arrange
        var app = IniciaApplication();

        //Assert
        await Assert.ThrowsAsync<ResgateException>(() => app.ConsultarResgate(Guid.Empty));
    }


    [Fact]
    public async Task ResgateApp_ConsultarResgates_ComSucesso()
    {
        //Arrange
        var resgate = new Resgate(_idInvestimento, _idCliente, _valor, _motivoResgate, _data);
        _resgateRepository.Setup(x => x.ConsultarResgates(It.IsAny<Guid>())).ReturnsAsync(new List<Resgate> { resgate });
        var app = IniciaApplication();

        //Act
        var response = await app.ConsultarResgates(_idCliente);

        //Assert
        Assert.NotNull(response);
        var responseList = response.FirstOrDefault();
        Assert.Equal(resgate.Id, responseList.Id);
        Assert.Equal(resgate.IdCliente, responseList.IdCliente);
        Assert.Equal(resgate.IdInvestimento, responseList.IdInvestimento);
        Assert.Equal(resgate.Valor, responseList.Valor);
        Assert.Equal(resgate.DataResgate, responseList.DataResgate);
        Assert.Equal(resgate.MotivoResgate, responseList.MotivoResgate);
    }

    [Fact]
    public async Task ResgateApp_ConsultarResgates_ComId_Invalido()
    {
        //Arrange
        var app = IniciaApplication();

        //Assert
        await Assert.ThrowsAsync<ResgateException>(() => app.ConsultarResgates(Guid.Empty));
    }

    [Fact]
    public async Task ResgateApp_RealizarResgate_ComSucesso()
    {
        //Arrange
        var resgate = new Resgate(_idInvestimento, _idCliente, _valor, _motivoResgate, _data);
        _resgateRepository.Setup(x => x.RealizarResgate(It.IsAny<Resgate>())).ReturnsAsync(resgate);
        var request = new RealizarResgateViewModel.Request { IdCliente = _idCliente, IdInvestimento = _idInvestimento, Valor = _valor, MotivoResgate = _motivoResgate, DataResgate = _data };
        var app = IniciaApplication();

        //Act
        var response = await app.RealizarResgate(request);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(resgate.Id, response.Id);
        Assert.Equal(resgate.IdCliente, response.IdCliente);
        Assert.Equal(resgate.IdInvestimento, response.IdInvestimento);
        Assert.Equal(resgate.Valor, response.Valor);
        Assert.Equal(resgate.DataResgate, response.DataResgate);
        Assert.Equal(resgate.MotivoResgate, response.MotivoResgate);
    }

    [Fact]
    public async Task ResgateApp_RealizarResgate_ComId_Invalido()
    {
        //Arrange
        var request = new RealizarResgateViewModel.Request { IdCliente = Guid.Empty, IdInvestimento = _idInvestimento, Valor = _valor, MotivoResgate = _motivoResgate, DataResgate = _data };
        var app = IniciaApplication();

        //Assert
        await Assert.ThrowsAsync<DomainException>(() => app.RealizarResgate(request));
    }

    [Fact]
    public async Task ResgateApp_RealizarResgate_ComIdInvestimento_Invalido()
    {
        //Arrange
        var request = new RealizarResgateViewModel.Request { IdCliente = _idCliente, IdInvestimento = Guid.Empty, Valor = _valor, MotivoResgate = _motivoResgate, DataResgate = _data };
        var app = IniciaApplication();

        //Assert
        await Assert.ThrowsAsync<DomainException>(() => app.RealizarResgate(request));
    }

    [Theory]
    [MemberData(nameof(Valor))]
    public async Task ResgateApp_RealizarResgate_ComValor_Invalido(decimal valor)
    {
        //Arrange
        var request = new RealizarResgateViewModel.Request { IdCliente = _idCliente, IdInvestimento = _idInvestimento, Valor = valor, MotivoResgate = _motivoResgate, DataResgate = _data };
        var app = IniciaApplication();

        //Assert
        await Assert.ThrowsAsync<DomainException>(() => app.RealizarResgate(request));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ResgateApp_RealizarResgate_ComMotivoResgate_Invalido(string motivoResgate)
    {
        //Arrange
        var request = new RealizarResgateViewModel.Request { IdCliente = _idCliente, IdInvestimento = _idInvestimento, Valor = _valor, MotivoResgate = motivoResgate, DataResgate = _data };
        var app = IniciaApplication();

        //Assert
        await Assert.ThrowsAsync<DomainException>(() => app.RealizarResgate(request));
    }

    [Theory]
    [MemberData(nameof(Data))]
    public async Task ResgateApp_RealizarResgate_ComData_Invalida(DateTimeOffset data)
    {
        //Arrange
        var request = new RealizarResgateViewModel.Request { IdCliente = _idCliente, IdInvestimento = _idInvestimento, Valor = _valor, MotivoResgate = _motivoResgate, DataResgate = data };
        var app = IniciaApplication();

        //Assert
        await Assert.ThrowsAsync<DomainException>(() => app.RealizarResgate(request));
    }
}