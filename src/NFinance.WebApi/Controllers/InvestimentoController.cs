using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.ViewModel.InvestimentosViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestimentoController : ControllerBase
    {
        private readonly IInvestimentoService _investimentosService;
        private readonly ILogger<InvestimentoController> _logger;

        public InvestimentoController(ILogger<InvestimentoController> logger,
            IInvestimentoService investimentosService)
        {
            _logger = logger;
            _investimentosService = investimentosService;
        }

        [HttpGet("/api/Investimento/Consultar/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarInvestimentoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarInvestimento(Guid id)
        {
            try
            {
                var investimento = await _investimentosService.ConsultarInvestimento(id);
                _logger.LogInformation("Investimento Encontrado Com Sucesso!");
                return Ok(investimento);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao consultar investimento");
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Investimentos/Consultar/{idCliente}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarInvestimentosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarInvestimentos(Guid idCliente)
        {
            try
            {
                var investimentos = await _investimentosService.ConsultarInvestimentos(idCliente);
                _logger.LogInformation("Investimentos Encontrados Com Sucesso!");
                return Ok(investimentos);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar");
                return BadRequest(ex);
            }
        }

        [HttpPost("/api/Investimento/Investir")]
        [Authorize]
        [ProducesResponseType(typeof(RealizarInvestimentoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RealizarInvestimento(
            [FromBody] RealizarInvestimentoViewModel.Request investimentosRequest)
        {
            try
            {
                var investimentoResponse = await _investimentosService.RealizarInvestimento(investimentosRequest);
                _logger.LogInformation("Investimento Realizado Com Sucesso!");
                return Ok(investimentoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao cadastrar investimento");
                return BadRequest(ex);
            }
        }

        [HttpPut("/api/Investimento/Atualizar/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(AtualizarInvestimentoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AtualizarInvestimento(Guid id,
            [FromBody] AtualizarInvestimentoViewModel.Request investimentosRequest)
        {
            try
            {
                var investimentoResponse = await _investimentosService.AtualizarInvestimento(id, investimentosRequest);
                _logger.LogInformation("Investimento Atualizado Com Sucesso!");
                return Ok(investimentoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao atualizar investimento");
                return BadRequest(ex);
            }
        }
    }
}