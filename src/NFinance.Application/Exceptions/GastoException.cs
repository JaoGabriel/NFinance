using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Application.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class GastoException : Exception
    {
        public GastoException()
        {
        }
        public GastoException(string message) : base(message)
        {
        }
        public GastoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}