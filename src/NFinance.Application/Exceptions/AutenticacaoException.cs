using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NFinance.Application.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class AutenticacaoException : Exception
    {
        public AutenticacaoException()
        {
        }
        public AutenticacaoException(string message) : base(message)
        {
        }
        public AutenticacaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}