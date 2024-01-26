﻿using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.GanhoViewModel
{
    public class GanhoViewModel
    {
        public Guid Id { get; set; }

        public Guid IdCliente { get; set; }

        public string NomeGanho { get; set; }

        public decimal Valor { get; set; }

        public bool Recorrente { get; set; }

        public DateTimeOffset DataDoGanho { get; set; }

        public GanhoViewModel() { }

        public GanhoViewModel(Ganho ganho)
        {
            Id = ganho.Id;
            IdCliente = ganho.IdCliente;
            NomeGanho = ganho.NomeGanho;
            Valor = ganho.Valor;
            Recorrente = ganho.Recorrente;
            DataDoGanho = ganho.DataDoGanho;
        }
    }
}