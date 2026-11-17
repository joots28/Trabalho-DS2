using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SistemaSeguro.Model;

namespace SistemaSeguro.DAO
{
    public class AcidenteDAO
    {
        

        public bool Cadastrar(Acidente acidente) => Inserir(acidente) > 0;

        public int Inserir(Acidente acidente)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"INSERT INTO acidente (data_acidente, hora_acidente, endereco_acidente, motivo_acidente, automoveis_envolvidos, danos_terceiros, id_automovel) 
                                   VALUES (@data, @hora, @endereco, @motivo, @envolvidos, @danos, @idAutomovel);
                                   SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@data", acidente.DataAcidente == DateTime.MinValue ? DateTime.Today : acidente.DataAcidente);
                        cmd.Parameters.AddWithValue("@hora", acidente.HoraAcidente);
                        cmd.Parameters.AddWithValue("@endereco", acidente.EnderecoAcidente ?? string.Empty);
                        cmd.Parameters.AddWithValue("@motivo", (object)acidente.MotivoAcidente ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@envolvidos", (object)acidente.AutomoveisEnvolvidos ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@danos", (object)acidente.DanosTerceiros ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@idAutomovel", acidente.IdAutomovel);

                        int idCriado = Convert.ToInt32(cmd.ExecuteScalar());
                        acidente.IdAcidente = idCriado;
                        return idCriado;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao registrar acidente: " + ex.Message, ex);
            }
        }

        public DataTable Listar()
        {
            DataTable tabela = new DataTable();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT ac.id_acidente AS 'ID', 
                                          ac.data_acidente AS 'Data', 
                                          ac.hora_acidente AS 'Hora', 
                                          ac.endereco_acidente AS 'Endereço', 
                                          ac.motivo_acidente AS 'Motivo', 
                                          au.placa AS 'Placa do Veículo',
                                          au.modelo AS 'Modelo'
                                   FROM acidente ac
                                   INNER JOIN automovel au ON ac.id_automovel = au.id_automovel
                                   ORDER BY ac.id_acidente DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conexao))
                    {
                        adapter.Fill(tabela);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar acidentes: " + ex.Message, ex);
            }
            return tabela;
        }

        public List<Acidente> ListarTodos()
        {
            var lista = new List<Acidente>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_acidente, data_acidente, hora_acidente, endereco_acidente, motivo_acidente, automoveis_envolvidos, danos_terceiros, id_automovel FROM acidente ORDER BY id_acidente ASC";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ConverterReader(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar acidentes: " + ex.Message, ex);
            }
            return lista;
        }

        public Acidente ObterPorId(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_acidente, data_acidente, hora_acidente, endereco_acidente, motivo_acidente, automoveis_envolvidos, danos_terceiros, id_automovel FROM acidente WHERE id_acidente = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return ConverterReader(reader);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar acidente {id}: " + ex.Message, ex);
            }
        }

        public List<Acidente> ListarPorAutomovel(int idAutomovel)
        {
            var lista = new List<Acidente>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_acidente, data_acidente, hora_acidente, endereco_acidente, motivo_acidente, automoveis_envolvidos, danos_terceiros, id_automovel FROM acidente WHERE id_automovel = @idAutomovel";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@idAutomovel", idAutomovel);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(ConverterReader(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar acidentes do automóvel {idAutomovel}: " + ex.Message, ex);
            }
            return lista;
        }

        public bool Atualizar(Acidente acidente)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"UPDATE acidente 
                                   SET data_acidente = @data, 
                                       hora_acidente = @hora, 
                                       endereco_acidente = @endereco, 
                                       motivo_acidente = @motivo, 
                                       automoveis_envolvidos = @envolvidos, 
                                       danos_terceiros = @danos, 
                                       id_automovel = @idAutomovel 
                                   WHERE id_acidente = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@data", acidente.DataAcidente == DateTime.MinValue ? DateTime.Today : acidente.DataAcidente);
                        cmd.Parameters.AddWithValue("@hora", acidente.HoraAcidente);
                        cmd.Parameters.AddWithValue("@endereco", acidente.EnderecoAcidente ?? string.Empty);
                        cmd.Parameters.AddWithValue("@motivo", (object)acidente.MotivoAcidente ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@envolvidos", (object)acidente.AutomoveisEnvolvidos ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@danos", (object)acidente.DanosTerceiros ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@idAutomovel", acidente.IdAutomovel);
                        cmd.Parameters.AddWithValue("@id", acidente.IdAcidente);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar acidente {acidente.IdAcidente}: " + ex.Message, ex);
            }
        }

        public bool Excluir(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "DELETE FROM acidente WHERE id_acidente = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir acidente {id}: " + ex.Message, ex);
            }
        }

        public bool Existe(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT COUNT(1) FROM acidente WHERE id_acidente = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar acidente {id}: " + ex.Message, ex);
            }
        }

        private Acidente ConverterReader(MySqlDataReader reader)
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
                IdAutomovel = reader.GetInt32("id_automovel")
            };
        }
    }
}
