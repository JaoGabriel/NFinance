﻿using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class ResgateService : IResgateService
    {
        private readonly IResgateRepository _resgateRepository;

        public ResgateService(IResgateRepository resgateRepository)
        {
            _resgateRepository = resgateRepository;
        }

        public Task<Resgate> ConsultarResgate(Guid id)
        {
            return _resgateRepository.ConsultarResgate(id);
        }

        public Task<List<Resgate>> ListarResgates()
        {
            return _resgateRepository.ListarResgates();
        }

        public Task<Resgate> RealizarResgate(Guid id,Resgate resgate)
        {
            return _resgateRepository.RealizarResgate(id,resgate);
        }
    }
}
