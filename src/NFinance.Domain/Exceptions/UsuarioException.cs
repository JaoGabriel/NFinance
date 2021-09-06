using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions
{
    [Serializable]
    public class UsuarioException : DomainException
    {
        public override string ErrorCode => "ERRO_USUARIO_INEXISTENTE";
        private const string DefaultMessage = "O Usuario deve existir";

        public UsuarioException() : this(DefaultMessage)
        {
        }

        public UsuarioException(string message) : base(message)
        {
        }

        protected UsuarioException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}