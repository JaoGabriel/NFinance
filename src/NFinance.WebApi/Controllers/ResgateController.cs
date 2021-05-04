using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.ViewModel.ResgatesViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResgateController : ControllerBase
    {
        private readonly IResgateService _resgateService;
        private readonly ILogger<ResgateController> _logger;
        private readonly IAutenticacaoService _autenticacaoService;

        public ResgateController(ILogger<ResgateController> logger, IResgateService resgateService, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _resgateService = resgateService;
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("/api/Resgate/Consultar/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarResgate([FromHeader]string authorization, Guid id)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var resgates = await _resgateService.ConsultarResgate(id);
                _logger.LogInformation("Resgate Encontrado Com Sucesso!");
                return Ok(resgates);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar resgate");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/Resgates/Consultar/{idCliente}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarResgatesViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarResgates([FromHeader]string authorization, Guid idCliente)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var resgates = await _resgateService.ConsultarResgates(idCliente);
                _logger.LogInformation("Resgates Encontrados Com Sucesso!");
                return Ok(resgates);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/Resgate/Resgatar")]
        [Authorize]
        [ProducesResponseType(typeof(RealizarResgateViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RealizarResgate([FromHeader]string authorization, RealizarResgateViewModel.Request resgateRequest)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var resgateResponse = await _resgateService.RealizarResgate(resgateRequest);
                _logger.LogInformation("Resgate Realizado Com Sucesso!");
                return Ok(resgateResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao cadastrar investimento");
                return BadRequest(ex.Message);
            }
        }
    }
}