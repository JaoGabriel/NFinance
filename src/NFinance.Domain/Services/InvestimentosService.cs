﻿using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Investimento;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.ClientesViewModel;
using NFinance.Model.InvestimentosViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class InvestimentosService : IInvestimentosService
    {
        private readonly IInvestimentosRepository _investimentosRepository;
        private readonly IClienteService _clienteService;

        public InvestimentosService(IInvestimentosRepository investimentosRepository, IClienteService clienteService)
        {
            _investimentosRepository = investimentosRepository;
            _clienteService = clienteService;
        }

        public async Task<AtualizarInvestimentoViewModel.Response> AtualizarInvestimento(Guid id, AtualizarInvestimentoViewModel.Request request)
        {
            if (Guid.Empty.Equals(id) == true) throw new IdException("Id investimento invalido");
            if (Guid.Empty.Equals(request.IdCliente) == true) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeInvestimento) == true) throw new NomeInvestimentoException("Nome nao pode ser nulo,vazio ou em branco");
            if (request.Valor <= 0) throw new ValorInvestimentoException("Valor nao pode ser menor que zero");
            if (request.DataAplicacao > DateTime.MaxValue.AddYears(-7899) || request.DataAplicacao < DateTime.MinValue.AddYears(1949)) throw new DataInvestimentoException();

            var investimento = new Investimentos(id,request);
            var cliente = await _clienteService.ConsultarCliente(request.IdCliente);
            var atualizado = await _investimentosRepository.AtualizarInvestimento(id, investimento);
            var response = new AtualizarInvestimentoViewModel.Response() {Id = atualizado.Id, NomeInvestimento = atualizado.NomeInvestimento, Valor = atualizado.Valor, DataAplicacao = atualizado.DataAplicacao, Cliente = new ClienteViewModel.Response() {Id = cliente.Id, Nome = cliente.Nome } };
            return response;
        }

        public async Task<ConsultarInvestimentoViewModel.Response> ConsultarInvestimento(Guid id)
        {
            if (Guid.Empty.Equals(id) == true) throw new IdException("Id investimento invalido");

            var consulta = await _investimentosRepository.ConsultarInvestimento(id);
            var cliente = await _clienteService.ConsultarCliente(consulta.IdCliente);
            var response = new ConsultarInvestimentoViewModel.Response() { Id = consulta.Id, NomeInvestimento = consulta.NomeInvestimento, Valor = consulta.Valor, DataAplicacao = consulta.DataAplicacao, Cliente = new ClienteViewModel.Response() { Id = cliente.Id, Nome = cliente.Nome } };
            return response;
        }

        public async Task<ListarInvestimentosViewModel.Response> ListarInvestimentos()
        {
            var listaInvestimentos = await _investimentosRepository.ListarInvestimentos();
            var response = new ListarInvestimentosViewModel.Response(listaInvestimentos);
            return response;
        }

        public async Task<RealizarInvestimentoViewModel.Response> RealizarInvestimento(RealizarInvestimentoViewModel.Request request)
        {
            if (Guid.Empty.Equals(request.IdCliente) == true) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeInvestimento) == true) throw new NomeInvestimentoException("Nome nao pode ser nulo,vazio ou em branco");
            if (request.Valor <= 0) throw new ValorInvestimentoException("Valor nao pode ser menor que zero");
            if (request.DataAplicacao > DateTime.MaxValue.AddYears(-7899) || request.DataAplicacao < DateTime.MinValue.AddYears(1949)) throw new DataInvestimentoException();

            var investimento = new Investimentos(request);
            var cliente = await _clienteService.ConsultarCliente(request.IdCliente);
            var investido = await _investimentosRepository.RealizarInvestimento(investimento);
            var response = new RealizarInvestimentoViewModel.Response() { Id = investido.Id, NomeInvestimento = investido.NomeInvestimento, Valor = investido.Valor, DataAplicacao = investido.DataAplicacao, Cliente = new ClienteViewModel.Response() { Id = cliente.Id, Nome = cliente.Nome } };
            return response;
        }
    }
}
