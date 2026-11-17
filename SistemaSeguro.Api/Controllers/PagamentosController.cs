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
    public class PagamentosController : ControllerBase
    {
        private readonly IPagamentoRepository _pagamentoRepo;
        private readonly IClienteRepository _clienteRepo;
        private readonly IAcidenteRepository _acidenteRepo;

        public PagamentosController(IPagamentoRepository pagamentoRepo, IClienteRepository clienteRepo, IAcidenteRepository acidenteRepo)
        {
            _pagamentoRepo = pagamentoRepo;
            _clienteRepo = clienteRepo;
            _acidenteRepo = acidenteRepo;
        }

        /// <summary>
        /// Lista todos os pagamentos de sinistros realizados, opcionalmente filtrando por ID do cliente.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<PagamentoSeguro>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery] int? idCliente)
        {
            var pagamentos = await _pagamentoRepo.ListarTodosAsync(idCliente);
            return Ok(ApiResponse<List<PagamentoSeguro>>.Ok(pagamentos, $"{pagamentos.Count} pagamento(s) retornado(s)."));
        }

        /// <summary>
        /// Obtém os detalhes de um pagamento por ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<PagamentoSeguro>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var pgto = await _pagamentoRepo.ObterPorIdAsync(id);
            if (pgto == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Pagamento com ID {id} não encontrado."));
            }
            return Ok(ApiResponse<PagamentoSeguro>.Ok(pgto));
        }

        /// <summary>
        /// Registra um novo pagamento de sinistro/indenização.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PagamentoSeguro>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar([FromBody] PagamentoCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            if (!await _clienteRepo.ExisteAsync(dto.IdCliente))
            {
                return BadRequest(ApiResponse<string>.Fail($"Cliente com ID {dto.IdCliente} não existe no sistema."));
            }

            if (!await _acidenteRepo.ExisteAsync(dto.IdAcidente))
            {
                return BadRequest(ApiResponse<string>.Fail($"Acidente/Sinistro com ID {dto.IdAcidente} não existe no sistema."));
            }

            var pgto = new PagamentoSeguro
            {
                Valor = dto.Valor,
                DataPagamento = dto.DataPagamento,
                IdCliente = dto.IdCliente,
                IdAcidente = dto.IdAcidente
            };

            await _pagamentoRepo.InserirAsync(pgto);
            var criado = await _pagamentoRepo.ObterPorIdAsync(pgto.IdPagamento);
            return CreatedAtAction(nameof(ObterPorId), new { id = pgto.IdPagamento }, ApiResponse<PagamentoSeguro>.Ok(criado ?? pgto, "Pagamento registrado com sucesso."));
        }

        /// <summary>
        /// Atualiza os dados de um pagamento existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<PagamentoSeguro>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] PagamentoCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos.", ObterErrosModelState()));
            }

            var existente = await _pagamentoRepo.ObterPorIdAsync(id);
            if (existente == null)
            {
                return NotFound(ApiResponse<string>.Fail($"Pagamento com ID {id} não encontrado."));
            }

            if (!await _clienteRepo.ExisteAsync(dto.IdCliente))
            {
                return BadRequest(ApiResponse<string>.Fail($"Cliente com ID {dto.IdCliente} não existe no sistema."));
            }

            if (!await _acidenteRepo.ExisteAsync(dto.IdAcidente))
            {
                return BadRequest(ApiResponse<string>.Fail($"Acidente com ID {dto.IdAcidente} não existe no sistema."));
            }

            existente.Valor = dto.Valor;
            existente.DataPagamento = dto.DataPagamento;
            existente.IdCliente = dto.IdCliente;
            existente.IdAcidente = dto.IdAcidente;

            await _pagamentoRepo.AtualizarAsync(existente);
            var atualizado = await _pagamentoRepo.ObterPorIdAsync(id);
            return Ok(ApiResponse<PagamentoSeguro>.Ok(atualizado ?? existente, "Pagamento atualizado com sucesso."));
        }

        /// <summary>
        /// Remove um pagamento do sistema.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(int id)
        {
            var existe = await _pagamentoRepo.ExisteAsync(id);
            if (!existe)
            {
                return NotFound(ApiResponse<string>.Fail($"Pagamento com ID {id} não encontrado."));
            }

            await _pagamentoRepo.ExcluirAsync(id);
            return Ok(ApiResponse<bool>.Ok(true, "Pagamento excluído com sucesso."));
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
