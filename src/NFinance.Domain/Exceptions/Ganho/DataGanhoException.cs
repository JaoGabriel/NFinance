using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Ganho
{
    [Serializable]
    public class DataGanhoException : DomainException
    {
        public override string ErrorCode => "ERRO_DATA_GANHO";
        private const string DefaultMessage = "A data inserida e invalida";

        public DataGanhoException() : this(DefaultMessage)
        {
        }
        public DataGanhoException(string message) : base(message)
        {
        }
        protected DataGanhoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
