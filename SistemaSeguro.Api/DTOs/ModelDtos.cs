using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaSeguro.Api.DTOs
{
    public class ClienteCreateUpdateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(150, ErrorMessage = "O endereço deve ter no máximo 150 caracteres.")]
        public string Endereco { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
        [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
        public string? Email { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(50, ErrorMessage = "A profissão deve ter no máximo 50 caracteres.")]
        public string? Profissao { get; set; }
    }

    public class AutomovelCreateUpdateDto
    {
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(10, ErrorMessage = "A placa deve ter no máximo 10 caracteres.")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(50, ErrorMessage = "O modelo deve ter no máximo 50 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A marca é obrigatória.")]
        [StringLength(50, ErrorMessage = "A marca deve ter no máximo 50 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "O ano deve estar entre 1900 e 2100.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "O ID do cliente proprietário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do cliente inválido.")]
        public int IdCliente { get; set; }
    }

    public class SeguroCreateUpdateDto
    {
        [Required(ErrorMessage = "O número da apólice é obrigatório.")]
        [StringLength(50, ErrorMessage = "O número da apólice deve ter no máximo 50 caracteres.")]
        public string NumeroApolice { get; set; } = string.Empty;

        [Range(0.01, 99999999.99, ErrorMessage = "O valor de cobertura deve ser maior que zero.")]
        public decimal ValorCobertura { get; set; }

        [Required(ErrorMessage = "O ID do cliente titular é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do cliente inválido.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "O ID do automóvel segurado é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do automóvel inválido.")]
        public int IdAutomovel { get; set; }
    }

    public class AcidenteCreateUpdateDto
    {
        [Required(ErrorMessage = "A data do acidente é obrigatória.")]
        public DateTime DataAcidente { get; set; }

        [Required(ErrorMessage = "A hora do acidente é obrigatória (formato HH:mm:ss).")]
        public string HoraAcidente { get; set; } = "00:00:00";

        [Required(ErrorMessage = "O endereço/local do sinistro é obrigatório.")]
        [StringLength(150, ErrorMessage = "O endereço deve ter no máximo 150 caracteres.")]
        public string EnderecoAcidente { get; set; } = string.Empty;

        public string? MotivoAcidente { get; set; }
        public string? AutomoveisEnvolvidos { get; set; }
        public string? DanosTerceiros { get; set; }

        [Required(ErrorMessage = "O ID do automóvel é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do automóvel inválido.")]
        public int IdAutomovel { get; set; }
    }

    public class PagamentoCreateUpdateDto
    {
        [Required(ErrorMessage = "O valor da indenização é obrigatório.")]
        [Range(0.01, 99999999.99, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A data de pagamento é obrigatória.")]
        public DateTime DataPagamento { get; set; }

        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do cliente inválido.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "O ID do acidente/sinistro é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do acidente inválido.")]
        public int IdAcidente { get; set; }
    }
}
