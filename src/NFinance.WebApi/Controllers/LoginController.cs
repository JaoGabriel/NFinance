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
    public class LoginController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, IAutenticacaoService autenticacaoService)
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
            // Incluir Redis para token de sessao, cada chamada incluir validacao de token de sessao
            // caso seja necessario, atualizar a classe cliente, adicionando uma prop de Token ou Autenticado
        }
    }
}
