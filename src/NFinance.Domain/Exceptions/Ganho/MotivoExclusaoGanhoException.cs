using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Ganho
{
    [Serializable]
    public class MotivoExclusaoGanhoException : DomainException
    {
        public override string ErrorCode => "ERRO_MOTIVO_EXCLUSAO_GANHO";
        private const string DefaultMessage = "O motivo inserido e invalido";

        public MotivoExclusaoGanhoException() : this(DefaultMessage)
        {
        }

        public MotivoExclusaoGanhoException(string message) : base(message)
        {
        }

        protected MotivoExclusaoGanhoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}