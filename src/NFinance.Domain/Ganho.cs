using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NFinance.Domain.ViewModel.GanhoViewModel;

namespace NFinance.Domain
{
    public class Ganho
    {

        [Key] 
        [Required] 
        public Guid Id { get; set; }

        [ForeignKey("Id")] 
        [Required] 
        public Guid IdCliente { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string NomeGanho { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal Valor { get; set; }

        [Required] 
        public bool Recorrente { get; set; }

        public Ganho() {}

        public Ganho(CadastrarGanhoViewModel.Request request)
        {
            Id = Guid.NewGuid();
            IdCliente = request.IdCliente;
            NomeGanho = request.NomeGanho;
            Valor = request.Valor;
            Recorrente = request.Recorrente;
        }

        public Ganho(Guid id,AtualizarGanhoViewModel.Request request)
        {
            Id = id;
            IdCliente = request.IdCliente;
            NomeGanho = request.NomeGanho;
            Valor = request.Valor;
            Recorrente = request.Recorrente;
        }
    }
}