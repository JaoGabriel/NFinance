using System;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Identidade;
using NFinance.Domain.Exceptions.Cliente;
using System.ComponentModel.DataAnnotations;

namespace NFinance.Domain
{
    public class Cliente
    {
        [Key]
        [Required]
        public Guid Id { get; private set; }

        [Required]
        public Usuario Usuario { get; private set; }
        
        [Required]
        public string Nome { get; private set; }

        [Required]
        public string CPF { get; private set; }
        
        [Required]
        public string Email { get; private set; }

        public Cliente(string nome, string cpf, string email, Usuario usuario)
        {
            ValidaCadastroCliente(nome,cpf,email,usuario);
                
            Id = Guid.NewGuid();
            Nome = nome;
            CPF = cpf;
            Email = email;
            Usuario = usuario;
        }
        
        public Cliente(Guid id,string nome, string cpf, string email)
        {
            ValidaCliente(id,nome,cpf,email);
                
            Id = id;
            Nome = nome;
            CPF = cpf;
            Email = email;
        }

        private static void ValidaCadastroCliente(string nome, string cpf, string email,Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(nome)) 
                throw new NomeClienteException();
            
            if (string.IsNullOrWhiteSpace(cpf)) 
                throw new CpfClienteException();
           
            if (string.IsNullOrWhiteSpace(email)) 
                throw new EmailClienteException();

            if (usuario is null)
                throw new UsuarioException();
        }
        
        private static void ValidaCliente(Guid id, string nome, string cpf, string email)
        {
            if (Guid.Empty.Equals(id)) 
                throw new IdException();
            
            if (string.IsNullOrWhiteSpace(nome))
                throw new NomeClienteException();
            
            if (string.IsNullOrWhiteSpace(cpf)) 
                throw new CpfClienteException();
            
            if (string.IsNullOrWhiteSpace(email)) 
                throw new EmailClienteException();
        }
    }
}
