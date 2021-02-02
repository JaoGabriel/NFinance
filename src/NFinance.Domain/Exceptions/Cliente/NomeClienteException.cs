using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Cliente
{
    [Serializable]
    public class NomeClienteException : DomainException
    {
        public override string ErrorCode => "ERRO_NOME_CLIENTE";
        private const string DefaultMessage = "O nome nao deve ser branco, nulo ou vazio";

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
