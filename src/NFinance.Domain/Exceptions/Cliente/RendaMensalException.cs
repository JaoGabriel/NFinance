using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Cliente
{
    [Serializable]
    public class RendaMensalException : DomainException
    {
        public override string ErrorCode => "ERRO_RENDA_MENSAL_CLIENTE";
        private const string DefaultMessage = "A renda mensal deve ser maior que 0";

        public RendaMensalException() : this(DefaultMessage)
        {
        }

        public RendaMensalException(string message) : base(message)
        {
        }

        protected RendaMensalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}