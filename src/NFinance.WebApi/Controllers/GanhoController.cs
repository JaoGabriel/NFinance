using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.GanhoViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GanhoController : ControllerBase
    {
        private readonly IGanhoService _ganhoService;
        private readonly ILogger<GanhoController> _logger;

        public GanhoController(ILogger<GanhoController> logger, IGanhoService ganhoService)
        {
            _logger = logger;
            _ganhoService = ganhoService;
        }

        [HttpGet("/api/Ganhos/Listar")]
        [Authorize]
        [ProducesResponseType(typeof(ListarGanhosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarGanhos()
        {
            try
            {
                var response = await _ganhoService.ListarGanhos();
                _logger.LogInformation("Ganhos Listados Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao listar ganhos");
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Ganho/Consultar{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarGanho(Guid id)
        {
            try
            {
                var ganho = await _ganhoService.ConsultarGanho(id);
                _logger.LogInformation("Ganho Encontrado Com Sucesso!");
                return Ok(ganho);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar ganho");
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Ganhos/Consultar/{idCliente}")]
        [Authorize]
        [ProducesResponseType(typeof(ConsultarGanhosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarGanhos(Guid idCliente)
        {
            try
            {
                var ganhos = await _ganhoService.ConsultarGanhos(idCliente);
                _logger.LogInformation("Ganhos Encontrados Com Sucesso!");
                return Ok(ganhos);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar");
                return BadRequest(ex);
            }
        }

        [HttpPost("/api/Ganho/Cadastrar")]
        [Authorize]
        [ProducesResponseType(typeof(CadastrarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CadastrarGanho([FromBody] CadastrarGanhoViewModel.Request ganhoRequest)
        {
            try
            {
                var ganhoResponse = await _ganhoService.CadastrarGanho(ganhoRequest);
                _logger.LogInformation("Ganho Cadastrado Com Sucesso!");
                return Ok(ganhoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao cadastrar ganho");
                return BadRequest(ex);
            }
        }

        [HttpPut("/api/Ganho/Atualizar/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(AtualizarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AtualizarGanho(Guid id, [FromBody] AtualizarGanhoViewModel.Request ganhoRequest)
        {
            try
            {
                var ganhoResponse = await _ganhoService.AtualizarGanho(id, ganhoRequest);
                _logger.LogInformation("Ganho Atualizado Com Sucesso!");
                return Ok(ganhoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao atualizar ganho");
                return BadRequest(ex);
            }
        }

        [HttpDelete("/api/Ganho/Excluir")]
        [Authorize]
        [ProducesResponseType(typeof(ExcluirGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExcluirGanho([FromBody] ExcluirGanhoViewModel.Request request)
        {
            try
            {
                var ganhoResponse = await _ganhoService.ExcluirGanho(request);
                _logger.LogInformation("Ganho Excluido Com Sucesso!");
                return Ok(ganhoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao excluir ganho");
                return BadRequest(ex);
            }
        }
    }
}