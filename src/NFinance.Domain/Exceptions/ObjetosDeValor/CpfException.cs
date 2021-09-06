using System;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions.ObjetosDeValor
{
    [Serializable]
    public class CpfException : DomainException
    {
        public override string ErrorCode => "ERRO_CPF_INEXISTENTE";
        private const string DefaultMessage = "O CPF não pode ser nulo, vazio ou em branco";

        public CpfException() : this(DefaultMessage)
        {
        }

        public CpfException(string message) : base(message)
        {
        }

        protected CpfException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}