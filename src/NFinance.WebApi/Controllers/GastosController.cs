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

        public GastosController(ILogger<GastosController> logger, IGastoService gastosService)
        {
            _logger = logger;
            _gastosService = gastosService;
        }

        [HttpGet("/api/Gasto/Consultar/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarGastoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarGasto(Guid id)
        {
            try
            {
                var gasto = await _gastosService.ConsultarGasto(id);
                _logger.LogInformation("Gasto Encontrado Com Sucesso!");
                return Ok(gasto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao consultar gasto");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/Gastos/Consultar/{idCliente}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarGastosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarGastos(Guid idCliente)
        {
            try
            {
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
        public async Task<IActionResult> CadastrarGasto([FromBody] CadastrarGastoViewModel.Request gastosRequest)
        {
            try
            {
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
        public async Task<IActionResult> AtualizarGasto(Guid id, [FromBody] AtualizarGastoViewModel.Request gastosRequest)
        {
            try
            {
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
        public async Task<IActionResult> ExcluirGasto([FromBody] ExcluirGastoViewModel.Request request)
        {
            try
            {
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