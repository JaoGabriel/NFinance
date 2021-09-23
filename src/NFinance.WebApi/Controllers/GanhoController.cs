using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GanhoController : ControllerBase
    {
        private readonly IGanhoApp _ganhoApp;
        private readonly ILogger<GanhoController> _logger;

        public GanhoController(ILogger<GanhoController> logger, IGanhoApp ganhoApp)
        {
            _logger = logger;
            _ganhoApp = ganhoApp;
        }

        [HttpGet("Ganho/Consultar/{id}")]
        [ProducesResponseType(typeof(ConsultarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarGanho([FromHeader] string authorization, Guid id)
        {
            var ganho = await _ganhoApp.ConsultarGanho(id);
            _logger.LogInformation("Ganho Encontrado Com Sucesso!");
            return Ok(ganho);
        }

        [HttpGet("Ganhos/Consultar/{idCliente}")]
        [ProducesResponseType(typeof(ConsultarGanhosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarGanhos([FromHeader] string authorization, Guid idCliente)
        {
            var ganhos = await _ganhoApp.ConsultarGanhos(idCliente);
            _logger.LogInformation("Ganhos Encontrados Com Sucesso!");
            return Ok(ganhos);
        }

        [HttpPost("Ganho/Cadastrar")]
        [ProducesResponseType(typeof(CadastrarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> CadastrarGanho(CadastrarGanhoViewModel.Request ganhoRequest)
        {
            var ganhoResponse = await _ganhoApp.CadastrarGanho(ganhoRequest);
            _logger.LogInformation("Ganho Cadastrado Com Sucesso!");
            return Ok(ganhoResponse);
        }

        [HttpPut("Ganho/Atualizar/{id}")]
        [ProducesResponseType(typeof(AtualizarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> AtualizarGanho([FromHeader] string authorization, Guid id, AtualizarGanhoViewModel.Request ganhoRequest)
        {
            var ganhoResponse = await _ganhoApp.AtualizarGanho(id, ganhoRequest);
            _logger.LogInformation("Ganho Atualizado Com Sucesso!");
            return Ok(ganhoResponse);
        }

        [HttpDelete("Ganho/Excluir")]
        [ProducesResponseType(typeof(ExcluirGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ExcluirGanho([FromHeader] string authorization, ExcluirGanhoViewModel.Request request)
        {
            var ganhoResponse = await _ganhoApp.ExcluirGanho(request);
            _logger.LogInformation("Ganho Excluido Com Sucesso!");
            return Ok(ganhoResponse);
        }
    }
}