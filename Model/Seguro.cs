using System;

namespace SistemaSeguro.Model
{
    public class Seguro
    {
        public int IdSeguro { get; set; }
        public string NumeroApolice { get; set; }
        public decimal ValorCobertura { get; set; }
        public int IdCliente { get; set; }     // Chave Estrangeira (FK) do Cliente
        public int IdAutomovel { get; set; }   // Chave Estrangeira (FK) do Automóvel
    }
}
