using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Gasto
{
    [Serializable]
    public class MotivoExclusaoException : DomainException
    {
        public override string ErrorCode => "ERRO_MOTIVO_EXCLUSAO_GASTO";
        private const string DefaultMessage = "O motivo inserido e invalida";

        public MotivoExclusaoException() : this(DefaultMessage)
        {
        }
        public MotivoExclusaoException(string message) : base(message)
        {
        }
        protected MotivoExclusaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
