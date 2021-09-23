using Xunit;
using System;
using NFinance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NFinance.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NFinance.Domain.Identidade;

namespace NFinance.Tests.WebApi
{
    public class ResgatesControllerTests
    {
        private readonly Mock<IResgateApp> _resgateApp;
        private readonly Mock<ILogger<ResgateController>> _logger;

        public ResgatesControllerTests()
        {
            _resgateApp = new Mock<IResgateApp>();
            _logger = new Mock<ILogger<ResgateController>>();
        }

        private ResgateController InicializarResgateController()
        {
            return new(_logger.Object, _resgateApp.Object);
        }

        public static Resgate GeraResgate()
        {
            return new(Guid.NewGuid(),Guid.NewGuid(),32138123987.20138M,"asuhdasud",DateTime.Today);
        }

        private static Usuario GeraUsuario()
        {
            return new() { Id = Guid.NewGuid(), Email = "teste@teste.com", PasswordHash = "123456" };
        }

        public static Cliente GeraCliente()
        {
            return new("ASDASD", "12345678910", "teste@tst.com", GeraUsuario());
        }

        [Fact]
        public void ResgateController_RealizarResgate_ComSucesso()
        {
            //Arrange
            var resgate = GeraResgate();
            _resgateApp.Setup(x => x.RealizarResgate(It.IsAny<RealizarResgateViewModel.Request>())).ReturnsAsync(new RealizarResgateViewModel.Response(resgate));
            var controller = InicializarResgateController();
            var resgateRequest = new RealizarResgateViewModel.Request();
            var token = TokenApp.GerarToken(GeraUsuario());
            
            //Act
            var teste = controller.RealizarResgate(token, resgateRequest);
            var okResult = teste.Result as ObjectResult;
            var realizarResgateViewModel = Assert.IsType<RealizarResgateViewModel.Response>(okResult.Value);
            
            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(resgate.Id, realizarResgateViewModel.Id);
            Assert.Equal(resgate.MotivoResgate, realizarResgateViewModel.MotivoResgate);
            Assert.Equal(resgate.Valor, realizarResgateViewModel.Valor);
            Assert.Equal(resgate.DataResgate, realizarResgateViewModel.DataResgate);
            Assert.Equal(resgate.IdInvestimento, realizarResgateViewModel.IdInvestimento);
        }

        [Fact]
        public void ResgateController_ConsultarResgate_ComSucesso()
        {
            //Arrange
            var resgate = GeraResgate();
            _resgateApp.Setup(x => x.ConsultarResgate(It.IsAny<Guid>())).ReturnsAsync(new ConsultarResgateViewModel.Response(resgate));
            var controller = InicializarResgateController();

            //Act
            var token = TokenApp.GerarToken(GeraUsuario());
            var teste = controller.ConsultarResgate(token,resgate.Id);
            var okResult = teste.Result as ObjectResult;
            var consultarResgateViewModel = Assert.IsType<ConsultarResgateViewModel.Response>(okResult.Value);
            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(resgate.Id, consultarResgateViewModel.Id);
            Assert.Equal(resgate.MotivoResgate, consultarResgateViewModel.MotivoResgate);
            Assert.Equal(resgate.Valor, consultarResgateViewModel.Valor);
            Assert.Equal(DateTime.Today, consultarResgateViewModel.DataResgate);
            Assert.Equal(resgate.IdCliente, consultarResgateViewModel.IdCliente);
            Assert.Equal(resgate.IdInvestimento, consultarResgateViewModel.IdInvestimento);
        }

        [Fact]
        public void ResgateController_ConsultarResgates_ComSucesso()
        {
            //Arrange
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var idInvestimento = Guid.NewGuid();
            var motivoResgate = "uasduhasha";
            var valor = 1238.12M;
            var dataResgate = DateTime.Today;
            var listaResgate = new List<Resgate>();
            var gasto = new Resgate
            {
                Id = id,
                IdInvestimento = idInvestimento,
                MotivoResgate = motivoResgate,
                Valor = valor,
                DataResgate = dataResgate
            };
            var gasto1 = new Resgate
            {
                Id = id1,
                IdInvestimento = idInvestimento,
                MotivoResgate = motivoResgate,
                Valor = valor,
                DataResgate = dataResgate
            };
            listaResgate.Add(gasto);
            listaResgate.Add(gasto1);
            var listarResgates = new ConsultarResgatesViewModel.Response(listaResgate);
            _resgateApp.Setup(x => x.ConsultarResgates(It.IsAny<Guid>())).ReturnsAsync(listarResgates);
            var controller = InicializarResgateController();
            var token = TokenApp.GerarToken(GeraUsuario());

            //Act
            var teste = controller.ConsultarResgates(token,idInvestimento);
            var okResult = teste.Result as ObjectResult;
            var consultarResgatesViewModel = Assert.IsType<ConsultarResgatesViewModel.Response>(okResult.Value);

            //Assert
            Assert.NotNull(teste);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            //verificar o gasto
            var resgateTest = consultarResgatesViewModel.Find(g => g.Id == id);
            Assert.Equal(id, resgateTest.Id);
            Assert.Equal(idInvestimento, resgateTest.IdInvestimento);
            Assert.Equal(motivoResgate, resgateTest.MotivoResgate);
            Assert.Equal(valor, resgateTest.Valor);
            Assert.Equal(dataResgate, resgateTest.DataResgate);
            //verificar o gasto1
            var resgateTest1 = consultarResgatesViewModel.Find(g => g.Id == id1);
            Assert.Equal(id1, resgateTest1.Id);
            Assert.Equal(idInvestimento, resgateTest1.IdInvestimento);
            Assert.Equal(motivoResgate, resgateTest1.MotivoResgate);
            Assert.Equal(valor, resgateTest1.Valor);
            Assert.Equal(dataResgate, resgateTest1.DataResgate);
        }
    }
}
