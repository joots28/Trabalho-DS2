using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using SistemaSeguro.Api.Data;
using SistemaSeguro.Api.Models;

namespace SistemaSeguro.Api.Repositories
{
    public interface IPagamentoRepository
    {
        Task<int> InserirAsync(PagamentoSeguro pgto);
        Task<List<PagamentoSeguro>> ListarTodosAsync(int? idCliente = null);
        Task<PagamentoSeguro?> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(PagamentoSeguro pgto);
        Task<bool> ExcluirAsync(int id);
        Task<bool> ExisteAsync(int id);
    }

    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public PagamentoRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> InserirAsync(PagamentoSeguro pgto)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"INSERT INTO pagamento (valor, data_pagamento, id_cliente, id_acidente) 
                                 VALUES (@valor, @dataPgto, @idCliente, @idAcidente);
                                 SELECT LAST_INSERT_ID();";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@valor", pgto.Valor);
            cmd.Parameters.AddWithValue("@dataPgto", pgto.DataPagamento);
            cmd.Parameters.AddWithValue("@idCliente", pgto.IdCliente);
            cmd.Parameters.AddWithValue("@idAcidente", pgto.IdAcidente);

            var idResult = await cmd.ExecuteScalarAsync();
            pgto.IdPagamento = Convert.ToInt32(idResult);
            return pgto.IdPagamento;
        }

        public async Task<List<PagamentoSeguro>> ListarTodosAsync(int? idCliente = null)
        {
            var lista = new List<PagamentoSeguro>();
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            string sql = @"SELECT p.id_pagamento, p.valor, p.data_pagamento, p.id_cliente, p.id_acidente, 
                                  c.nome AS nome_cliente, ac.endereco_acidente
                           FROM pagamento p
                           INNER JOIN cliente c ON p.id_cliente = c.id_cliente
                           LEFT JOIN acidente ac ON p.id_acidente = ac.id_acidente";

            if (idCliente.HasValue)
            {
                sql += " WHERE p.id_cliente = @idCliente";
            }
            sql += " ORDER BY p.id_pagamento ASC";

            await using var cmd = new MySqlCommand(sql, conn);
            if (idCliente.HasValue)
            {
                cmd.Parameters.AddWithValue("@idCliente", idCliente.Value);
            }

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new PagamentoSeguro
                {
                    IdPagamento = reader.GetInt32("id_pagamento"),
                    Valor = reader.GetDecimal("valor"),
                    DataPagamento = reader.GetDateTime("data_pagamento"),
                    IdCliente = reader.GetInt32("id_cliente"),
                    NomeCliente = reader.GetString("nome_cliente"),
                    IdAcidente = reader.GetInt32("id_acidente"),
                    EnderecoAcidente = reader.IsDBNull(reader.GetOrdinal("endereco_acidente")) ? null : reader.GetString("endereco_acidente")
                });
            }
            return lista;
        }

        public async Task<PagamentoSeguro?> ObterPorIdAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"SELECT p.id_pagamento, p.valor, p.data_pagamento, p.id_cliente, p.id_acidente, 
                                        c.nome AS nome_cliente, ac.endereco_acidente
                                 FROM pagamento p
                                 INNER JOIN cliente c ON p.id_cliente = c.id_cliente
                                 LEFT JOIN acidente ac ON p.id_acidente = ac.id_acidente
                                 WHERE p.id_pagamento = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new PagamentoSeguro
                {
                    IdPagamento = reader.GetInt32("id_pagamento"),
                    Valor = reader.GetDecimal("valor"),
                    DataPagamento = reader.GetDateTime("data_pagamento"),
                    IdCliente = reader.GetInt32("id_cliente"),
                    NomeCliente = reader.GetString("nome_cliente"),
                    IdAcidente = reader.GetInt32("id_acidente"),
                    EnderecoAcidente = reader.IsDBNull(reader.GetOrdinal("endereco_acidente")) ? null : reader.GetString("endereco_acidente")
                };
            }
            return null;
        }

        public async Task<bool> AtualizarAsync(PagamentoSeguro pgto)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"UPDATE pagamento 
                                 SET valor = @valor, 
                                     data_pagamento = @dataPgto, 
                                     id_cliente = @idCliente, 
                                     id_acidente = @idAcidente 
                                 WHERE id_pagamento = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@valor", pgto.Valor);
            cmd.Parameters.AddWithValue("@dataPgto", pgto.DataPagamento);
            cmd.Parameters.AddWithValue("@idCliente", pgto.IdCliente);
            cmd.Parameters.AddWithValue("@idAcidente", pgto.IdAcidente);
            cmd.Parameters.AddWithValue("@id", pgto.IdPagamento);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "DELETE FROM pagamento WHERE id_pagamento = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "SELECT COUNT(1) FROM pagamento WHERE id_pagamento = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }
    }
}
