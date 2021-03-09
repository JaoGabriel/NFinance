using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Resgate
{
    [Serializable]
    public class DataResgateException : DomainException
    {
        public override string ErrorCode => "ERRO_DATA_RESGATE";
        private const string DefaultMessage = "A data inserida e invalida";

        public DataResgateException() : this(DefaultMessage)
        {
        }
        public DataResgateException(string message) : base(message)
        {
        }
        protected DataResgateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
