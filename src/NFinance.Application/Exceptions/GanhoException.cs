using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Application.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class GanhoException : Exception
    {
        public GanhoException()
        {
        }
        public GanhoException(string message) : base(message)
        {
        }
        public GanhoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}