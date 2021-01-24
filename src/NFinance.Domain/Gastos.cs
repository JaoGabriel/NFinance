using NFinance.Model.GastosViewModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFinance.Domain
{
    public class Gastos
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey("Id")]
        [Required]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "Nome deve conter no minimo 10 letras e no maximo 100")]
        [StringLength(100, MinimumLength = 10)]
        public string Nome { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal ValorTotal { get; set; }
        
        [Required]
        [Range(0, 100, ErrorMessage = "Sua parcela {0} deve estar entre {1} e {2}")]
        public int QuantidadeParcelas { get; set; }
        
        [Required]
        [Range(typeof(DateTime), "01/01/1900", "12/31/2060", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTime DataDoGasto { get; set; }

        public Gastos() { }

        public Gastos(Gastos gastos)
        {
            Id = Guid.NewGuid();
            IdCliente = gastos.IdCliente;
            Nome = gastos.Nome;
            ValorTotal = gastos.ValorTotal;
            QuantidadeParcelas = gastos.QuantidadeParcelas;
            DataDoGasto = gastos.DataDoGasto;
        }

        public Gastos(CadastrarGastoViewModel.Request request)
        {
            Id = Guid.NewGuid();
            IdCliente = request.IdCliente;
            Nome = request.Nome;
            ValorTotal = request.ValorTotal;
            QuantidadeParcelas = request.QuantidadeParcelas;
            DataDoGasto = request.DataDoGasto;
        }

        public Gastos(Guid id,AtualizarGastoViewModel.Request request)
        {
            Id = id;
            IdCliente = request.IdCliente;
            Nome = request.Nome;
            ValorTotal = request.ValorTotal;
            QuantidadeParcelas = request.QuantidadeParcelas;
            DataDoGasto = request.DataDoGasto;
        }

        public Gastos(ExcluirGastoViewModel.Request request)
        {
            Id = request.IdGasto;
            IdCliente = request.IdCliente;
            Nome = request.MotivoExclusao;
            DataDoGasto = DateTime.UtcNow;
        }
    }
}
