using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.TelaInicialViewModel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TelaInicialController : ControllerBase
    {
        private readonly ITelaInicialService _telaInicialService;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<TelaInicialController> _logger;

        public TelaInicialController(ILogger<TelaInicialController> logger, ITelaInicialService telaInicialService, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _telaInicialService = telaInicialService;
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("/api/TelaInicial/{idCliente}")]
        [Authorize]
        [ProducesResponseType(typeof(TelaInicialViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> TelaInicial([FromHeader] string authorization, Guid idCliente)
        {
            try
            {
                _logger.LogInformation("Validando Bearer Token!");
                await _autenticacaoService.ValidaTokenRequest(authorization);
                _logger.LogInformation("Bearer Token Validado!");
                var response = await _telaInicialService.TelaInicial(idCliente);
                _logger.LogInformation("Tela Inicial Carregada Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Falha ao carregar tela inicial");
                return BadRequest(ex.Message);
            }
        }
    }
}
