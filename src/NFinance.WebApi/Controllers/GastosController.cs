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
        [ProducesResponseType(typeof(Gastos), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Gastos), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarGastos()
        {
            List<Gastos> listaGastos = new List<Gastos>();
            try
            {
                var gastos = await _gastosService.ListarGastos();
                foreach (var gasto in gastos)
                    listaGastos.Add(gasto);

                _logger.LogInformation("Gasto Listados Com Sucesso!");
                return Ok(listaGastos);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao listar gasto", ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Gasto/{id}")]
        [ProducesResponseType(typeof(Gastos), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Gastos), (int)HttpStatusCode.BadRequest)]
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
        [ProducesResponseType(typeof(Gastos), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Gastos), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CadastrarGasto([FromBody] Gastos gastos)
        {
            try
            {
                var gastoResponse = await _gastosService.CadastrarGasto(gastos);
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
        public async Task<IActionResult> AtualizarGasto(Guid id, [FromBody] Gastos gastos)
        {
            try
            {
                var gastoResponse = await _gastosService.AtualizarGasto(id, gastos);
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
