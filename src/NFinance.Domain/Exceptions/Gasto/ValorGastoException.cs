using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Gasto
{
    [Serializable]
    public class ValorGastoException : DomainException
    {
        public override string ErrorCode => "ERRO_VALOR_GASTO";
        private const string DefaultMessage = "O valor inserido e invalido";

        public ValorGastoException() : this(DefaultMessage)
        {
        }
        public ValorGastoException(string message) : base(message)
        {
        }
        protected ValorGastoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
