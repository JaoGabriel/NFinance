using System;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Cliente;
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
            ValidaCadastroCliente(nome,cpf,email,senha);
                
            Id = Guid.NewGuid();
            Nome = nome;
            CPF = cpf;
            Email = email;
            Senha = senha;
        }

        public Cliente(Guid id, string nome, string cpf, string email, string senha, string logoutToken)
        {
            ValidaCliente(id,nome,cpf,email,senha,logoutToken);
            
            Id = id;
            Nome = nome;
            CPF = cpf;
            Email = email;
            Senha = senha;
            LogoutToken = logoutToken;
        }

        private static void ValidaCadastroCliente(string nome, string cpf, string email, string senha)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new NomeClienteException();
            if (string.IsNullOrWhiteSpace(cpf)) throw new CpfClienteException();
            if (string.IsNullOrWhiteSpace(email)) throw new EmailClienteException();
            if (string.IsNullOrWhiteSpace(senha)) throw new SenhaClienteException();
        }
        
        private static void ValidaCliente(Guid id, string nome, string cpf, string email, string senha, string logoutToken)
        {
            if (Guid.Empty.Equals(id)) throw new IdException();
            if (string.IsNullOrWhiteSpace(nome)) throw new NomeClienteException();
            if (string.IsNullOrWhiteSpace(cpf)) throw new CpfClienteException();
            if (string.IsNullOrWhiteSpace(email)) throw new EmailClienteException();
            if (string.IsNullOrWhiteSpace(senha)) throw new SenhaClienteException();
            if (string.IsNullOrWhiteSpace(logoutToken)) throw new LogoutTokenException();
        }
        
    }
}
