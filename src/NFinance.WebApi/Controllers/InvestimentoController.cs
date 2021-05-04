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
        private readonly ILogger<InvestimentoController> _logger;
        private readonly IInvestimentoService _investimentosService;
        private readonly IAutenticacaoService _autenticacaoService;

        public InvestimentoController(ILogger<InvestimentoController> logger, IInvestimentoService investimentosService, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _autenticacaoService = autenticacaoService;
            _investimentosService = investimentosService;
        }

        [HttpGet("/api/Investimento/Consultar/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarInvestimentoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarInvestimento([FromHeader]string authorization, Guid id)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
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
        public async Task<IActionResult> ConsultarInvestimentos([FromHeader]string authorization, Guid idCliente)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
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
        public async Task<IActionResult> RealizarInvestimento([FromHeader]string authorization, RealizarInvestimentoViewModel.Request investimentosRequest)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
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
        public async Task<IActionResult> AtualizarInvestimento([FromHeader]string authorization, Guid id, AtualizarInvestimentoViewModel.Request investimentosRequest)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
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