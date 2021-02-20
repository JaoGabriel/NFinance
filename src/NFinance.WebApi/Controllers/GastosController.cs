using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.GastosViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly IGastosService _gastosService;
        private readonly ILogger<GastosController> _logger;

        public GastosController(ILogger<GastosController> logger, IGastosService gastosService)
        {
            _logger = logger;
            _gastosService = gastosService;
        }

        [HttpGet("/api/Gastos/Listar")]
        [ProducesResponseType(typeof(ListarGastosViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
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
                _logger.LogInformation(ex,"Falha ao listar gasto");
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Gasto/Consultar/{id}")]
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
                return BadRequest(ex);
            }
        }

        [HttpPost("/api/Gasto/Cadastar")]
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
                _logger.LogInformation(ex,"Falha ao cadastrar gasto");
                return BadRequest(ex);
            }
        }

        [HttpPut("/api/Gasto/Atualizar/{id}")]
        [ProducesResponseType(typeof(AtualizarGastoViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AtualizarGasto(Guid id,
            [FromBody] AtualizarGastoViewModel.Request gastosRequest)
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
                return BadRequest(ex);
            }
        }

        [HttpDelete("/api/Gasto/Excluir")]
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
                return BadRequest(ex);
            }
        }
    }
}