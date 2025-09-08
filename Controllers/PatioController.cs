using Microsoft.AspNetCore.Mvc;
using Sprint.Dtos;
using Sprint.Models;
using Sprint.Services;
using System.Collections.Generic;

namespace Sprint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatioController : ControllerBase
    {
        private readonly IPatioService _patioService;

        public PatioController(IPatioService patioService)
        {
            _patioService = patioService;
        }

        /// <summary>
        /// Obtém uma lista de todos os pátios cadastrados.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/Patio
        ///
        /// </remarks>
        /// <returns>Uma lista de pátios.</returns>
        /// <response code="200">Retorna a lista completa de pátios.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Patio>))]
        public IActionResult GetAll()
        {
            var patios = _patioService.GetAll();
            return Ok(patios);
        }

        /// <summary>
        /// Retorna um pátio específico por ID.
        /// </summary>
        /// <param name="id">ID do pátio.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/Patio/1
        ///
        /// </remarks>
        /// <returns>O pátio encontrado ou NotFound.</returns>
        /// <response code="200">Retorna o pátio.</response>
        /// <response code="404">Pátio não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Patio))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(long id)
        {
            var patio = _patioService.GetById(id);
            if (patio == null) return NotFound();
            return Ok(patio);
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        /// <param name="patioDto">Objeto do pátio a ser criado.</param>
        /// <remarks>
        /// O ID do pátio é gerado automaticamente.
        /// Exemplo de requisição:
        ///
        ///     POST /api/Patio
        ///     {
        ///         "nome": "Pátio Central",
        ///         "endereco": "Rua das Flores, 123"
        ///     }
        /// </remarks>
        /// <returns>O pátio recém-criado, incluindo o ID.</returns>
        /// <response code="201">Retorna o pátio recém-criado.</response>
        /// <response code="400">Se o pátio for nulo ou inválido.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Patio))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] PatioDTO patioDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (patio, error) = _patioService.Create(patioDto);
            if (patio == null)
                return BadRequest(new { message = error });

            return CreatedAtAction(nameof(GetById), new { id = patio.Id }, patio);
        }

        /// <summary>
        /// Atualiza um pátio existente.
        /// </summary>
        /// <param name="id">ID do pátio a ser atualizado.</param>
        /// <param name="patioDto">Objeto pátio com os dados atualizados.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     PUT /api/Patio/1
        ///     {
        ///         "id": 1,
        ///         "nome": "Pátio Central Atualizado",
        ///         "endereco": "Rua das Palmeiras, 456"
        ///     }
        /// </remarks>
        /// <returns>O pátio atualizado.</returns>
        /// <response code="200">Retorna o pátio atualizado.</response>
        /// <response code="400">Se o corpo da requisição for inválido ou IDs não coincidirem.</response>
        /// <response code="404">Pátio não encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Patio))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(long id, [FromBody] PatioDTO patioDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (patio, error) = _patioService.Update(id, patioDto);
            if (error == "ID do corpo não corresponde ao da URL")
                return BadRequest();
            if (error == "Pátio não encontrado")
                return NotFound();

            return Ok(patio);
        }

        /// <summary>
        /// Remove um pátio pelo ID.
        /// </summary>
        /// <param name="id">ID do pátio a ser removido.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     DELETE /api/Patio/1
        ///
        /// </remarks>
        /// <response code="204">Pátio removido com sucesso.</response>
        /// <response code="404">Pátio não encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(long id)
        {
            var deleted = _patioService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
