using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using NFinance.Application.ViewModel.GastosViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly IGastoApp _gastoApp;
        private readonly ILogger<GastosController> _logger;
        private readonly IAutenticacaoApp _autenticacaoApp;

        public GastosController(ILogger<GastosController> logger, IGastoApp gastoApp, IAutenticacaoApp autenticacaoApp)
        {
            _logger = logger;
            _gastoApp = gastoApp;
            _autenticacaoApp = autenticacaoApp;
        }

        [HttpGet("Gasto/Consultar/{id}")]
        [ProducesResponseType(typeof(ConsultarGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarGasto([FromHeader] string authorization, Guid id)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var gasto = await _gastoApp.ConsultarGasto(id);
            _logger.LogInformation("Gasto Encontrado Com Sucesso!");
            return Ok(gasto);
        }

        [HttpGet("Gastos/Consultar/{idCliente}")]
        [ProducesResponseType(typeof(ConsultarGastosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarGastos([FromHeader] string authorization, Guid idCliente)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var gastos = await _gastoApp.ConsultarGastos(idCliente);
            _logger.LogInformation("Gastos Encontrados Com Sucesso!");
            return Ok(gastos);
        }

        [HttpPost("Gasto/Cadastar")]
        [ProducesResponseType(typeof(CadastrarGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> CadastrarGasto([FromHeader] string authorization, CadastrarGastoViewModel.Request gastosRequest)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var gastoResponse = await _gastoApp.CadastrarGasto(gastosRequest);
            _logger.LogInformation("Gasto Cadastrado Com Sucesso!");
            return Ok(gastoResponse);
        }

        [HttpPut("Gasto/Atualizar/{id}")]
        [ProducesResponseType(typeof(AtualizarGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> AtualizarGasto([FromHeader] string authorization, Guid id, AtualizarGastoViewModel.Request gastosRequest)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var gastoResponse = await _gastoApp.AtualizarGasto(id, gastosRequest);
            _logger.LogInformation("Gasto Atualizado Com Sucesso!");
            return Ok(gastoResponse);
        }

        [HttpDelete("Gasto/Excluir")]
        [ProducesResponseType(typeof(ExcluirGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ExcluirGasto([FromHeader] string authorization, ExcluirGastoViewModel.Request request)
        {

            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoApp.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var gastoResponse = await _gastoApp.ExcluirGasto(request);
            _logger.LogInformation("Gasto Atualizado Com Sucesso!");
            return Ok(gastoResponse);
        }
    }
}