using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Investimento
{
    [Serializable]
    public class NomeInvestimentoException : DomainException
    {
        public override string ErrorCode => "ERRO_NOME_INVESTIMENTO";
        private const string DefaultMessage = "O nome inserido e invalido";

        public NomeInvestimentoException() : this(DefaultMessage)
        {
        }
        public NomeInvestimentoException(string message) : base(message)
        {
        }
        protected NomeInvestimentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
