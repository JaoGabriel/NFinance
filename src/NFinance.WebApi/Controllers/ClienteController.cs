using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.ClientesViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ILogger<ClienteController> logger, IClienteService clienteService)
        {
            _logger = logger;
            _clienteService = clienteService;
        }

        [HttpGet("/api/Cliente/Consultar/{id}")]
        [ProducesResponseType(typeof(ConsultarClienteViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarCliente(Guid id)
        {
            try
            {
                var cliente = await _clienteService.ConsultarCliente(id);
                _logger.LogInformation("Clientes Encontrado Com Sucesso!");
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao consultar cliente");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/Cliente/Cadastrar")]
        [ProducesResponseType(typeof(CadastrarClienteViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarCliente([FromBody] CadastrarClienteViewModel.Request request)
        {
            try
            {
                var response = await _clienteService.CadastrarCliente(request);
                _logger.LogInformation("Clientes Cadastrado Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao cadastrar cliente");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/api/Cliente/Atualizar/{id}")]
        [ProducesResponseType(typeof(AtualizarClienteViewModel.Response), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int) HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> AtualizarCliente(Guid id, [FromBody] AtualizarClienteViewModel.Request request)
        {
            try
            {
                var response = await _clienteService.AtualizarCliente(id, request);
                _logger.LogInformation("Cliente Atualizado Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Falha ao atualizar cliente");
                return BadRequest(ex.Message);
            }
        }
    }
}