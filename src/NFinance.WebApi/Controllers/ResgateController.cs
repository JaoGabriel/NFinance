using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResgateController : ControllerBase
    {
        private readonly IResgateApp _resgateApp;
        private readonly ILogger<ResgateController> _logger;

        public ResgateController(ILogger<ResgateController> logger, IResgateApp resgateApp)
        {
            _logger = logger;
            _resgateApp = resgateApp;
        }

        [HttpGet("Resgate/Consultar/{id}")]
        [ProducesResponseType(typeof(ConsultarResgateViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarResgate([FromHeader] string authorization, Guid id)
        {
            var resgates = await _resgateApp.ConsultarResgate(id);
            _logger.LogInformation("Resgate Encontrado Com Sucesso!");
            return Ok(resgates);
        }

        [HttpGet("Resgates/Consultar/{idCliente}")]
        [ProducesResponseType(typeof(ConsultarResgatesViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarResgates([FromHeader] string authorization, Guid idCliente)
        {
            var resgates = await _resgateApp.ConsultarResgates(idCliente);
            _logger.LogInformation("Resgates Encontrados Com Sucesso!");
            return Ok(resgates);
        }

        [HttpPost("Resgate/Resgatar")]
        [ProducesResponseType(typeof(RealizarResgateViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> RealizarResgate([FromHeader] string authorization, RealizarResgateViewModel.Request resgateRequest)
        {
            var resgateResponse = await _resgateApp.RealizarResgate(resgateRequest);
            _logger.LogInformation("Resgate Realizado Com Sucesso!");
            return Ok(resgateResponse);
        }
    }
}