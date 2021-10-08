using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Infra.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class InfraException : Exception
    {
        public InfraException()
        {
        }
        public InfraException(string message) : base(message)
        {
        }
        public InfraException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}