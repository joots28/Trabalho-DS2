using System.Collections.Generic;
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
    public class SegurosController : ControllerBase
    {
        private readonly ISeguroRepository _seguroRepo;
        private readonly IClienteRepository _clienteRepo;
        private readonly IAutomovelRepository _automovelRepo;

        public SegurosController(ISeguroRepository seguroRepo, IClienteRepository clienteRepo, IAutomovelRepository automovelRepo)
        {
            _seguroRepo = seguroRepo;
            _clienteRepo = clienteRepo;
            _automovelRepo = automovelRepo;
        }

        /// <summary>
        /// Lista todos os contratos de seguro cadastrados, opcionalmente filtrando por ID do cliente.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<Seguro>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery] int? idCliente)
        {
            var seguros = await _seguroRepo.ListarTodosAsync(idCliente);
            return Ok(ApiResponse<List<Seguro>>.Ok(seguros, $"{seguros.Count} seguro(s) retornado(s)."));
        }

        /// <summary>
        /// Obtém os detalhes de um seguro por ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Seguro>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var seguro = await _seguroRepo.ObterPorIdAsync(id);
            if (seguro == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Seguro com ID {id} não encontrado."));
            }
            return Ok(ApiResponse<Seguro>.Ok(seguro));
        }

        /// <summary>
        /// Cadastra uma nova apólice de seguro.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Seguro>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar([FromBody] SeguroCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            if (!await _clienteRepo.ExisteAsync(dto.IdCliente))
            {
                return BadRequest(ApiResponse<string>.Fail($"Cliente com ID {dto.IdCliente} não existe no sistema."));
            }

            if (!await _automovelRepo.ExisteAsync(dto.IdAutomovel))
            {
                return BadRequest(ApiResponse<string>.Fail($"Automóvel com ID {dto.IdAutomovel} não existe no sistema."));
            }

            var seguro = new Seguro
            {
                NumeroApolice = dto.NumeroApolice.Trim(),
                ValorCobertura = dto.ValorCobertura,
                IdCliente = dto.IdCliente,
                IdAutomovel = dto.IdAutomovel
            };

            await _seguroRepo.InserirAsync(seguro);
            var criado = await _seguroRepo.ObterPorIdAsync(seguro.IdSeguro);
            return CreatedAtAction(nameof(ObterPorId), new { id = seguro.IdSeguro }, ApiResponse<Seguro>.Ok(criado ?? seguro, "Seguro cadastrado com sucesso."));
        }

        /// <summary>
        /// Atualiza uma apólice de seguro existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Seguro>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] SeguroCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            var existente = await _seguroRepo.ObterPorIdAsync(id);
            if (existente == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Seguro com ID {id} não encontrado."));
            }

            if (!await _clienteRepo.ExisteAsync(dto.IdCliente))
            {
                return BadRequest(ApiResponse<string>.Fail($"Cliente com ID {dto.IdCliente} não existe no sistema."));
            }

            if (!await _automovelRepo.ExisteAsync(dto.IdAutomovel))
            {
                return BadRequest(ApiResponse<string>.Fail($"Automóvel com ID {dto.IdAutomovel} não existe no sistema."));
            }

            existente.NumeroApolice = dto.NumeroApolice.Trim();
            existente.ValorCobertura = dto.ValorCobertura;
            existente.IdCliente = dto.IdCliente;
            existente.IdAutomovel = dto.IdAutomovel;

            await _seguroRepo.AtualizarAsync(existente);
            var atualizado = await _seguroRepo.ObterPorIdAsync(id);
            return Ok(ApiResponse<Seguro>.Ok(atualizado ?? existente, "Seguro atualizado com sucesso."));
        }

        /// <summary>
        /// Remove um seguro do sistema.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(int id)
        {
            var existe = await _seguroRepo.ExisteAsync(id);
            if (!existe)
            {
                return NotFound(ApiResponse<string>.Fail($"Seguro com ID {id} não encontrado."));
            }

            await _seguroRepo.ExcluirAsync(id);
            return Ok(ApiResponse<bool>.Ok(true, "Seguro excluído com sucesso."));
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
