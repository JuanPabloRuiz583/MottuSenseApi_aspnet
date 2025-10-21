using Microsoft.AspNetCore.Mvc;

namespace Sprint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Endpoint customizado de health check.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/health
        ///
        /// <br/>
        /// <b>Veja a interface gráfica dos health checks:</b>
        /// <a href="/health-ui" target="_blank">/health-ui</a>
        /// </remarks>
        /// <returns>Status da API.</returns>
        /// <response code="200">API está saudável.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
