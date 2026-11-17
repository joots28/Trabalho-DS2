using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaSeguro.Api.DTOs;
using SistemaSeguro.Api.Models;
using SistemaSeguro.Api.Repositories;

namespace SistemaSeguro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AutomoveisController : ControllerBase
    {
        private readonly IAutomovelRepository _automovelRepo;
        private readonly IClienteRepository _clienteRepo;

        public AutomoveisController(IAutomovelRepository automovelRepo, IClienteRepository clienteRepo)
        {
            _automovelRepo = automovelRepo;
            _clienteRepo = clienteRepo;
        }

        /// <summary>
        /// Lista todos os automóveis cadastrados, opcionalmente filtrando por ID do cliente.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<Automovel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery] int? idCliente)
        {
            var automoveis = await _automovelRepo.ListarTodosAsync(idCliente);
            return Ok(ApiResponse<List<Automovel>>.Ok(automoveis, $"{automoveis.Count} automóvel(is) retornado(s)."));
        }

        /// <summary>
        /// Obtém os detalhes de um automóvel por ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Automovel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var auto = await _automovelRepo.ObterPorIdAsync(id);
            if (auto == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Automóvel com ID {id} não encontrado."));
            }
            return Ok(ApiResponse<Automovel>.Ok(auto));
        }

        /// <summary>
        /// Cadastra um novo automóvel vinculado a um cliente.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Automovel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar([FromBody] AutomovelCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            string placaLimpa = dto.Placa.Trim().ToUpper().Replace("-", "");
            var regexPlaca = new Regex(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$");
            if (!regexPlaca.IsMatch(placaLimpa))
            {
                return BadRequest(ApiResponse<string>.Fail("Placa inválida. Utilize o formato Mercosul (ABC1D23) ou tradicional (ABC1234)."));
            }

            if (!await _clienteRepo.ExisteAsync(dto.IdCliente))
            {
                return BadRequest(ApiResponse<string>.Fail($"Cliente com ID {dto.IdCliente} não existe no sistema."));
            }

            var auto = new Automovel
            {
                Placa = placaLimpa,
                Modelo = dto.Modelo.Trim(),
                Marca = dto.Marca.Trim(),
                Ano = dto.Ano,
                IdCliente = dto.IdCliente
            };

            await _automovelRepo.InserirAsync(auto);
            var criado = await _automovelRepo.ObterPorIdAsync(auto.IdAutomovel);
            return CreatedAtAction(nameof(ObterPorId), new { id = auto.IdAutomovel }, ApiResponse<Automovel>.Ok(criado ?? auto, "Automóvel cadastrado com sucesso."));
        }

        /// <summary>
        /// Atualiza os dados de um automóvel existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Automovel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AutomovelCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            var existente = await _automovelRepo.ObterPorIdAsync(id);
            if (existente == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Automóvel com ID {id} não encontrado."));
            }

            string placaLimpa = dto.Placa.Trim().ToUpper().Replace("-", "");
            var regexPlaca = new Regex(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$");
            if (!regexPlaca.IsMatch(placaLimpa))
            {
                return BadRequest(ApiResponse<string>.Fail("Placa inválida. Utilize o formato Mercosul (ABC1D23) ou tradicional (ABC1234)."));
            }

            if (!await _clienteRepo.ExisteAsync(dto.IdCliente))
            {
                return BadRequest(ApiResponse<string>.Fail($"Cliente com ID {dto.IdCliente} não existe no sistema."));
            }

            existente.Placa = placaLimpa;
            existente.Modelo = dto.Modelo.Trim();
            existente.Marca = dto.Marca.Trim();
            existente.Ano = dto.Ano;
            existente.IdCliente = dto.IdCliente;

            await _automovelRepo.AtualizarAsync(existente);
            var atualizado = await _automovelRepo.ObterPorIdAsync(id);
            return Ok(ApiResponse<Automovel>.Ok(atualizado ?? existente, "Automóvel atualizado com sucesso."));
        }

        /// <summary>
        /// Remove um automóvel do sistema.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(int id)
        {
            var existe = await _automovelRepo.ExisteAsync(id);
            if (!existe)
            {
                return NotFound(ApiResponse<string>.Fail($"Automóvel com ID {id} não encontrado."));
            }

            await _automovelRepo.ExcluirAsync(id);
            return Ok(ApiResponse<bool>.Ok(true, "Automóvel excluído com sucesso."));
        }

        private List<string> ObterErrosModelState()
        {
            var erros = new List<string>();
            foreach (var state in ModelState.Values)
            {
                foreach (var err in state.Errors)
                {
                    erros.Add(err.ErrorMessage);
                }
            }
            return erros;
        }
    }
}
