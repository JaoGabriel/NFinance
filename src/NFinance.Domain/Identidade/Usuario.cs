using System;
using Microsoft.AspNetCore.Identity;
using NFinance.Domain.ObjetosDeValor;

namespace NFinance.Domain.Identidade
{
    public class Usuario : IdentityUser<Guid>
    {
        public Usuario(Cliente cliente)
        {
            Id = cliente.Id;
            Email = cliente.Email.ToString();
            EmailConfirmed = false;
            PhoneNumber = cliente.Celular.ToString();
            UserName = cliente.Nome;
        }
    }
}