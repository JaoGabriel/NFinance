using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Gasto
{
    [Serializable]
    public class QuantidadeParcelaException : DomainException
    {
        public override string ErrorCode => "ERRO_QUANTIDADE_PARCELA_GASTO";
        private const string DefaultMessage = "A quantidade inserida e invalida";

        public QuantidadeParcelaException() : this(DefaultMessage)
        {
        }
        public QuantidadeParcelaException(string message) : base(message)
        {
        }
        protected QuantidadeParcelaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
