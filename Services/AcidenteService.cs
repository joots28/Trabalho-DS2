using System;
using System.Collections.Generic;
using SistemaSeguro.DAO;
using SistemaSeguro.Model;

namespace SistemaSeguro.Services
{
    public class AcidenteService
    {
        private readonly AcidenteDAO _dao;
        private readonly AutomovelDAO _automovelDao;

        public AcidenteService(AcidenteDAO dao = null, AutomovelDAO automovelDao = null)
        {
            _dao = dao ?? new AcidenteDAO();
            _automovelDao = automovelDao ?? new AutomovelDAO();
        }

        public int Cadastrar(Acidente acidente)
        {
            Validar(acidente);
            return _dao.Inserir(acidente);
        }

        public bool Atualizar(Acidente acidente)
        {
            if (acidente == null || acidente.IdAcidente <= 0)
                throw new ArgumentException("ID do acidente inválido para atualização.");

            if (!_dao.Existe(acidente.IdAcidente))
                throw new KeyNotFoundException($"Acidente com ID {acidente.IdAcidente} não encontrado.");

            Validar(acidente);
            return _dao.Atualizar(acidente);
        }

        public bool Excluir(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do acidente inválido.");

            if (!_dao.Existe(id))
                throw new KeyNotFoundException($"Acidente com ID {id} não encontrado.");

            return _dao.Excluir(id);
        }

        public Acidente ObterPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do acidente inválido.");

            return _dao.ObterPorId(id);
        }

        public List<Acidente> ListarTodos()
        {
            return _dao.ListarTodos();
        }

        public List<Acidente> ListarPorAutomovel(int idAutomovel)
        {
            if (idAutomovel <= 0)
                throw new ArgumentException("ID do automóvel inválido.");

            return _dao.ListarPorAutomovel(idAutomovel);
        }

        public void Validar(Acidente acidente)
        {
            if (acidente == null)
                throw new ArgumentNullException(nameof(acidente), "Os dados do acidente não podem ser nulos.");

            if (acidente.DataAcidente > DateTime.Today.AddDays(1))
                throw new ArgumentException("A data do acidente não pode ser futura.");

            if (string.IsNullOrWhiteSpace(acidente.EnderecoAcidente))
                throw new ArgumentException("O local/endereço do acidente é obrigatório.");

            if (acidente.IdAutomovel <= 0)
                throw new ArgumentException("É obrigatório associar um automóvel ao sinistro de acidente.");

            if (!_automovelDao.Existe(acidente.IdAutomovel))
                throw new ArgumentException($"Automóvel com ID {acidente.IdAutomovel} não existe no sistema.");
        }
    }
}
