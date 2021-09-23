using System;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;

namespace NFinance.Domain.ObjetosDeValor
{
    public class Cpf : ValueObject
    {
        public string Numero { get; private set; }

        public Cpf(string numero)
        {
            AtribuiCpf(numero);
        }

        private void AtribuiCpf(string numero)
        {
            VerificaSeNumeroEstaVazioNuloOuEmpacoEmBranco(numero);
            numero = RemoveFormatacao(numero);
            ValidaCpf(numero);
            Numero = numero;
        }

        private static void VerificaSeNumeroEstaVazioNuloOuEmpacoEmBranco(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new DomainException("Número do CPF não pode ser nulo,vazio ou em branco");
        }

        private static string RemoveFormatacao(string numero)
        {
            return numero
                .Replace(".", string.Empty)
                .Replace("-", string.Empty);
        }

        private static void ValidaCpf(string numero)
        {
            VerificaSeNumeroTemOnzeDigitos(numero);
            ValidaNumeroCpf(numero);
        }

        private static void VerificaSeNumeroTemOnzeDigitos(string numero)
        {
            if (numero.Length != 11)
                throw new DomainException("CPF deve conter 11 digitos");
        }

        private static void ValidaNumeroCpf(string cpf)
        {
            ValidaQueNaoDeveSerSequenciaNumerica(cpf);
            ValidaQueTodosNumerosNaoDevemSerIguais(cpf);

            int[] cpfSemDigitosVerificadores = ConvertCpfSemDigitosParaIntArray(cpf);
            int[] cpfComPrimeiroDigito = CalculaPrimeiroDigitoVerificador(cpfSemDigitosVerificadores);
            ValidaDigitosVerificadores(cpfComPrimeiroDigito, cpf);
        }

        private static void ValidaQueNaoDeveSerSequenciaNumerica(string cpf)
        {
            if (cpf == "12345678909")
                throw new DomainException("Número de CPF inválido.");
        }

        private static void ValidaQueTodosNumerosNaoDevemSerIguais(string cpf)
        {
            bool todosNumerosIguais = true;

            for (int i = 1; i < 11 && todosNumerosIguais; i++)
                if (cpf[i] != cpf[0])
                    todosNumerosIguais = false;

            if (todosNumerosIguais)
                throw new DomainException("Número de CPF inválido.");
        }
        private static int[] ConvertCpfSemDigitosParaIntArray(string cpf)
        {
            int[] numeros = new int[9];

            for (int i = 0; i < 9; i++)
                numeros[i] = int.Parse(
                  cpf[i].ToString());
            return numeros;
        }

        private static int[] CalculaPrimeiroDigitoVerificador(int[] numeros)
        {
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Array.Resize<int>(ref numeros, 10);
            numeros[9] = resto;

            return numeros;
        }

        private static void ValidaDigitosVerificadores(int[] numeros, string cpf)
        {
            int soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            int resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string primeiroDigito = numeros[9].ToString();
            string segundoDigito = resto.ToString();

            if (cpf.EndsWith(primeiroDigito + segundoDigito) is not true)
                throw new DomainException("Número de CPF inválido.");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Numero;
        }
    }
}
