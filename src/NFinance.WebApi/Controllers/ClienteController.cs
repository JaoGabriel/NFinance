﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFinance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.ClientesViewModel;

namespace NFinance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteApp _clienteApp;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ILogger<ClienteController> logger, IClienteApp clienteApp, IAutenticacaoService AutenticacaoService)
        {
            _logger = logger;
            _clienteApp = clienteApp;
            _autenticacaoService = AutenticacaoService;
        }

        [HttpGet("Cliente/Consultar/{id}")]
        [ProducesResponseType(typeof(ConsultarClienteViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConsultarCliente([FromHeader] string authorization, Guid id)
        {

            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoService.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var cliente = await _clienteApp.ConsultaCliente(id);
            _logger.LogInformation("Clientes Encontrado Com Sucesso!");
            return Ok(cliente);
        }

        [HttpPost("Cliente/Cadastrar")]
        [ProducesResponseType(typeof(CadastrarClienteViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarCliente(CadastrarClienteViewModel.Request request)
        {
            var response = await _clienteApp.CadastrarCliente(request);
            _logger.LogInformation("Clientes Cadastrado Com Sucesso!");
            return Ok(response);
        }

        [HttpPut("Cliente/Atualizar/{id}")]
        [ProducesResponseType(typeof(AtualizarClienteViewModel.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<IActionResult> AtualizarCliente([FromHeader] string authorization, Guid id, AtualizarClienteViewModel.Request request)
        {
            _logger.LogInformation("Validando Bearer Token!");
            await _autenticacaoService.ValidaTokenRequest(authorization);
            _logger.LogInformation("Bearer Token Validado!");
            var response = await _clienteApp.AtualizarCliente(id, request);
            _logger.LogInformation("Cliente Atualizado Com Sucesso!");
            return Ok(response);
        }
    }
}