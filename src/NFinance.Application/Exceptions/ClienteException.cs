using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Application.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ClienteException : Exception
    {
        public ClienteException()
        {
        }
        public ClienteException(string message) : base(message)
        {
        }
        public ClienteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}