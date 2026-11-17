using System;
using SistemaSeguro.Model;
using SistemaSeguro.Services;
using Xunit;

namespace SistemaSeguro.Tests
{
    public class ServiceUnitTests
    {
        [Fact]
        public void ClienteService_Validar_DeveLancarExcecaoQuandoNomeForInvalido()
        {
            var service = new ClienteService();
            var clienteCurto = new Cliente { Nome = "Jo", Endereco = "Rua Teste" };

            var ex = Assert.Throws<ArgumentException>(() => service.Validar(clienteCurto));
            Assert.Contains("entre 3 e 100 caracteres", ex.Message);
        }

        [Fact]
        public void AutomovelService_Validar_DeveLancarExcecaoQuandoPlacaForInvalida()
        {
            var service = new AutomovelService();
            var autoInvalido = new Automovel
            {
                Placa = "12345",
                Modelo = "Civic",
                Marca = "Honda",
                Ano = 2020,
                IdCliente = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => service.Validar(autoInvalido));
            Assert.Contains("placa informada é inválida", ex.Message);
        }

        [Fact]
        public void SeguroService_Validar_DeveLancarExcecaoQuandoValorCoberturaForNegativoOuZero()
        {
            var service = new SeguroService();
            var seguroInvalido = new Seguro
            {
                NumeroApolice = "AP123",
                ValorCobertura = 0m,
                IdCliente = 1,
                IdAutomovel = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => service.Validar(seguroInvalido));
            Assert.Contains("maior que zero", ex.Message);
        }

        [Fact]
        public void PagamentoSeguroService_Validar_DeveLancarExcecaoQuandoValorForZero()
        {
            var service = new PagamentoSeguroService();
            var pgtoInvalido = new PagamentoSeguro
            {
                Valor = -50m,
                DataPagamento = DateTime.Today,
                IdCliente = 1,
                IdAcidente = 1
            };

            var ex = Assert.Throws<ArgumentException>(() => service.Validar(pgtoInvalido));
            Assert.Contains("maior que zero", ex.Message);
        }
    }
}
