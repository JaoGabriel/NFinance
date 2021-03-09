using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Investimento
{
    [Serializable]
    public class DataInvestimentoException : DomainException
    {
        public override string ErrorCode => "ERRO_DATA_INVESTIMENTO";
        private const string DefaultMessage = "A data inserida e invalida";

        public DataInvestimentoException() : this(DefaultMessage)
        {
        }
        public DataInvestimentoException(string message) : base(message)
        {
        }
        protected DataInvestimentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
