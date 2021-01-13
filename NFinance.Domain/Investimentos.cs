using System;

namespace NFinance.Domain
{
    public class Investimentos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataAplicacao { get; set; }
    }
}
