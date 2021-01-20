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
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarResgates()
        {
            List<Resgate> listaResgates = new List<Resgate>();
            try
            {
                var resgates = await _resgateService.ListarResgates();
                foreach (var resgate in resgates)
                    listaResgates.Add(resgate);

                _logger.LogInformation("Resgates Listados Com Sucesso!");
                return Ok(listaResgates);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao listar resgates", ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Resgate/{id}")]
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.BadRequest)]
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
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Resgate), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CadastrarResgate(Guid idInvestimentos, [FromBody] Resgate resgate)
        {
            try
            {
                var resgateResponse = await _resgateService.RealizarResgate(idInvestimentos, resgate);
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
