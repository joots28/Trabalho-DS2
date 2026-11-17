using System;

namespace SistemaSeguro.Model
{
    public class Acidente
    {
        public int IdAcidente { get; set; }
        public DateTime DataAcidente { get; set; }
        public TimeSpan HoraAcidente { get; set; }
        public string EnderecoAcidente { get; set; }
        public string MotivoAcidente { get; set; }
        public string AutomoveisEnvolvidos { get; set; }
        public string DanosTerceiros { get; set; }
        public int IdAutomovel { get; set; } // Chave Estrangeira (FK) do Automóvel
    }
}