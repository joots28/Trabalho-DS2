using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using SistemaSeguro.Api.Data;
using SistemaSeguro.Api.Models;

namespace SistemaSeguro.Api.Repositories
{
    public interface IAcidenteRepository
    {
        Task<int> InserirAsync(Acidente acidente);
        Task<List<Acidente>> ListarTodosAsync(int? idAutomovel = null);
        Task<Acidente?> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(Acidente acidente);
        Task<bool> ExcluirAsync(int id);
        Task<bool> ExisteAsync(int id);
    }

    public class AcidenteRepository : IAcidenteRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public AcidenteRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> InserirAsync(Acidente acidente)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"INSERT INTO acidente (data_acidente, hora_acidente, endereco_acidente, motivo_acidente, automoveis_envolvidos, danos_terceiros, id_automovel) 
                                 VALUES (@data, @hora, @endereco, @motivo, @envolvidos, @danos, @idAutomovel);
                                 SELECT LAST_INSERT_ID();";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@data", acidente.DataAcidente);
            cmd.Parameters.AddWithValue("@hora", acidente.HoraAcidente);
            cmd.Parameters.AddWithValue("@endereco", acidente.EnderecoAcidente.Trim());
            cmd.Parameters.AddWithValue("@motivo", (object?)acidente.MotivoAcidente ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@envolvidos", (object?)acidente.AutomoveisEnvolvidos ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@danos", (object?)acidente.DanosTerceiros ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@idAutomovel", acidente.IdAutomovel);

            var idResult = await cmd.ExecuteScalarAsync();
            acidente.IdAcidente = Convert.ToInt32(idResult);
            return acidente.IdAcidente;
        }

        public async Task<List<Acidente>> ListarTodosAsync(int? idAutomovel = null)
        {
            var lista = new List<Acidente>();
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            string sql = @"SELECT ac.id_acidente, ac.data_acidente, ac.hora_acidente, ac.endereco_acidente, 
                                  ac.motivo_acidente, ac.automoveis_envolvidos, ac.danos_terceiros, 
                                  ac.id_automovel, au.placa AS placa_automovel
                           FROM acidente ac
                           INNER JOIN automovel au ON ac.id_automovel = au.id_automovel";

            if (idAutomovel.HasValue)
            {
                sql += " WHERE ac.id_automovel = @idAutomovel";
            }
            sql += " ORDER BY ac.id_acidente ASC";

            await using var cmd = new MySqlCommand(sql, conn);
            if (idAutomovel.HasValue)
            {
                cmd.Parameters.AddWithValue("@idAutomovel", idAutomovel.Value);
            }

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                TimeSpan hora = TimeSpan.Zero;
                object horaObj = reader["hora_acidente"];
                if (horaObj is TimeSpan ts) hora = ts;
                else if (horaObj != null) TimeSpan.TryParse(horaObj.ToString(), out hora);

                lista.Add(new Acidente
                {
                    IdAcidente = reader.GetInt32("id_acidente"),
                    DataAcidente = reader.GetDateTime("data_acidente"),
                    HoraAcidente = hora,
                    EnderecoAcidente = reader.GetString("endereco_acidente"),
                    MotivoAcidente = reader.IsDBNull(reader.GetOrdinal("motivo_acidente")) ? null : reader.GetString("motivo_acidente"),
                    AutomoveisEnvolvidos = reader.IsDBNull(reader.GetOrdinal("automoveis_envolvidos")) ? null : reader.GetString("automoveis_envolvidos"),
                    DanosTerceiros = reader.IsDBNull(reader.GetOrdinal("danos_terceiros")) ? null : reader.GetString("danos_terceiros"),
                    IdAutomovel = reader.GetInt32("id_automovel"),
                    PlacaAutomovel = reader.GetString("placa_automovel")
                });
            }
            return lista;
        }

        public async Task<Acidente?> ObterPorIdAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"SELECT ac.id_acidente, ac.data_acidente, ac.hora_acidente, ac.endereco_acidente, 
                                        ac.motivo_acidente, ac.automoveis_envolvidos, ac.danos_terceiros, 
                                        ac.id_automovel, au.placa AS placa_automovel
                                 FROM acidente ac
                                 INNER JOIN automovel au ON ac.id_automovel = au.id_automovel
                                 WHERE ac.id_acidente = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                TimeSpan hora = TimeSpan.Zero;
                object horaObj = reader["hora_acidente"];
                if (horaObj is TimeSpan ts) hora = ts;
                else if (horaObj != null) TimeSpan.TryParse(horaObj.ToString(), out hora);

                return new Acidente
                {
                    IdAcidente = reader.GetInt32("id_acidente"),
                    DataAcidente = reader.GetDateTime("data_acidente"),
                    HoraAcidente = hora,
                    EnderecoAcidente = reader.GetString("endereco_acidente"),
                    MotivoAcidente = reader.IsDBNull(reader.GetOrdinal("motivo_acidente")) ? null : reader.GetString("motivo_acidente"),
                    AutomoveisEnvolvidos = reader.IsDBNull(reader.GetOrdinal("automoveis_envolvidos")) ? null : reader.GetString("automoveis_envolvidos"),
                    DanosTerceiros = reader.IsDBNull(reader.GetOrdinal("danos_terceiros")) ? null : reader.GetString("danos_terceiros"),
                    IdAutomovel = reader.GetInt32("id_automovel"),
                    PlacaAutomovel = reader.GetString("placa_automovel")
                };
            }
            return null;
        }

        public async Task<bool> AtualizarAsync(Acidente acidente)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"UPDATE acidente 
                                 SET data_acidente = @data, 
                                     hora_acidente = @hora, 
                                     endereco_acidente = @endereco, 
                                     motivo_acidente = @motivo, 
                                     automoveis_envolvidos = @envolvidos, 
                                     danos_terceiros = @danos, 
                                     id_automovel = @idAutomovel 
                                 WHERE id_acidente = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@data", acidente.DataAcidente);
            cmd.Parameters.AddWithValue("@hora", acidente.HoraAcidente);
            cmd.Parameters.AddWithValue("@endereco", acidente.EnderecoAcidente.Trim());
            cmd.Parameters.AddWithValue("@motivo", (object?)acidente.MotivoAcidente ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@envolvidos", (object?)acidente.AutomoveisEnvolvidos ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@danos", (object?)acidente.DanosTerceiros ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@idAutomovel", acidente.IdAutomovel);
            cmd.Parameters.AddWithValue("@id", acidente.IdAcidente);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "DELETE FROM acidente WHERE id_acidente = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync();

            const string sql = "SELECT COUNT(1) FROM acidente WHERE id_acidente = @id";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }
    }
}
