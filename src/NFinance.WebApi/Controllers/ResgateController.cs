using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.ResgatesViewModel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResgateController : ControllerBase
    {
        private readonly ILogger<ResgateController> _logger;
        private readonly IResgateService _resgateService;

        public ResgateController(ILogger<ResgateController> logger, IResgateService resgateService)
        {
            _logger = logger;
            _resgateService = resgateService;
        }

        [HttpGet("/api/Resgates")]
        [ProducesResponseType(typeof(ListarResgatesViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarResgates()
        {
            try
            {
                var response = await _resgateService.ListarResgates();
                _logger.LogInformation("Resgates Listados Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao listar resgates", ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Resgate/{id}")]
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsultarResgate(Guid id)
        {
            try
            {
                var resgates = await _resgateService.ConsultarResgate(id);
                _logger.LogInformation("Resgate Encontrado Com Sucesso!");
                return Ok(resgates);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao consultar resgate", ex);
                return BadRequest(ex);
            }
        }

        [HttpPost("/api/Resgate")]
        [ProducesResponseType(typeof(RealizarResgateViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CadastrarResgate([FromBody] RealizarResgateViewModel.Request resgateRequest)
        {
            try
            {
                var resgateResponse = await _resgateService.RealizarResgate(resgateRequest);
                _logger.LogInformation("Resgate Realizado Com Sucesso!");
                return Ok(resgateResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao cadastrar investimento", ex);
                return BadRequest(ex);
            }
        }
    }
}
