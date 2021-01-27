using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions
{
    [Serializable]
    public class IdException : DomainException
    {
        public override string ErrorCode => "ERRO_ID_INEXISTENTE";
        private const string DefaultMessage = "O ID deve ser diferente de 0 ou nao deve ser nulo";

        public IdException() : this(DefaultMessage)
        {
        }

        public IdException(string message) : base(message)
        {
        }

        protected IdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
