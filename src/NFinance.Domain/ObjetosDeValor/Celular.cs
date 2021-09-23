using System.Collections.Generic;
using NFinance.Domain.Exceptions;

namespace NFinance.Domain.ObjetosDeValor
{
    public class Celular : ValueObject
    {
        public string Ddd { get; private set; } 
        public string Numero { get; private set; }
        
        public Celular(string numero)
        {
            VerificaSeNumeroEstaVazioNuloOuEmpacoEmBranco(numero);
            numero = FormataTelefone(numero);
            AtribuiNumeroTelefone(numero);
        }

        public Celular(string ddd, string numero)
        {
            Ddd = ddd;
            Numero = numero;
        }

        private static void VerificaSeNumeroEstaVazioNuloOuEmpacoEmBranco(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new DomainException("Número de Telefone não pode ser nulo,vazio ou em branco");
        }
        
        private static string FormataTelefone(string numero)
        {
            return numero
                .Replace(" ", string.Empty)
                .Replace("(",string.Empty)
                .Replace(")",string.Empty)
                .Replace("-",string.Empty);
        }
        
        private void AtribuiNumeroTelefone(string numeroTelefone) 
        {
            if (numeroTelefone.Length > 1)
                Ddd = numeroTelefone.Substring(0, 2);

            numeroTelefone = numeroTelefone.Remove(0, 2);
            
            if (numeroTelefone.Length >= 9)
                Numero = numeroTelefone.Substring(0, 9);
            else if (numeroTelefone.Length >= 8)
                Numero = "9" + numeroTelefone.Substring(0, 8);
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Ddd;
            yield return Numero;
        }

        public override string ToString()
        {
            return Ddd + Numero;
        }
    }
}