using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Cliente
{
    [Serializable]
    public class CpfClienteException : DomainException
    {
        public override string ErrorCode => "ERRO_CPF_CLIENTE";
        private const string DefaultMessage = "O CPF e obrigatorio";

        public CpfClienteException() : this(DefaultMessage)
        {
        }
        public CpfClienteException(string message) : base(message)
        {
        }
        protected CpfClienteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
