using System.ComponentModel.DataAnnotations;

namespace NFinance.Domain
{
    public class PainelDeControle
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal ValorNaCarteira { get; set; }

        public decimal SaldoMensal { get; set; }

        public decimal SaldoAnual { get; set; }

        public decimal GastosMensal { get; set; }

        public decimal GastosAnual { get; set; }

        public decimal ValorInvestidoMensal { get; set; }

        public decimal ValorInvestidoAnual { get; set; }

        public decimal ValorRecebidoAnual { get; set; }
    }
}
