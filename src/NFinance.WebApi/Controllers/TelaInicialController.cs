using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.TelaInicialViewModel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TelaInicialController : ControllerBase
    {
        private readonly ITelaInicialService _telaInicialService;

        private readonly ILogger<TelaInicialController> _logger;

        public TelaInicialController(ILogger<TelaInicialController> logger, ITelaInicialService telaInicialService)
        {
            _logger = logger;
            _telaInicialService = telaInicialService;
        }

        [HttpGet("/api/TelaInicial/{idCliente}")]
        [Authorize]
        [ProducesResponseType(typeof(TelaInicialViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> TelaInicial(Guid idCliente)
        {
            try
            {
                var response = await _telaInicialService.TelaInicial(idCliente);
                _logger.LogInformation("Tela Inicial Carregada Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao carregar tela inicial");
                return BadRequest(ex.Message);
            }
        }
    }
}
