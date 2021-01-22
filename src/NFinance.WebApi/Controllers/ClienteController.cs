using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;

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
        [ProducesResponseType(typeof(Cliente.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationFailure), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarClientes()
        {
            List<Cliente> listaClientes = new List<Cliente>();
            try
            {
                var client = await _clienteService.ListarClientes();
                foreach (var cliente in client)
                    listaClientes.Add(cliente);
               
                _logger.LogInformation("Clientes Listados Com Sucesso!");
                return Ok(listaClientes);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao listar clientes",ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("/api/Cliente/{id}")]
        [ProducesResponseType(typeof(Cliente.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationFailure), (int)HttpStatusCode.BadRequest)]
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
        [ProducesResponseType(typeof(Cliente.ViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationFailure), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CadastrarCliente([FromBody] Cliente request)
        {
            try
            {
                Cliente.Response clienteResponse = await _clienteService.CadastrarCliente(request);
                _logger.LogInformation("Clientes Cadastrado Com Sucesso!");
                return Ok(clienteResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao cadastrar cliente", ex);
                return BadRequest(ex);
            }
        }

        [HttpPut("/api/Clientes/{id}")]
        [ProducesResponseType(typeof(Cliente.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationFailure), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AtualizarCliente(Guid id,[FromBody] Cliente cliente)
        {
            try
            {
                var clienteResponse = await _clienteService.AtualizarCliente(id,cliente);
                _logger.LogInformation("Cliente Atualizado Com Sucesso!");
                return Ok(clienteResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao atualizar cliente", ex);
                return BadRequest(ex);
            }
        }
    } 
    
}
