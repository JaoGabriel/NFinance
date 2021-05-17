using System;
using System.ComponentModel.DataAnnotations;

namespace NFinance.Domain
{
    public class Cliente
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100,MinimumLength = 10)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14,MinimumLength = 11)]
        public string CPF { get; set; }
        
        [Required]
        [StringLength(120,MinimumLength = 15)]
        public string Email { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 10)]
        public string Senha { get; set; }

        [StringLength(400)]
        public string LogoutToken { get; set; }

        public Cliente() { }

        public Cliente(string nome, string cpf, string email, string senha)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            CPF = cpf;
            Email = email;
            Senha = senha;
        }

        public Cliente(Guid id, string nome, string cpf, string email, string senha, string logoutToken)
        {
            Id = id;
            Nome = nome;
            CPF = cpf;
            Email = email;
            Senha = senha;
            LogoutToken = logoutToken;
        }
    }
}
