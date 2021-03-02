using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Cliente
{
    [Serializable]
    public class SenhaClienteException : DomainException
    {
        public override string ErrorCode => "ERRO_SENHA_CLIENTE";
        private const string DefaultMessage = "A senha nao deve ser branco, nulo ou vazio";

        public SenhaClienteException() : this(DefaultMessage)
        {
        }
        public SenhaClienteException(string message) : base(message)
        {
        }
        protected SenhaClienteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
