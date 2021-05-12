using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.GanhoViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GanhoController : ControllerBase
    {
        private readonly IGanhoService _ganhoService;
        private readonly ILogger<GanhoController> _logger;
        private readonly IAutenticacaoService _autenticacaoService;

        public GanhoController(ILogger<GanhoController> logger, IGanhoService ganhoService, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _ganhoService = ganhoService;
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("Ganho/Consultar{id}")]
        [ProducesResponseType(typeof(ConsultarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarGanho([FromHeader]string authorization, Guid id)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var ganho = await _ganhoService.ConsultarGanho(id);
                _logger.LogInformation("Ganho Encontrado Com Sucesso!");
                return Ok(ganho);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar ganho");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Ganhos/Consultar/{idCliente}")]
        [ProducesResponseType(typeof(ConsultarGanhosViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarGanhos([FromHeader]string authorization, Guid idCliente)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var ganhos = await _ganhoService.ConsultarGanhos(idCliente);
                _logger.LogInformation("Ganhos Encontrados Com Sucesso!");
                return Ok(ganhos);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao consultar");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Ganho/Cadastrar")]
        [ProducesResponseType(typeof(CadastrarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> CadastrarGanho(CadastrarGanhoViewModel.Request ganhoRequest)
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
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Ganho/Atualizar/{id}")]
        [ProducesResponseType(typeof(AtualizarGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> AtualizarGanho([FromHeader]string authorization, Guid id, AtualizarGanhoViewModel.Request ganhoRequest)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var ganhoResponse = await _ganhoService.AtualizarGanho(id, ganhoRequest);
                _logger.LogInformation("Ganho Atualizado Com Sucesso!");
                return Ok(ganhoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao atualizar ganho");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Ganho/Excluir")]
        [ProducesResponseType(typeof(ExcluirGanhoViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ExcluirGanho([FromHeader]string authorization, ExcluirGanhoViewModel.Request request)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var ganhoResponse = await _ganhoService.ExcluirGanho(request);
                _logger.LogInformation("Ganho Excluido Com Sucesso!");
                return Ok(ganhoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao excluir ganho");
                return BadRequest(ex.Message);
            }
        }
    }
}