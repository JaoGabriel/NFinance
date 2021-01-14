using System;

namespace NFinance.Domain
{
    public class Gastos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal ValorTotal { get; set; }
        public int QuantidadeParcelas { get; set; }
        public DateTime DataDoGasto { get; set; }
    }
}
