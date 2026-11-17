using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SistemaSeguro.Api.Data;
using SistemaSeguro.Api.DTOs;

namespace SistemaSeguro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public HealthController(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Verifica a integridade da API e a conectividade com o banco MySQL.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> ChecarStatus()
        {
            try
            {
                await using var conn = _connectionFactory.CreateConnection();
                await conn.OpenAsync();

                await using var cmd = new MySqlCommand("SELECT 1;", conn);
                var ping = await cmd.ExecuteScalarAsync();

                return Ok(ApiResponse<object>.Ok(new
                {
                    status = "Healthy",
                    database = "Connected",
                    serverTime = DateTime.UtcNow,
                    version = "1.0.0"
                }, "Sistema e banco de dados operando normalmente."));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ApiResponse<object>.Fail("Falha na conectividade com o banco de dados.", new System.Collections.Generic.List<string> { ex.Message }));
            }
        }
    }
}
