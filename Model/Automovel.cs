namespace SistemaSeguro.Model
{
    public class Automovel
    {
        public int IdAutomovel { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public int Ano { get; set; }
        public int IdCliente { get; set; } // Chave Estrangeira (FK) do Cliente
    }
}