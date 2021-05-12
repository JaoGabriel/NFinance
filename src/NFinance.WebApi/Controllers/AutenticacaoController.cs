using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NFinance.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.AutenticacaoViewModel;

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

        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationException), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DomainException), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> Autenticar(LoginViewModel request)
        {
            _logger.LogInformation("Iniciando Login!");
            var response = await _autenticacaoService.RealizarLogin(request.Email,request.Senha);
            _logger.LogInformation("Login Realizado Com Sucesso!");
            return Ok(response);
        }

        [HttpPost("Logout/{id}")]
        [ProducesResponseType(typeof(LogoutViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationException), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DomainException), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Deslogar([FromHeader] string authorization, Guid id)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoService.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            _logger.LogInformation("Iniciando Logout!");
            var response = await _autenticacaoService.RealizarLogut(id);
            _logger.LogInformation("Logot Realizado Com Sucesso!");
            return Ok(response);
        }
    }
}
