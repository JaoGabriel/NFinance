using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestimentoController : ControllerBase
    {
        private readonly ILogger<InvestimentoController> _logger;
        private readonly IInvestimentosService _investimentosService;

        public InvestimentoController(ILogger<InvestimentoController> logger, IInvestimentosService investimentosService)
        {
            _logger = logger;
            _investimentosService = investimentosService;
        }

        [HttpGet("/api/Investimentos")]
        [ProducesResponseType(typeof(Investimentos), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Investimentos), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarInvestimentos()
        {
            List<Investimentos> listaInvestimentos = new List<Investimentos>();
            try
            {
                var investimentos = await _investimentosService.ListarInvestimentos();
                foreach (var investimento in investimentos)
                    listaInvestimentos.Add(investimento);

                _logger.LogInformation("Investimentos Listados Com Sucesso!");
                return Ok(listaInvestimentos);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao listar investimentos", ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Investimento/{id}")]
        [ProducesResponseType(typeof(Investimentos), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Investimentos), (int)HttpStatusCode.BadRequest)]
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
                _logger.LogInformation("Falha ao consultar investimento", ex);
                return BadRequest(ex);
            }
        }

        [HttpPost("/api/Investimento")]
        [ProducesResponseType(typeof(Investimentos), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Investimentos), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CadastrarInvestimento([FromBody] Investimentos investimentos)
        {
            try
            {
                var investimentoResponse = await _investimentosService.RealizarInvestimento(investimentos);
                _logger.LogInformation("Investimento Realizado Com Sucesso!");
                return Ok(investimentoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao cadastrar investimento", ex);
                return BadRequest(ex);
            }
        }

        [HttpPut("/api/Investimento/{id}")]
        [ProducesResponseType(typeof(Investimentos), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Investimentos), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AtualizarInvestimento(Guid id, [FromBody] Investimentos investimentos)
        {
            try
            {
                var investimentoResponse = await _investimentosService.AtualizarInvestimento(id, investimentos);
                _logger.LogInformation("Investimento Atualizado Com Sucesso!");
                return Ok(investimentoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao atualizar investimento", ex);
                return BadRequest(ex);
            }
        }
    }
}
