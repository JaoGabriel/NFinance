using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException()
        {
        }
        public DomainException(string message) : base(message)
        {
        }
        public DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
