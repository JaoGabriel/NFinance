using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Cliente
{
    public class NomeClienteException : DomainException
    {
        public override string ErrorCode => "ERRO_NOME_CLIENTE";
        private const string DefaultMessage = "O nome deve ser maior que 10 caracteres";

        public NomeClienteException() : this(DefaultMessage)
        {
        }
        public NomeClienteException(string message) : base(message)
        {
        }
        protected NomeClienteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
