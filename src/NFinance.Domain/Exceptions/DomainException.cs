using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public abstract class DomainException : Exception
    {
        public abstract string ErrorCode { get; }

        protected DomainException()
        {
        }
        protected DomainException(string message) : base(message)
        {
        }
        protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
