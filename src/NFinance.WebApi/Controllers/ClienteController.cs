using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.ClientesViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {

        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteService _clienteService;

        public ClienteController(ILogger<ClienteController> logger, IClienteService clienteService)
        {
            _logger = logger;
            _clienteService = clienteService;
        }

        [HttpGet("/api/Clientes")]
        [ProducesResponseType(typeof(ListarClientesViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarClientes()
        {
            try
            {
                var response = await _clienteService.ListarClientes();
                _logger.LogInformation("Clientes Listados Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao listar clientes",ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Cliente/{id}")]
        [ProducesResponseType(typeof(ConsultarClienteViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
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
                _logger.LogInformation("Falha ao consultar cliente", ex);
                return BadRequest(ex);
            }
        }

        [HttpPost("/api/Cliente")]
        [ProducesResponseType(typeof(CadastrarClienteViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
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
                _logger.LogInformation("Falha ao cadastrar cliente", ex);
                return BadRequest(ex);
            }
        }

        [HttpPut("/api/Clientes/{id}")]
        [ProducesResponseType(typeof(AtualizarClienteViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AtualizarCliente(Guid id,[FromBody] AtualizarClienteViewModel.Request request)
        {
            try
            {
                var response = await _clienteService.AtualizarCliente(id, request);
                _logger.LogInformation("Cliente Atualizado Com Sucesso!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao atualizar cliente", ex);
                return BadRequest(ex);
            }
        }
    } 
    
}
