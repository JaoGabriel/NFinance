using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Autenticacao
{
    [Serializable]
    public class LogoutException : DomainException
    {
        public override string ErrorCode => "ERRO_LOGOUT_CLIENTE";
        private const string DefaultMessage = "Aconteceu um erro, tente novamente em instantes!";

        public LogoutException() : this(DefaultMessage)
        {
        }
        public LogoutException(string message) : base(message)
        {
        }
        protected LogoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}