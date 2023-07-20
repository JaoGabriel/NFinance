using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Application.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ResgateException : Exception
    {
        public ResgateException()
        {
        }
        public ResgateException(string message) : base(message)
        {
        }
        public ResgateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}