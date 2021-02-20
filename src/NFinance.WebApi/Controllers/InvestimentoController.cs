using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.InvestimentosViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestimentoController : ControllerBase
    {
        private readonly IInvestimentosService _investimentosService;
        private readonly ILogger<InvestimentoController> _logger;

        public InvestimentoController(ILogger<InvestimentoController> logger,
            IInvestimentosService investimentosService)
        {
            _logger = logger;
            _investimentosService = investimentosService;
        }

        [HttpGet("/api/Investimentos/Listar")]
        [ProducesResponseType(typeof(ListarInvestimentosViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarInvestimentos()
        {
            try
            {
                var response = await _investimentosService.ListarInvestimentos();
                _logger.LogInformation("Investimentos Listados Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao listar investimentos");
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Investimento/Consultar/{id}")]
        [ProducesResponseType(typeof(ConsultarInvestimentoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarInvestimentos(Guid id)
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

        [HttpPost("/api/Investimento/Investir")]
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