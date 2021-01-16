using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {

        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteService _clienteService;

        public ClienteController(ILogger<ClienteController> logger, IClienteService clienteService)
        {
            _logger = logger;
            _clienteService = clienteService;
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> ConsultarCliente(int id)
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

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente([FromBody] Cliente cliente)
        {
            try
            {
                var clienteResponse = await _clienteService.CadastrarCliente(cliente);
                _logger.LogInformation("Clientes Cadastrado Com Sucesso!");
                return Ok(clienteResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Falha ao cadastrar cliente", ex);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> CadastrarCliente(int id,[FromBody] Cliente cliente)
        {
            try
            {
                var clienteResponse = await _clienteService.AtualiazarCliente(id,cliente);
                _logger.LogInformation("Clientes Atualizado Com Sucesso!");
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
