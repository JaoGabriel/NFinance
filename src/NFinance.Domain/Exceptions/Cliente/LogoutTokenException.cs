using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Cliente
{
    [Serializable]
    public class LogoutTokenException : DomainException
    {
        public override string ErrorCode => "ERRO_LOGOUT_TOKEN";
        private const string DefaultMessage = "Erro ao cadastrar logout token";

        public LogoutTokenException() : this(DefaultMessage)
        {
        }
        public LogoutTokenException(string message) : base(message)
        {
        }
        protected LogoutTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
