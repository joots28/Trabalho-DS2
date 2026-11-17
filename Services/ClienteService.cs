using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SistemaSeguro.DAO;
using SistemaSeguro.Model;

namespace SistemaSeguro.Services
{
    public class ClienteService
    {
        private readonly ClienteDAO _dao;

        public ClienteService(ClienteDAO dao = null)
        {
            _dao = dao ?? new ClienteDAO();
        }

        public int Cadastrar(Cliente cliente)
        {
            Validar(cliente);
            return _dao.Inserir(cliente);
        }

        public bool Atualizar(Cliente cliente)
        {
            if (cliente == null || cliente.IdCliente <= 0)
                throw new ArgumentException("ID do cliente inválido para atualização.");

            if (!_dao.Existe(cliente.IdCliente))
                throw new KeyNotFoundException($"Cliente com ID {cliente.IdCliente} não encontrado.");

            Validar(cliente);
            return _dao.Atualizar(cliente);
        }

        public bool Excluir(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do cliente inválido.");

            if (!_dao.Existe(id))
                throw new KeyNotFoundException($"Cliente com ID {id} não encontrado.");

            return _dao.Excluir(id);
        }

        public Cliente ObterPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do cliente inválido.");

            return _dao.ObterPorId(id);
        }

        public List<Cliente> ListarTodos()
        {
            return _dao.ListarTodos();
        }

        public void Validar(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente), "Os dados do cliente não podem ser nulos.");

            if (string.IsNullOrWhiteSpace(cliente.Nome))
                throw new ArgumentException("O nome do cliente é obrigatório.");

            if (cliente.Nome.Trim().Length < 3 || cliente.Nome.Trim().Length > 100)
                throw new ArgumentException("O nome do cliente deve ter entre 3 e 100 caracteres.");

            if (string.IsNullOrWhiteSpace(cliente.Endereco))
                throw new ArgumentException("O endereço do cliente é obrigatório.");

            if (!string.IsNullOrWhiteSpace(cliente.Email))
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(cliente.Email.Trim()))
                    throw new ArgumentException("O e-mail informado possui formato inválido.");
            }

            if (cliente.DataNascimento != DateTime.MinValue && cliente.DataNascimento > DateTime.Today)
                throw new ArgumentException("A data de nascimento não pode ser no futuro.");
        }
    }
}
