using System;

namespace SistemaSeguro.Api.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Profissao { get; set; }
    }

    public class Automovel
    {
        public int IdAutomovel { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int Ano { get; set; }
        public int IdCliente { get; set; }
        public string? NomeCliente { get; set; }
    }

    public class Seguro
    {
        public int IdSeguro { get; set; }
        public string NumeroApolice { get; set; } = string.Empty;
        public decimal ValorCobertura { get; set; }
        public int IdCliente { get; set; }
        public string? NomeCliente { get; set; }
        public int IdAutomovel { get; set; }
        public string? PlacaAutomovel { get; set; }
    }

    public class Acidente
    {
        public int IdAcidente { get; set; }
        public DateTime DataAcidente { get; set; }
        public TimeSpan HoraAcidente { get; set; }
        public string EnderecoAcidente { get; set; } = string.Empty;
        public string? MotivoAcidente { get; set; }
        public string? AutomoveisEnvolvidos { get; set; }
        public string? DanosTerceiros { get; set; }
        public int IdAutomovel { get; set; }
        public string? PlacaAutomovel { get; set; }
    }

    public class PagamentoSeguro
    {
        public int IdPagamento { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public int IdCliente { get; set; }
        public string? NomeCliente { get; set; }
        public int IdAcidente { get; set; }
        public string? EnderecoAcidente { get; set; }
    }
}
