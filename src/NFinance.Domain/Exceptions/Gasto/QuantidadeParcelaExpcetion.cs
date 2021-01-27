using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Gasto
{
    [Serializable]
    public class QuantidadeParcelaExpcetion : DomainException
    {
        public override string ErrorCode => "ERRO_QUANTIDADE_PARCELA_GASTO";
        private const string DefaultMessage = "A quantidade inserida e invalida";

        public QuantidadeParcelaExpcetion() : this(DefaultMessage)
        {
        }
        public QuantidadeParcelaExpcetion(string message) : base(message)
        {
        }
        protected QuantidadeParcelaExpcetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
