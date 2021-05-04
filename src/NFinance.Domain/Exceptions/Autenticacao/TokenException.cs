using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Autenticacao
{
    [Serializable]
    public class TokenException : DomainException
    {
        public override string ErrorCode => "ERRO_TOKEN_INVALIDO";
        private const string DefaultMessage = "Token Invalido";

        public TokenException() : this(DefaultMessage)
        {
        }
        public TokenException(string message) : base(message)
        {
        }
        protected TokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}