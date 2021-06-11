using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestimentoController : ControllerBase
    {
        private readonly ILogger<InvestimentoController> _logger;
        private readonly IInvestimentoApp _investimentoApp;
        private readonly IAutenticacaoApp _autenticacaoApp;

        public InvestimentoController(ILogger<InvestimentoController> logger, IInvestimentoApp investimentoApp, IAutenticacaoApp autenticacaoApp)
        {
            _logger = logger;
            _autenticacaoApp = autenticacaoApp;
            _investimentoApp = investimentoApp;
        }

        [HttpGet("Investimento/Consultar/{id}")]
        [ProducesResponseType(typeof(ConsultarInvestimentoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarInvestimento([FromHeader] string authorization, Guid id)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var investimento = await _investimentoApp.ConsultarInvestimento(id);
            _logger.LogInformation("Investimento Encontrado Com Sucesso!");
            return Ok(investimento);
        }

        [HttpGet("Investimentos/Consultar/{idCliente}")]
        [ProducesResponseType(typeof(ConsultarInvestimentosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarInvestimentos([FromHeader] string authorization, Guid idCliente)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var investimentos = await _investimentoApp.ConsultarInvestimentos(idCliente);
            _logger.LogInformation("Investimentos Encontrados Com Sucesso!");
            return Ok(investimentos);
        }

        [HttpPost("Investimento/Investir")]
        [ProducesResponseType(typeof(RealizarInvestimentoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> RealizarInvestimento([FromHeader] string authorization, RealizarInvestimentoViewModel.Request investimentosRequest)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var investimentoResponse = await _investimentoApp.RealizarInvestimento(investimentosRequest);
            _logger.LogInformation("Investimento Realizado Com Sucesso!");
            return Ok(investimentoResponse);
        }

        [HttpPut("Investimento/Atualizar/{id}")]
        [ProducesResponseType(typeof(AtualizarInvestimentoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> AtualizarInvestimento([FromHeader] string authorization, Guid id, AtualizarInvestimentoViewModel.Request investimentosRequest)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var investimentoResponse = await _investimentoApp.AtualizarInvestimento(id, investimentosRequest);
            _logger.LogInformation("Investimento Atualizado Com Sucesso!");
            return Ok(investimentoResponse);
        }
    }
}