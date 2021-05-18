using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.ResgatesViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResgateController : ControllerBase
    {
        private readonly IResgateApp _resgateApp;
        private readonly ILogger<ResgateController> _logger;
        private readonly IAutenticacaoService _autenticacaoService;

        public ResgateController(ILogger<ResgateController> logger, IResgateApp resgateApp, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _resgateApp = resgateApp;
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("Resgate/Consultar/{id}")]
        [ProducesResponseType(typeof(ConsultarResgateViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarResgate([FromHeader] string authorization, Guid id)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoService.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
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
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoService.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
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
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoService.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var resgateResponse = await _resgateApp.RealizarResgate(resgateRequest);
            _logger.LogInformation("Resgate Realizado Com Sucesso!");
            return Ok(resgateResponse);
        }
    }
}