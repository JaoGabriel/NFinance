using System;
using Microsoft.AspNetCore.Identity;

namespace NFinance.Infra.Identidade
{
    public class Usuario : IdentityUser<Guid>
    {
        public Guid IdUsuario { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }
    }
}