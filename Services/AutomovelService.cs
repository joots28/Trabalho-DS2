using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SistemaSeguro.DAO;
using SistemaSeguro.Model;

namespace SistemaSeguro.Services
{
    public class AutomovelService
    {
        private readonly AutomovelDAO _dao;
        private readonly ClienteDAO _clienteDao;

        public AutomovelService(AutomovelDAO dao = null, ClienteDAO clienteDao = null)
        {
            _dao = dao ?? new AutomovelDAO();
            _clienteDao = clienteDao ?? new ClienteDAO();
        }

        public int Cadastrar(Automovel auto)
        {
            Validar(auto);
            return _dao.Inserir(auto);
        }

        public bool Atualizar(Automovel auto)
        {
            if (auto == null || auto.IdAutomovel <= 0)
                throw new ArgumentException("ID do automóvel inválido para atualização.");

            if (!_dao.Existe(auto.IdAutomovel))
                throw new KeyNotFoundException($"Automóvel com ID {auto.IdAutomovel} não encontrado.");

            Validar(auto);
            return _dao.Atualizar(auto);
        }

        public bool Excluir(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do automóvel inválido.");

            if (!_dao.Existe(id))
                throw new KeyNotFoundException($"Automóvel com ID {id} não encontrado.");

            return _dao.Excluir(id);
        }

        public Automovel ObterPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID do automóvel inválido.");

            return _dao.ObterPorId(id);
        }

        public List<Automovel> ListarTodos()
        {
            return _dao.ListarTodos();
        }

        public List<Automovel> ListarPorCliente(int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException("ID do cliente inválido.");

            return _dao.ListarPorCliente(idCliente);
        }

        public void Validar(Automovel auto)
        {
            if (auto == null)
                throw new ArgumentNullException(nameof(auto), "Os dados do automóvel não podem ser nulos.");

            if (string.IsNullOrWhiteSpace(auto.Placa))
                throw new ArgumentException("A placa do automóvel é obrigatória.");

            string placaLimpa = auto.Placa.Trim().ToUpper().Replace("-", "");
            // Placa Mercosul (ABC1D23) ou tradicional (ABC1234)
            var regexPlaca = new Regex(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$");
            if (!regexPlaca.IsMatch(placaLimpa))
                throw new ArgumentException("A placa informada é inválida. Utilize o formato tradicional (ABC1234) ou Mercosul (ABC1D23).");

            if (string.IsNullOrWhiteSpace(auto.Modelo))
                throw new ArgumentException("O modelo do automóvel é obrigatório.");

            if (string.IsNullOrWhiteSpace(auto.Marca))
                throw new ArgumentException("A marca do automóvel é obrigatória.");

            int anoMaximo = DateTime.Now.Year + 1;
            if (auto.Ano < 1900 || auto.Ano > anoMaximo)
                throw new ArgumentException($"O ano do automóvel deve estar entre 1900 e {anoMaximo}.");

            if (auto.IdCliente <= 0)
                throw new ArgumentException("É obrigatório associar um proprietário (Cliente) ao automóvel.");

            if (!_clienteDao.Existe(auto.IdCliente))
                throw new ArgumentException($"Cliente com ID {auto.IdCliente} não existe no sistema.");
        }
    }
}
