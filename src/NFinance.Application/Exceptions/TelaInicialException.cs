using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Application.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class TelaInicialException : Exception
    {
        public TelaInicialException()
        {
        }
        public TelaInicialException(string message) : base(message)
        {
        }
        public TelaInicialException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}