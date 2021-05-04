using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Autenticacao
{
    [Serializable]
    public class LoginException : DomainException
    {
        public override string ErrorCode => "ERRO_LOGIN_CLIENTE";
        private const string DefaultMessage = "O Login ou senha sao obrigatorios";

        public LoginException() : this(DefaultMessage)
        {
        }
        public LoginException(string message) : base(message)
        {
        }
        protected LoginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}