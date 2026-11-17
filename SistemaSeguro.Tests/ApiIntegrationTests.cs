using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using SistemaSeguro.Api.DTOs;
using SistemaSeguro.Api.Models;
using Xunit;

namespace SistemaSeguro.Tests
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task HealthCheck_DeveRetornar200EConexaoComBancoValida()
        {
            var response = await _client.GetAsync("/api/health");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<JsonElement>>(_jsonOptions);
            Assert.NotNull(content);
            Assert.True(content.Success);
            Assert.Equal("Healthy", content.Data.GetProperty("status").GetString());
            Assert.Equal("Connected", content.Data.GetProperty("database").GetString());
        }

        [Fact]
        public async Task Cliente_DeveRealizarCrudCompletoEValidacoes()
        {
            // 1. Tentar cadastrar com dados inválidos (nome vazio) -> Deve retornar 400
            var dtoInvalido = new ClienteCreateUpdateDto
            {
                Nome = "",
                Endereco = "Rua Teste",
                Email = "email-invalido"
            };
            var resInvalida = await _client.PostAsJsonAsync("/api/clientes", dtoInvalido);
            Assert.Equal(HttpStatusCode.BadRequest, resInvalida.StatusCode);

            // 2. Cadastrar cliente válido -> 201 Created
            string nomeUnico = "Engenheiro Teste " + Guid.NewGuid().ToString("N")[..6];
            var dtoValido = new ClienteCreateUpdateDto
            {
                Nome = nomeUnico,
                Endereco = "Avenida Central 1000",
                Telefone = "11988887777",
                Email = "eng.teste@seguradora.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Profissao = "Engenheiro de Software"
            };
            var resCriacao = await _client.PostAsJsonAsync("/api/clientes", dtoValido);
            Assert.Equal(HttpStatusCode.Created, resCriacao.StatusCode);

            var clienteCriado = await resCriacao.Content.ReadFromJsonAsync<ApiResponse<Cliente>>(_jsonOptions);
            Assert.NotNull(clienteCriado);
            Assert.True(clienteCriado.Success);
            Assert.True(clienteCriado.Data!.IdCliente > 0);
            int idCliente = clienteCriado.Data.IdCliente;

            // 3. Consultar por ID -> 200 OK
            var resGet = await _client.GetAsync($"/api/clientes/{idCliente}");
            Assert.Equal(HttpStatusCode.OK, resGet.StatusCode);
            var clienteConsultado = await resGet.Content.ReadFromJsonAsync<ApiResponse<Cliente>>(_jsonOptions);
            Assert.Equal(nomeUnico, clienteConsultado!.Data!.Nome);

            // 4. Atualizar -> 200 OK
            dtoValido.Nome = nomeUnico + " Atualizado";
            var resPut = await _client.PutAsJsonAsync($"/api/clientes/{idCliente}", dtoValido);
            Assert.Equal(HttpStatusCode.OK, resPut.StatusCode);

            // 5. Excluir -> 200 OK
            var resDelete = await _client.DeleteAsync($"/api/clientes/{idCliente}");
            Assert.Equal(HttpStatusCode.OK, resDelete.StatusCode);

            // 6. Consultar após exclusão -> 404 Not Found
            var resGetPosDelete = await _client.GetAsync($"/api/clientes/{idCliente}");
            Assert.Equal(HttpStatusCode.NotFound, resGetPosDelete.StatusCode);
        }

        [Fact]
        public async Task FluxoCompleto_Cliente_Automovel_Seguro_Acidente_Pagamento()
        {
            // 1. Criar Cliente
            var dtoCliente = new ClienteCreateUpdateDto
            {
                Nome = "Cliente Integrado " + Guid.NewGuid().ToString("N")[..6],
                Endereco = "Rua das Flores 123",
                Telefone = "11999990000",
                Email = "cliente.fluxo@teste.com",
                DataNascimento = new DateTime(1985, 10, 20),
                Profissao = "Arquiteto"
            };
            var resCliente = await _client.PostAsJsonAsync("/api/clientes", dtoCliente);
            Assert.Equal(HttpStatusCode.Created, resCliente.StatusCode);
            var cliente = (await resCliente.Content.ReadFromJsonAsync<ApiResponse<Cliente>>(_jsonOptions))!.Data!;

            try
            {
                // 2. Criar Automovel vinculado ao Cliente
                var dtoAuto = new AutomovelCreateUpdateDto
                {
                    Placa = "BRA2E19",
                    Modelo = "Corolla Hybrid",
                    Marca = "Toyota",
                    Ano = 2024,
                    IdCliente = cliente.IdCliente
                };
                var resAuto = await _client.PostAsJsonAsync("/api/automoveis", dtoAuto);
                Assert.Equal(HttpStatusCode.Created, resAuto.StatusCode);
                var auto = (await resAuto.Content.ReadFromJsonAsync<ApiResponse<Automovel>>(_jsonOptions))!.Data!;

                // 3. Criar Seguro vinculado ao Cliente e Automóvel
                var dtoSeguro = new SeguroCreateUpdateDto
                {
                    NumeroApolice = "APOL-2026-" + Guid.NewGuid().ToString("N")[..4],
                    ValorCobertura = 150000.00m,
                    IdCliente = cliente.IdCliente,
                    IdAutomovel = auto.IdAutomovel
                };
                var resSeguro = await _client.PostAsJsonAsync("/api/seguros", dtoSeguro);
                Assert.Equal(HttpStatusCode.Created, resSeguro.StatusCode);
                var seguro = (await resSeguro.Content.ReadFromJsonAsync<ApiResponse<Seguro>>(_jsonOptions))!.Data!;
                Assert.True(seguro.IdSeguro > 0);

                // 4. Criar Acidente vinculado ao Automóvel
                var dtoAcidente = new AcidenteCreateUpdateDto
                {
                    DataAcidente = DateTime.Today,
                    HoraAcidente = "14:30:00",
                    EnderecoAcidente = "Av. Paulista, 1000 - Bela Vista",
                    MotivoAcidente = "Colisão traseira leve",
                    AutomoveisEnvolvidos = "Toyota Corolla e Honda Civic",
                    DanosTerceiros = "Para-choque traseiro avariado",
                    IdAutomovel = auto.IdAutomovel
                };
                var resAcidente = await _client.PostAsJsonAsync("/api/acidentes", dtoAcidente);
                Assert.Equal(HttpStatusCode.Created, resAcidente.StatusCode);
                var acidente = (await resAcidente.Content.ReadFromJsonAsync<ApiResponse<Acidente>>(_jsonOptions))!.Data!;
                Assert.True(acidente.IdAcidente > 0);

                // 5. Criar Pagamento vinculado ao Cliente e Acidente
                var dtoPgto = new PagamentoCreateUpdateDto
                {
                    Valor = 3500.50m,
                    DataPagamento = DateTime.Today,
                    IdCliente = cliente.IdCliente,
                    IdAcidente = acidente.IdAcidente
                };
                var resPgto = await _client.PostAsJsonAsync("/api/pagamentos", dtoPgto);
                Assert.Equal(HttpStatusCode.Created, resPgto.StatusCode);
                var pgto = (await resPgto.Content.ReadFromJsonAsync<ApiResponse<PagamentoSeguro>>(_jsonOptions))!.Data!;
                Assert.True(pgto.IdPagamento > 0);

                // 6. Consultar listas
                var resListAuto = await _client.GetAsync($"/api/automoveis?idCliente={cliente.IdCliente}");
                Assert.Equal(HttpStatusCode.OK, resListAuto.StatusCode);

                var resListSeguro = await _client.GetAsync($"/api/seguros?idCliente={cliente.IdCliente}");
                Assert.Equal(HttpStatusCode.OK, resListSeguro.StatusCode);

                var resListPgto = await _client.GetAsync($"/api/pagamentos?idCliente={cliente.IdCliente}");
                Assert.Equal(HttpStatusCode.OK, resListPgto.StatusCode);

                // Limpeza ordenada respeitando chaves estrangeiras
                await _client.DeleteAsync($"/api/pagamentos/{pgto.IdPagamento}");
                await _client.DeleteAsync($"/api/acidentes/{acidente.IdAcidente}");
                await _client.DeleteAsync($"/api/seguros/{seguro.IdSeguro}");
                await _client.DeleteAsync($"/api/automoveis/{auto.IdAutomovel}");
            }
            finally
            {
                // Limpar cliente
                await _client.DeleteAsync($"/api/clientes/{cliente.IdCliente}");
            }
        }
    }
}
