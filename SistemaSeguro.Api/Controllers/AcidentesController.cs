using System;
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
    public class AcidentesController : ControllerBase
    {
        private readonly IAcidenteRepository _acidenteRepo;
        private readonly IAutomovelRepository _automovelRepo;

        public AcidentesController(IAcidenteRepository acidenteRepo, IAutomovelRepository automovelRepo)
        {
            _acidenteRepo = acidenteRepo;
            _automovelRepo = automovelRepo;
        }

        /// <summary>
        /// Lista todos os acidentes/sinistros registrados, opcionalmente filtrando por ID do automóvel.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<Acidente>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery] int? idAutomovel)
        {
            var acidentes = await _acidenteRepo.ListarTodosAsync(idAutomovel);
            return Ok(ApiResponse<List<Acidente>>.Ok(acidentes, $"{acidentes.Count} acidente(s) retornado(s)."));
        }

        /// <summary>
        /// Obtém os detalhes de um acidente por ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Acidente>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var acidente = await _acidenteRepo.ObterPorIdAsync(id);
            if (acidente == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Acidente com ID {id} não encontrado."));
            }
            return Ok(ApiResponse<Acidente>.Ok(acidente));
        }

        /// <summary>
        /// Registra um novo acidente associado a um automóvel.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Acidente>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar([FromBody] AcidenteCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            if (!TimeSpan.TryParse(dto.HoraAcidente, out var horaParsed))
            {
                return BadRequest(ApiResponse<string>.Fail("Formato de hora inválido. Utilize HH:mm:ss."));
            }

            if (!await _automovelRepo.ExisteAsync(dto.IdAutomovel))
            {
                return BadRequest(ApiResponse<string>.Fail($"Automóvel com ID {dto.IdAutomovel} não existe no sistema."));
            }

            var acidente = new Acidente
            {
                DataAcidente = dto.DataAcidente,
                HoraAcidente = horaParsed,
                EnderecoAcidente = dto.EnderecoAcidente.Trim(),
                MotivoAcidente = dto.MotivoAcidente?.Trim(),
                AutomoveisEnvolvidos = dto.AutomoveisEnvolvidos?.Trim(),
                DanosTerceiros = dto.DanosTerceiros?.Trim(),
                IdAutomovel = dto.IdAutomovel
            };

            await _acidenteRepo.InserirAsync(acidente);
            var criado = await _acidenteRepo.ObterPorIdAsync(acidente.IdAcidente);
            return CreatedAtAction(nameof(ObterPorId), new { id = acidente.IdAcidente }, ApiResponse<Acidente>.Ok(criado ?? acidente, "Acidente registrado com sucesso."));
        }

        /// <summary>
        /// Atualiza os dados de um sinistro/acidente existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Acidente>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AcidenteCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            var existente = await _acidenteRepo.ObterPorIdAsync(id);
            if (existente == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Acidente com ID {id} não encontrado."));
            }

            if (!TimeSpan.TryParse(dto.HoraAcidente, out var horaParsed))
            {
                return BadRequest(ApiResponse<string>.Fail("Formato de hora inválido. Utilize HH:mm:ss."));
            }

            if (!await _automovelRepo.ExisteAsync(dto.IdAutomovel))
            {
                return BadRequest(ApiResponse<string>.Fail($"Automóvel com ID {dto.IdAutomovel} não existe no sistema."));
            }

            existente.DataAcidente = dto.DataAcidente;
            existente.HoraAcidente = horaParsed;
            existente.EnderecoAcidente = dto.EnderecoAcidente.Trim();
            existente.MotivoAcidente = dto.MotivoAcidente?.Trim();
            existente.AutomoveisEnvolvidos = dto.AutomoveisEnvolvidos?.Trim();
            existente.DanosTerceiros = dto.DanosTerceiros?.Trim();
            existente.IdAutomovel = dto.IdAutomovel;

            await _acidenteRepo.AtualizarAsync(existente);
            var atualizado = await _acidenteRepo.ObterPorIdAsync(id);
            return Ok(ApiResponse<Acidente>.Ok(atualizado ?? existente, "Acidente atualizado com sucesso."));
        }

        /// <summary>
        /// Remove o registro de um acidente do sistema.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(int id)
        {
            var existe = await _acidenteRepo.ExisteAsync(id);
            if (!existe)
            {
                return NotFound(ApiResponse<string>.Fail($"Acidente com ID {id} não encontrado."));
            }

            await _acidenteRepo.ExcluirAsync(id);
            return Ok(ApiResponse<bool>.Ok(true, "Acidente excluído com sucesso."));
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
