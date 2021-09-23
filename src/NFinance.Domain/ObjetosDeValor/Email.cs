using System.Collections.Generic;
using System.Text.RegularExpressions;
using NFinance.Domain.Exceptions;

namespace NFinance.Domain.ObjetosDeValor
{
    public class Email : ValueObject
    {
        public string Endereco { get; set; }
        
        public Email(string endereco)
        {
            ValidaEmail(endereco);
            Endereco = endereco;
        }
        
        private static void ValidaEmail(string endereco)
        {
            const string patternStrict = @"^(|(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6})$";
            var regexStrict = new Regex(patternStrict);

            if(regexStrict.Match(endereco).Success is false)
                throw new DomainException("Endereco de email inválido");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Endereco;
        }

        public override string ToString()
        {
            return Endereco;
        }
    }
}