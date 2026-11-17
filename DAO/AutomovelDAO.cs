using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SistemaSeguro.Model;

namespace SistemaSeguro.DAO
{
    public class AutomovelDAO
    {
        

        public bool Cadastrar(Automovel auto) => Inserir(auto) > 0;

        public int Inserir(Automovel auto)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"INSERT INTO automovel (placa, modelo, marca, ano, id_cliente) 
                                   VALUES (@placa, @modelo, @marca, @ano, @idCliente);
                                   SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@placa", auto.Placa ?? string.Empty);
                        cmd.Parameters.AddWithValue("@modelo", auto.Modelo ?? string.Empty);
                        cmd.Parameters.AddWithValue("@marca", auto.Marca ?? string.Empty);
                        cmd.Parameters.AddWithValue("@ano", auto.Ano);
                        cmd.Parameters.AddWithValue("@idCliente", auto.IdCliente);

                        int idCriado = Convert.ToInt32(cmd.ExecuteScalar());
                        auto.IdAutomovel = idCriado;
                        return idCriado;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar automóvel: " + ex.Message, ex);
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
                    string sql = @"SELECT a.id_automovel AS 'ID', 
                                          a.placa AS 'Placa', 
                                          a.modelo AS 'Modelo', 
                                          a.marca AS 'Marca', 
                                          a.ano AS 'Ano', 
                                          a.id_cliente AS 'ID Cliente',
                                          c.nome AS 'Proprietário'
                                   FROM automovel a
                                   INNER JOIN cliente c ON a.id_cliente = c.id_cliente
                                   ORDER BY a.id_automovel DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conexao))
                    {
                        adapter.Fill(tabela);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar automóveis: " + ex.Message, ex);
            }
            return tabela;
        }

        public List<Automovel> ListarTodos()
        {
            var lista = new List<Automovel>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_automovel, placa, modelo, marca, ano, id_cliente FROM automovel ORDER BY id_automovel ASC";

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
                throw new Exception("Erro ao consultar automóveis: " + ex.Message, ex);
            }
            return lista;
        }

        public Automovel ObterPorId(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_automovel, placa, modelo, marca, ano, id_cliente FROM automovel WHERE id_automovel = @id";

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
                throw new Exception($"Erro ao buscar automóvel {id}: " + ex.Message, ex);
            }
        }

        public List<Automovel> ListarPorCliente(int idCliente)
        {
            var lista = new List<Automovel>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_automovel, placa, modelo, marca, ano, id_cliente FROM automovel WHERE id_cliente = @idCliente";

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
                throw new Exception($"Erro ao consultar automóveis do cliente {idCliente}: " + ex.Message, ex);
            }
            return lista;
        }

        public bool Atualizar(Automovel auto)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"UPDATE automovel 
                                   SET placa = @placa, 
                                       modelo = @modelo, 
                                       marca = @marca, 
                                       ano = @ano, 
                                       id_cliente = @idCliente 
                                   WHERE id_automovel = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@placa", auto.Placa ?? string.Empty);
                        cmd.Parameters.AddWithValue("@modelo", auto.Modelo ?? string.Empty);
                        cmd.Parameters.AddWithValue("@marca", auto.Marca ?? string.Empty);
                        cmd.Parameters.AddWithValue("@ano", auto.Ano);
                        cmd.Parameters.AddWithValue("@idCliente", auto.IdCliente);
                        cmd.Parameters.AddWithValue("@id", auto.IdAutomovel);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar automóvel {auto.IdAutomovel}: " + ex.Message, ex);
            }
        }

        public bool Excluir(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "DELETE FROM automovel WHERE id_automovel = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir automóvel {id}: " + ex.Message, ex);
            }
        }

        public bool Existe(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT COUNT(1) FROM automovel WHERE id_automovel = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar automóvel {id}: " + ex.Message, ex);
            }
        }

        private Automovel ConverterReader(MySqlDataReader reader)
        {
            return new Automovel
            {
                IdAutomovel = reader.GetInt32("id_automovel"),
                Placa = reader.GetString("placa"),
                Modelo = reader.GetString("modelo"),
                Marca = reader.GetString("marca"),
                Ano = reader.IsDBNull(reader.GetOrdinal("ano")) ? 0 : reader.GetInt32("ano"),
                IdCliente = reader.GetInt32("id_cliente")
            };
        }
    }
}
