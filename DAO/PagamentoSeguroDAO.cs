using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SistemaSeguro.Model;

namespace SistemaSeguro.DAO
{
    public class PagamentoSeguroDAO
    {
        public bool Cadastrar(PagamentoSeguro pgto) => Inserir(pgto) > 0;

        public int Inserir(PagamentoSeguro pgto)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"INSERT INTO pagamento (valor, data_pagamento, id_cliente, id_acidente) 
                                   VALUES (@valor, @dataPgto, @idCliente, @idAcidente);
                                   SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@valor", pgto.Valor);
                        cmd.Parameters.AddWithValue("@dataPgto", pgto.DataPagamento == DateTime.MinValue ? DateTime.Today : pgto.DataPagamento);
                        cmd.Parameters.AddWithValue("@idCliente", pgto.IdCliente);
                        cmd.Parameters.AddWithValue("@idAcidente", pgto.IdAcidente);

                        int idCriado = Convert.ToInt32(cmd.ExecuteScalar());
                        pgto.IdPagamento = idCriado;
                        return idCriado;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao registrar pagamento: " + ex.Message, ex);
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
                    string sql = @"SELECT p.id_pagamento AS 'ID', 
                                          p.valor AS 'Valor', 
                                          p.data_pagamento AS 'Data', 
                                          c.nome AS 'Cliente', 
                                          p.id_acidente AS 'Nº Sinistro',
                                          ac.endereco_acidente AS 'Local Sinistro'
                                   FROM pagamento p
                                   INNER JOIN cliente c ON p.id_cliente = c.id_cliente
                                   LEFT JOIN acidente ac ON p.id_acidente = ac.id_acidente
                                   ORDER BY p.id_pagamento DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conexao))
                    {
                        adapter.Fill(tabela);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar pagamentos: " + ex.Message, ex);
            }
            return tabela;
        }

        public List<PagamentoSeguro> ListarTodos()
        {
            var lista = new List<PagamentoSeguro>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_pagamento, valor, data_pagamento, id_cliente, id_acidente FROM pagamento ORDER BY id_pagamento ASC";

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
                throw new Exception("Erro ao consultar pagamentos: " + ex.Message, ex);
            }
            return lista;
        }

        public PagamentoSeguro ObterPorId(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_pagamento, valor, data_pagamento, id_cliente, id_acidente FROM pagamento WHERE id_pagamento = @id";

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
                throw new Exception($"Erro ao buscar pagamento {id}: " + ex.Message, ex);
            }
        }

        public List<PagamentoSeguro> ListarPorCliente(int idCliente)
        {
            var lista = new List<PagamentoSeguro>();
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT id_pagamento, valor, data_pagamento, id_cliente, id_acidente FROM pagamento WHERE id_cliente = @idCliente";

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
                throw new Exception($"Erro ao consultar pagamentos do cliente {idCliente}: " + ex.Message, ex);
            }
            return lista;
        }

        public bool Atualizar(PagamentoSeguro pgto)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"UPDATE pagamento 
                                   SET valor = @valor, 
                                       data_pagamento = @dataPgto, 
                                       id_cliente = @idCliente, 
                                       id_acidente = @idAcidente 
                                   WHERE id_pagamento = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@valor", pgto.Valor);
                        cmd.Parameters.AddWithValue("@dataPgto", pgto.DataPagamento == DateTime.MinValue ? DateTime.Today : pgto.DataPagamento);
                        cmd.Parameters.AddWithValue("@idCliente", pgto.IdCliente);
                        cmd.Parameters.AddWithValue("@idAcidente", pgto.IdAcidente);
                        cmd.Parameters.AddWithValue("@id", pgto.IdPagamento);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar pagamento {pgto.IdPagamento}: " + ex.Message, ex);
            }
        }

        public bool Excluir(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "DELETE FROM pagamento WHERE id_pagamento = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir pagamento {id}: " + ex.Message, ex);
            }
        }

        public bool Existe(int id)
        {
            try
            {
                using (MySqlConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT COUNT(1) FROM pagamento WHERE id_pagamento = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar pagamento {id}: " + ex.Message, ex);
            }
        }

        private PagamentoSeguro ConverterReader(MySqlDataReader reader)
        {
            return new PagamentoSeguro
            {
                IdPagamento = reader.GetInt32("id_pagamento"),
                Valor = reader.GetDecimal("valor"),
                DataPagamento = reader.GetDateTime("data_pagamento"),
                IdCliente = reader.GetInt32("id_cliente"),
                IdAcidente = reader.GetInt32("id_acidente")
            };
        }
    }
}
