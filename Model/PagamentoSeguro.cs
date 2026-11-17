using System;

namespace SistemaSeguro.Model
{
    public class PagamentoSeguro
    {
        public int IdPagamento { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public int IdCliente { get; set; }   // Chave Estrangeira (FK) do Cliente
        public int IdAcidente { get; set; }  // Chave Estrangeira (FK) do Acidente
    }
}