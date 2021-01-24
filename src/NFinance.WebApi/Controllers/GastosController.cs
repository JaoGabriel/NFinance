using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.GastosViewModel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly ILogger<GastosController> _logger;
        private readonly IGastosService _gastosService;

        public GastosController(ILogger<GastosController> logger, IGastosService gastosService)
        {
            _logger = logger;
            _gastosService = gastosService;
        }

        [HttpGet("/api/Gastos")]
        [ProducesResponseType(typeof(ListarGastosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarGastos()
        {
            try
            {
                var response = await _gastosService.ListarGastos();
                _logger.LogInformation("Gasto Listados Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao listar gasto", ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Gasto/{id}")]
        [ProducesResponseType(typeof(ConsultarGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
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
                _logger.LogInformation("Falha ao consultar gasto", ex);
                return BadRequest(ex);
            }
        }

        [HttpPost("/api/Gasto")]
        [ProducesResponseType(typeof(CadastrarGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
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
                _logger.LogInformation("Falha ao cadastrar gasto", ex);
                return BadRequest(ex);
            }
        }

        [HttpPut("/api/Gasto/{id}")]
        [ProducesResponseType(typeof(AtualizarGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
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
                _logger.LogInformation("Falha ao atualizar gasto", ex);
                return BadRequest(ex);
            }
        }

        [HttpDelete("/api/Gasto/{id}")]
        [ProducesResponseType(typeof(ExcluirGastoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
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
                _logger.LogInformation("Falha ao atualizar gasto", ex);
                return BadRequest(ex);
            }
        }
    }
}
