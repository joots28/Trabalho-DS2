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
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _repository;

        public ClientesController(IClienteRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lista todos os clientes cadastrados com suporte a filtro por nome, e-mail ou profissão.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<Cliente>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery] string? busca)
        {
            var clientes = await _repository.ListarTodosAsync(busca);
            return Ok(ApiResponse<List<Cliente>>.Ok(clientes, $"{clientes.Count} cliente(s) retornado(s)."));
        }

        /// <summary>
        /// Obtém os detalhes de um cliente por ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var cliente = await _repository.ObterPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Cliente com ID {id} não encontrado."));
            }
            return Ok(ApiResponse<Cliente>.Ok(cliente));
        }

        /// <summary>
        /// Cadastra um novo cliente.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar([FromBody] ClienteCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            var cliente = new Cliente
            {
                Nome = dto.Nome.Trim(),
                Endereco = dto.Endereco.Trim(),
                Telefone = dto.Telefone?.Trim(),
                Email = dto.Email?.Trim(),
                DataNascimento = dto.DataNascimento,
                Profissao = dto.Profissao?.Trim()
            };

            await _repository.InserirAsync(cliente);
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.IdCliente }, ApiResponse<Cliente>.Ok(cliente, "Cliente cadastrado com sucesso."));
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ClienteCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            var existente = await _repository.ObterPorIdAsync(id);
            if (existente == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Cliente com ID {id} não encontrado."));
            }

            existente.Nome = dto.Nome.Trim();
            existente.Endereco = dto.Endereco.Trim();
            existente.Telefone = dto.Telefone?.Trim();
            existente.Email = dto.Email?.Trim();
            existente.DataNascimento = dto.DataNascimento;
            existente.Profissao = dto.Profissao?.Trim();

            await _repository.AtualizarAsync(existente);
            return Ok(ApiResponse<Cliente>.Ok(existente, "Cliente atualizado com sucesso."));
        }

        /// <summary>
        /// Remove um cliente do sistema.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(int id)
        {
            var existente = await _repository.ExisteAsync(id);
            if (!existente)
            {
                return NotFound(ApiResponse<string>.Fail($"Cliente com ID {id} não encontrado."));
            }

            await _repository.ExcluirAsync(id);
            return Ok(ApiResponse<bool>.Ok(true, "Cliente excluído com sucesso."));
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
