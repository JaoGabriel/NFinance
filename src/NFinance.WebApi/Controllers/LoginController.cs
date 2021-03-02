using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Services;
using NFinance.Domain.ViewModel.LoginViewModel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;

        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, IClienteRepository clienteRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
        }

        [HttpPost("/api/Login")]
        [ProducesResponseType(typeof(LoginViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Autenticar([FromBody]LoginViewModel request)
        {
            try
            {
                _logger.LogInformation("Iniciando Login!");
                var usuario = await _clienteRepository.CredenciaisLogin(request.Email, request.Senha);
                var token = TokenService.GerarToken(usuario);
                var response = new LoginViewModel.Response(usuario,token);
                _logger.LogInformation("Login Realizado Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao efetuar login!");
                return BadRequest(ex);
            }
        }
    }
}
