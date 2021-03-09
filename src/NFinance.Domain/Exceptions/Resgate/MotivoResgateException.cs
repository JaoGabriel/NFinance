using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.Resgate
{
    [Serializable]
    public class MotivoResgateException : DomainException
    {
        public override string ErrorCode => "ERRO_MOTIVO_RESGATE";
        private const string DefaultMessage = "O motivo nao deve ser vazio,branco ou nulo";

        public MotivoResgateException() : this(DefaultMessage)
        {
        }
        public MotivoResgateException(string message) : base(message)
        {
        }
        protected MotivoResgateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
