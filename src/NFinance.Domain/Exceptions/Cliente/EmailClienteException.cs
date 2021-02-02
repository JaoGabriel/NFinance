using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Cliente
{
    [Serializable]
    public class EmailClienteException : DomainException
    {
        public override string ErrorCode => "ERRO_EMAIL_CLIENTE";
        private const string DefaultMessage = "O email nao deve ser branco, nulo ou vazio";

        public EmailClienteException() : this(DefaultMessage)
        {
        }
        public EmailClienteException(string message) : base(message)
        {
        }
        protected EmailClienteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
