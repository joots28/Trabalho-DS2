using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SistemaSeguro.Model;

namespace SistemaSeguro.DAO
{
    public class ClienteDAO
    {
        

        public bool Cadastrar(Cliente cliente) => Inserir(cliente) > 0;

        public int Inserir(Cliente cliente)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"INSERT INTO cliente (nome, endereco, telefone, email, data_nascimento, profissao) 
                                   VALUES (@nome, @endereco, @telefone, @email, @dataNasc, @profissao);
                                   SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", cliente.Nome ?? string.Empty);
                        cmd.Parameters.AddWithValue("@endereco", cliente.Endereco ?? string.Empty);
                        cmd.Parameters.AddWithValue("@telefone", (object)cliente.Telefone ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@email", (object)cliente.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@dataNasc", cliente.DataNascimento == DateTime.MinValue ? (object)DBNull.Value : cliente.DataNascimento);
                        cmd.Parameters.AddWithValue("@profissao", (object)cliente.Profissao ?? DBNull.Value);

                        int idCriado = Convert.ToInt32(cmd.ExecuteScalar());
                        cliente.IdCliente = idCriado;
                        return idCriado;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar cliente: " + ex.Message, ex);
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
                    string sql = @"SELECT id_cliente AS 'ID', 
                                          nome AS 'Nome', 
                                          endereco AS 'Endereço',
                                          telefone AS 'Telefone', 
                                          email AS 'E-mail', 
                                          data_nascimento AS 'Nascimento', 
                                          profissao AS 'Profissão' 
                                   FROM cliente 
                                   ORDER BY id_cliente DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conexao))
                    {
                        adapter.Fill(tabela);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar clientes: " + ex.Message, ex);
            }
            return tabela;
        }

        public List<Cliente> ListarTodos()
        {
            var lista = new List<Cliente>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_cliente, nome, endereco, telefone, email, data_nascimento, profissao FROM cliente ORDER BY id_cliente ASC";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ConverterReaderParaCliente(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar clientes: " + ex.Message, ex);
            }
            return lista;
        }

        public Cliente ObterPorId(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_cliente, nome, endereco, telefone, email, data_nascimento, profissao FROM cliente WHERE id_cliente = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return ConverterReaderParaCliente(reader);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar cliente {id}: " + ex.Message, ex);
            }
        }

        public bool Atualizar(Cliente cliente)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"UPDATE cliente 
                                   SET nome = @nome, 
                                       endereco = @endereco, 
                                       telefone = @telefone, 
                                       email = @email, 
                                       data_nascimento = @dataNasc, 
                                       profissao = @profissao 
                                   WHERE id_cliente = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", cliente.Nome ?? string.Empty);
                        cmd.Parameters.AddWithValue("@endereco", cliente.Endereco ?? string.Empty);
                        cmd.Parameters.AddWithValue("@telefone", (object)cliente.Telefone ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@email", (object)cliente.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@dataNasc", cliente.DataNascimento == DateTime.MinValue ? (object)DBNull.Value : cliente.DataNascimento);
                        cmd.Parameters.AddWithValue("@profissao", (object)cliente.Profissao ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", cliente.IdCliente);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar cliente {cliente.IdCliente}: " + ex.Message, ex);
            }
        }

        public bool Excluir(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "DELETE FROM cliente WHERE id_cliente = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir cliente {id}: " + ex.Message, ex);
            }
        }

        public bool Existe(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT COUNT(1) FROM cliente WHERE id_cliente = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar cliente {id}: " + ex.Message, ex);
            }
        }

        private Cliente ConverterReaderParaCliente(MySqlDataReader reader)
        {
            return new Cliente
            {
                IdCliente = reader.GetInt32("id_cliente"),
                Nome = reader.GetString("nome"),
                Endereco = reader.IsDBNull(reader.GetOrdinal("endereco")) ? string.Empty : reader.GetString("endereco"),
                Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString("telefone"),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                DataNascimento = reader.IsDBNull(reader.GetOrdinal("data_nascimento")) ? DateTime.MinValue : reader.GetDateTime("data_nascimento"),
                Profissao = reader.IsDBNull(reader.GetOrdinal("profissao")) ? null : reader.GetString("profissao")
            };
        }
    }
}
