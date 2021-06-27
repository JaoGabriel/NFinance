using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NFinance.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using NFinance.Application.ViewModel.AutenticacaoViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacaoController : ControllerBase
    {

        private readonly IAutenticacaoApp _autenticacaoApp;
        private readonly ILogger<AutenticacaoController> _logger;

        public AutenticacaoController(ILogger<AutenticacaoController> logger, IAutenticacaoApp autenticacaoApp)
        {
            _logger = logger;
            _autenticacaoApp = autenticacaoApp;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationException), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DomainException), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> Autenticar(LoginViewModel request)
        {
            _logger.LogInformation($"Iniciando Login de {request.Email}!");
            return Ok(await _autenticacaoApp.EfetuarLogin(request));
        }

        [HttpPost("Logout/{id}")]
        [ProducesResponseType(typeof(LogoutViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationException), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DomainException), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Deslogar([FromHeader] string authorization, LogoutViewModel logout)
        {
            logout.Token = authorization;
            _logger.LogInformation("Bearer Token Validado!");
            _logger.LogInformation($"Iniciando Logout de {logout.IdCliente}!");
            var response = await _autenticacaoApp.EfetuarLogoff(logout);
            return Ok(response);
        }
    }
}
