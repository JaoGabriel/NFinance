using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Ganho
{
    [Serializable]
    public class ValorGanhoException : DomainException
    {
        public override string ErrorCode => "ERRO_VALOR_GANHO";
        private const string DefaultMessage = "O valor inserido e invalido";

        public ValorGanhoException() : this(DefaultMessage)
        {
        }
        public ValorGanhoException(string message) : base(message)
        {
        }
        protected ValorGanhoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}