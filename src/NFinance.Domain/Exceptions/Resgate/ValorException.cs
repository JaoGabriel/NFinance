using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Resgate
{
    [Serializable]
    public class ValorException : DomainException
    {
        public override string ErrorCode => "ERRO_VALOR_RESGATE";
        private const string DefaultMessage = "O valor nao pode ser menor que 0";

        public ValorException() : this(DefaultMessage)
        {
        }
        public ValorException(string message) : base(message)
        {
        }
        protected ValorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
