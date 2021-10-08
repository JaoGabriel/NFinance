using System;
using Microsoft.AspNetCore.Identity;

namespace NFinance.Domain.Identidade
{
    public class Usuario : IdentityUser<Guid>
    {
        public Usuario()
        {
            
        }
        
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