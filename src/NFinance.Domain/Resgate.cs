using System;

namespace NFinance.Domain
{
    public class Resgate
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string MotivoResgate { get; set; }
        public DateTime DataResgate { get; set; }

    }
}
