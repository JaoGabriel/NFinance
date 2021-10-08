using System;
using NFinance.Domain.Identidade;
using System.ComponentModel.DataAnnotations;
using NFinance.Domain.Exceptions;
using NFinance.Domain.ObjetosDeValor;

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
        public Cpf Cpf { get; private set; }
        
        [Required]
        public Email Email { get; private set; }

        [Required]
        public Celular Celular { get; set; }

        public Cliente(string nome, string cpf, string email,string celular)
        {
            ValidaDadosCliente(nome,cpf,email,celular);
            
            Id = Guid.NewGuid();
            Nome = nome;
            Cpf = new Cpf(cpf);
            Email = new Email(email);
            Celular = new Celular(celular);
        }

        public void AtualizarDadosCliente(string nome, string cpf, string email,string celular)
        {
            ValidaDadosCliente(nome,cpf,email,celular);
            
            Nome = nome;
            Cpf = new Cpf(cpf);
            Email = new Email(email);
            Celular = new Celular(celular);
        }

        public void AtruibiUsuarioCliente(Usuario usuario)
        {
            ValidaUsuarioCliente(usuario);

            Usuario = usuario;
        }

        private static void ValidaDadosCliente(string nome, string cpf, string email,string celular)
        {
            if (string.IsNullOrWhiteSpace(nome)) 
                throw new DomainException("Nome inválido.");
            
            if (string.IsNullOrWhiteSpace(cpf)) 
                throw new DomainException("Cpf inválido.");
           
            if (string.IsNullOrWhiteSpace(email)) 
                throw new DomainException("Email inválido.");
            
            if (string.IsNullOrWhiteSpace(celular)) 
                throw new DomainException("Telefone Celular inválido.");
        }
        
        private static void ValidaUsuarioCliente(Usuario usuario)
        {
            if (usuario is null) 
                throw new DomainException("Usuário inválido.");
        }
    }
}
