using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Gasto
{
    [Serializable]
    public class DataGastoException : DomainException
    {
        public override string ErrorCode => "ERRO_DATA_GASTO";
        private const string DefaultMessage = "A data inserida e invalida";

        public DataGastoException() : this(DefaultMessage)
        {
        }
        public DataGastoException(string message) : base(message)
        {
        }
        protected DataGastoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
