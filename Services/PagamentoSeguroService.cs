using System;
using System.Collections.Generic;
using SistemaSeguro.DAO;
using SistemaSeguro.Model;

namespace SistemaSeguro.Services
{
    public class PagamentoSeguroService
    {
        private readonly PagamentoSeguroDAO _dao;
        private readonly ClienteDAO _clienteDao;
        private readonly AcidenteDAO _acidenteDao;

        public PagamentoSeguroService(PagamentoSeguroDAO dao = null, ClienteDAO clienteDao = null, AcidenteDAO acidenteDao = null)
        {
            _dao = dao ?? new PagamentoSeguroDAO();
            _clienteDao = clienteDao ?? new ClienteDAO();
            _acidenteDao = acidenteDao ?? new AcidenteDAO();
        }

        public int Cadastrar(PagamentoSeguro pgto)
        {
            Validar(pgto);
            return _dao.Inserir(pgto);
        }

        public bool Atualizar(PagamentoSeguro pgto)
        {
            if (pgto == null || pgto.IdPagamento <= 0)
                throw new ArgumentException("ID do pagamento inválido para atualização.");

            if (!_dao.Existe(pgto.IdPagamento))
                throw new KeyNotFoundException($"Pagamento com ID {pgto.IdPagamento} não encontrado.");

            Validar(pgto);
            return _dao.Atualizar(pgto);
        }

        public bool Excluir(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do pagamento inválido.");

            if (!_dao.Existe(id))
                throw new KeyNotFoundException($"Pagamento com ID {id} não encontrado.");

            return _dao.Excluir(id);
        }

        public PagamentoSeguro ObterPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do pagamento inválido.");

            return _dao.ObterPorId(id);
        }

        public List<PagamentoSeguro> ListarTodos()
        {
            return _dao.ListarTodos();
        }

        public List<PagamentoSeguro> ListarPorCliente(int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException("ID do cliente inválido.");

            return _dao.ListarPorCliente(idCliente);
        }

        public void Validar(PagamentoSeguro pgto)
        {
            if (pgto == null)
                throw new ArgumentNullException(nameof(pgto), "Os dados do pagamento não podem ser nulos.");

            if (pgto.Valor <= 0)
                throw new ArgumentException("O valor do pagamento de indenização deve ser maior que zero.");

            if (pgto.DataPagamento > DateTime.Today.AddDays(1))
                throw new ArgumentException("A data do pagamento não pode ser uma data futura distante.");

            if (pgto.IdCliente <= 0)
                throw new ArgumentException("É obrigatório informar o cliente beneficiário.");

            if (!_clienteDao.Existe(pgto.IdCliente))
                throw new ArgumentException($"Cliente com ID {pgto.IdCliente} não existe no sistema.");

            if (pgto.IdAcidente <= 0)
                throw new ArgumentException("É obrigatório associar o sinistro/acidente ao pagamento.");

            if (!_acidenteDao.Existe(pgto.IdAcidente))
                throw new ArgumentException($"Acidente com ID {pgto.IdAcidente} não existe no sistema.");
        }
    }
}
