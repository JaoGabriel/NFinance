using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Gasto
{
    [Serializable]
    public class NomeGastoException : DomainException
    {
        public override string ErrorCode => "ERRO_NOME_GASTO";
        private const string DefaultMessage = "O nome inserido e invalido";

        public NomeGastoException() : this(DefaultMessage)
        {
        }
        public NomeGastoException(string message) : base(message)
        {
        }
        protected NomeGastoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
