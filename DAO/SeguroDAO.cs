using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SistemaSeguro.Model;

namespace SistemaSeguro.DAO
{
    public class SeguroDAO
    {
        

        public bool Cadastrar(Seguro seguro) => Inserir(seguro) > 0;

        public int Inserir(Seguro seguro)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"INSERT INTO seguro (numero_apolice, valor_cobertura, id_cliente, id_automovel) 
                                   VALUES (@numApolice, @valor, @idCliente, @idAutomovel);
                                   SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@numApolice", seguro.NumeroApolice ?? string.Empty);
                        cmd.Parameters.AddWithValue("@valor", seguro.ValorCobertura);
                        cmd.Parameters.AddWithValue("@idCliente", seguro.IdCliente);
                        cmd.Parameters.AddWithValue("@idAutomovel", seguro.IdAutomovel);

                        int idCriado = Convert.ToInt32(cmd.ExecuteScalar());
                        seguro.IdSeguro = idCriado;
                        return idCriado;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar seguro: " + ex.Message, ex);
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
                    string sql = @"SELECT s.id_seguro AS 'ID', 
                                          s.numero_apolice AS 'Apólice', 
                                          s.valor_cobertura AS 'Valor Cobertura', 
                                          c.nome AS 'Cliente', 
                                          a.placa AS 'Placa do Veículo',
                                          a.modelo AS 'Modelo'
                                   FROM seguro s
                                   INNER JOIN cliente c ON s.id_cliente = c.id_cliente
                                   INNER JOIN automovel a ON s.id_automovel = a.id_automovel
                                   ORDER BY s.id_seguro DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conexao))
                    {
                        adapter.Fill(tabela);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar seguros: " + ex.Message, ex);
            }
            return tabela;
        }

        public List<Seguro> ListarTodos()
        {
            var lista = new List<Seguro>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_seguro, numero_apolice, valor_cobertura, id_cliente, id_automovel FROM seguro ORDER BY id_seguro ASC";

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
                throw new Exception("Erro ao consultar seguros: " + ex.Message, ex);
            }
            return lista;
        }

        public Seguro ObterPorId(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_seguro, numero_apolice, valor_cobertura, id_cliente, id_automovel FROM seguro WHERE id_seguro = @id";

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
                throw new Exception($"Erro ao buscar seguro {id}: " + ex.Message, ex);
            }
        }

        public List<Seguro> ListarPorCliente(int idCliente)
        {
            var lista = new List<Seguro>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_seguro, numero_apolice, valor_cobertura, id_cliente, id_automovel FROM seguro WHERE id_cliente = @idCliente";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@idCliente", idCliente);
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
                throw new Exception($"Erro ao consultar seguros do cliente {idCliente}: " + ex.Message, ex);
            }
            return lista;
        }

        public bool Atualizar(Seguro seguro)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"UPDATE seguro 
                                   SET numero_apolice = @numApolice, 
                                       valor_cobertura = @valor, 
                                       id_cliente = @idCliente, 
                                       id_automovel = @idAutomovel 
                                   WHERE id_seguro = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@numApolice", seguro.NumeroApolice ?? string.Empty);
                        cmd.Parameters.AddWithValue("@valor", seguro.ValorCobertura);
                        cmd.Parameters.AddWithValue("@idCliente", seguro.IdCliente);
                        cmd.Parameters.AddWithValue("@idAutomovel", seguro.IdAutomovel);
                        cmd.Parameters.AddWithValue("@id", seguro.IdSeguro);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar seguro {seguro.IdSeguro}: " + ex.Message, ex);
            }
        }

        public bool Excluir(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "DELETE FROM seguro WHERE id_seguro = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir seguro {id}: " + ex.Message, ex);
            }
        }

        public bool Existe(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT COUNT(1) FROM seguro WHERE id_seguro = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar seguro {id}: " + ex.Message, ex);
            }
        }

        private Seguro ConverterReader(MySqlDataReader reader)
        {
            return new Seguro
            {
                IdSeguro = reader.GetInt32("id_seguro"),
                NumeroApolice = reader.GetString("numero_apolice"),
                ValorCobertura = reader.IsDBNull(reader.GetOrdinal("valor_cobertura")) ? 0 : reader.GetDecimal("valor_cobertura"),
                IdCliente = reader.GetInt32("id_cliente"),
                IdAutomovel = reader.GetInt32("id_automovel")
            };
        }
    }
}
