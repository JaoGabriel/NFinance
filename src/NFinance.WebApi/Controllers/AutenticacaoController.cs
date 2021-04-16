using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<AutenticacaoController> _logger;

        public AutenticacaoController(ILogger<AutenticacaoController> logger, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("/api/Login")]
        [ProducesResponseType(typeof(LoginViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Autenticar([FromBody] LoginViewModel request)
        {
            try
            {
                _logger.LogInformation("Iniciando Login!");
                var response = await _autenticacaoService.RealizarLogin(request);
                _logger.LogInformation("Login Realizado Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao efetuar login!");
                return BadRequest(ex.Message);
            }

            // TODO
            // Criar uma validacao de token antes de cada chamada
        }

        [HttpPost("/api/Logout")]
        [ProducesResponseType(typeof(LogoutViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> Deslogar([FromBody] LogoutViewModel request)
        {
            try
            {
                _logger.LogInformation("Iniciando Logout!");
                var response = await _autenticacaoService.RealizarLogut(request);
                _logger.LogInformation("Logot Realizado Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao efetuar logout!");
                return BadRequest(ex.Message);
            }
        }
    }
}
