using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Ganho
{
    [Serializable]
    public class NomeGanhoException : DomainException
    {
        public override string ErrorCode => "ERRO_NOME_GANHO";
        private const string DefaultMessage = "O nome inserido e invalido";

        public NomeGanhoException() : this(DefaultMessage)
        {
        }
        public NomeGanhoException(string message) : base(message)
        {
        }
        protected NomeGanhoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}