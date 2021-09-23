﻿using NFinance.Domain;

namespace NFinance.Application.ViewModel.ClientesViewModel
{
    public class AtualizarClienteViewModel
    {
        public class Request : ClienteViewModel
        {
            public Request() { }
            
            public Request(Cliente request)
            {
                Nome = request.Nome;
                Cpf = request.Cpf;
                Email = request.Email;
            }
        }

        public class Response : ClienteViewModel
        {
            public Response(Cliente cliente) : base(cliente) { }
        }
    }
}