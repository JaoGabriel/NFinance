using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.ViewModel.GastosViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly IGastoService _gastosService;
        private readonly ILogger<GastosController> _logger;
        private readonly IAutenticacaoService _autenticacaoService;

        public GastosController(ILogger<GastosController> logger, IGastoService gastosService, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _gastosService = gastosService;
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("/api/Gasto/Consultar/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarGasto([FromHeader] string authorization, Guid id)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var gasto = await _gastosService.ConsultarGasto(id);
                _logger.LogInformation("Gasto Encontrado Com Sucesso!");
                return Ok(gasto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar gasto");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/Gastos/Consultar/{idCliente}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarGastosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarGastos([FromHeader] string authorization, Guid idCliente)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var gastos = await _gastosService.ConsultarGastos(idCliente);
                _logger.LogInformation("Gastos Encontrados Com Sucesso!");
                return Ok(gastos);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/Gasto/Cadastar")]
        [Authorize]
        [ProducesResponseType(typeof(CadastrarGastoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CadastrarGasto([FromHeader] string authorization, CadastrarGastoViewModel.Request gastosRequest)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var gastoResponse = await _gastosService.CadastrarGasto(gastosRequest);
                _logger.LogInformation("Gasto Cadastrado Com Sucesso!");
                return Ok(gastoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao cadastrar gasto");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/api/Gasto/Atualizar/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(AtualizarGastoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AtualizarGasto([FromHeader] string authorization, Guid id, AtualizarGastoViewModel.Request gastosRequest)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var gastoResponse = await _gastosService.AtualizarGasto(id, gastosRequest);
                _logger.LogInformation("Gasto Atualizado Com Sucesso!");
                return Ok(gastoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao atualizar gasto");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/api/Gasto/Excluir")]
        [Authorize]
        [ProducesResponseType(typeof(ExcluirGastoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExcluirGasto([FromHeader] string authorization, ExcluirGastoViewModel.Request request)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var gastoResponse = await _gastosService.ExcluirGasto(request);
                _logger.LogInformation("Gasto Atualizado Com Sucesso!");
                return Ok(gastoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao atualizar gasto");
                return BadRequest(ex.Message);
            }
        }
    }
}