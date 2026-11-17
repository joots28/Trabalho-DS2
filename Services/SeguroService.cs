using System;
using System.Collections.Generic;
using SistemaSeguro.DAO;
using SistemaSeguro.Model;

namespace SistemaSeguro.Services
{
    public class SeguroService
    {
        private readonly SeguroDAO _dao;
        private readonly ClienteDAO _clienteDao;
        private readonly AutomovelDAO _automovelDao;

        public SeguroService(SeguroDAO dao = null, ClienteDAO clienteDao = null, AutomovelDAO automovelDao = null)
        {
            _dao = dao ?? new SeguroDAO();
            _clienteDao = clienteDao ?? new ClienteDAO();
            _automovelDao = automovelDao ?? new AutomovelDAO();
        }

        public int Cadastrar(Seguro seguro)
        {
            Validar(seguro);
            return _dao.Inserir(seguro);
        }

        public bool Atualizar(Seguro seguro)
        {
            if (seguro == null || seguro.IdSeguro <= 0)
                throw new ArgumentException("ID do seguro inválido para atualização.");

            if (!_dao.Existe(seguro.IdSeguro))
                throw new KeyNotFoundException($"Seguro com ID {seguro.IdSeguro} não encontrado.");

            Validar(seguro);
            return _dao.Atualizar(seguro);
        }

        public bool Excluir(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do seguro inválido.");

            if (!_dao.Existe(id))
                throw new KeyNotFoundException($"Seguro com ID {id} não encontrado.");

            return _dao.Excluir(id);
        }

        public Seguro ObterPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do seguro inválido.");

            return _dao.ObterPorId(id);
        }

        public List<Seguro> ListarTodos()
        {
            return _dao.ListarTodos();
        }

        public List<Seguro> ListarPorCliente(int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException("ID do cliente inválido.");

            return _dao.ListarPorCliente(idCliente);
        }

        public void Validar(Seguro seguro)
        {
            if (seguro == null)
                throw new ArgumentNullException(nameof(seguro), "Os dados do seguro não podem ser nulos.");

            if (string.IsNullOrWhiteSpace(seguro.NumeroApolice))
                throw new ArgumentException("O número da apólice é obrigatório.");

            if (seguro.ValorCobertura <= 0)
                throw new ArgumentException("O valor de cobertura do seguro deve ser maior que zero.");

            if (seguro.IdCliente <= 0)
                throw new ArgumentException("É obrigatório informar o cliente titular do seguro.");

            if (!_clienteDao.Existe(seguro.IdCliente))
                throw new ArgumentException($"Cliente com ID {seguro.IdCliente} não existe no sistema.");

            if (seguro.IdAutomovel <= 0)
                throw new ArgumentException("É obrigatório informar o automóvel segurado.");

            if (!_automovelDao.Existe(seguro.IdAutomovel))
                throw new ArgumentException($"Automóvel com ID {seguro.IdAutomovel} não existe no sistema.");
        }
    }
}
