using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.TelaInicialViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TelaInicialController : ControllerBase
    {
        private readonly ITelaInicialApp _telaInicialApp;
        private readonly ILogger<TelaInicialController> _logger;

        public TelaInicialController(ILogger<TelaInicialController> logger, ITelaInicialApp telaInicialApp)
        {
            _logger = logger;
            _telaInicialApp = telaInicialApp;
        }

        [HttpGet("/api/TelaInicial/{idCliente}")]
        [Authorize]
        [ProducesResponseType(typeof(TelaInicialViewModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> TelaInicial([FromHeader] string authorization, Guid idCliente)
        {
            var response = await _telaInicialApp.TelaInicial(idCliente);
            _logger.LogInformation("Tela Inicial Carregada Com Sucesso!");
            return Ok(response);
        }
    }
}