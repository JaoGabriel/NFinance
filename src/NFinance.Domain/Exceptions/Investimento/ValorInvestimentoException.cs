using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Investimento
{
    [Serializable]
    public class ValorInvestimentoException : DomainException
    {
        public override string ErrorCode => "ERRO_VALOR_INVESTIMENTO";
        private const string DefaultMessage = "O valor inserido e invalido";

        public ValorInvestimentoException() : this(DefaultMessage)
        {
        }
        public ValorInvestimentoException(string message) : base(message)
        {
        }
        protected ValorInvestimentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
