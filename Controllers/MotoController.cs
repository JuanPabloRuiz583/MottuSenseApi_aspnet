using Microsoft.AspNetCore.Mvc;
using Sprint.Dtos;
using Sprint.Models;
using Sprint.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Sprint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotoController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        /// <summary>
        /// Obtém uma lista de todas as motos cadastradas.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/Moto
        ///
        /// </remarks>
        /// <returns>Uma lista de motos.</returns>
        /// <response code="200">Retorna a lista completa de motos.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Moto>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var motos = _motoService.GetAll();
            return Ok(motos);
        }

        /// <summary>
        /// Retorna uma moto específica por ID.
        /// </summary>
        /// <param name="id">ID da moto.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/Moto/1
        ///
        /// </remarks>
        /// <returns>A moto encontrada ou NotFound.</returns>
        /// <response code="200">Retorna a moto.</response>
        /// <response code="404">Moto não encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(long id)
        {
            var moto = _motoService.GetById(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="motoDto">Objeto da moto a ser criada.</param>
        /// <remarks>
        /// O ID da moto é gerado automaticamente.
        /// Exemplo de requisição:
        ///
        ///     POST /api/Moto
        ///     {
        ///         "placa": "ABC1234",
        ///         "modelo": "Honda CG 160",
        ///         "numeroChassi": "9C2KC1670GR123456",
        ///         "status": "DISPONIVEL",
        ///         "patioId": 1,
        ///         "clienteId": 1
        ///     }
        /// </remarks>
        /// <returns>A moto recém-criada, incluindo o ID.</returns>
        /// <response code="201">Retorna a moto recém-criada.</response>
        /// <response code="400">Se a moto for nula, inválida ou IDs de cliente/pátio não existirem.</response>
        /// <response code="409">Se já existir uma moto com o mesmo número de chassi.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Create([FromBody] MotoDTO motoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (moto, error) = _motoService.Create(motoDto);
            if (error == "ja existe uma moto com esse numero de chassi")
                return Conflict(new { message = error });
            if (error == "patio id invalido" || error == "cliente id invalido")
                return BadRequest(new { message = error });
            if (moto == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = moto.Id }, moto);
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        /// <param name="id">ID da moto a ser atualizada.</param>
        /// <param name="motoDto">Objeto moto com os dados atualizados.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     PUT /api/Moto/1
        ///     {
        ///         "id": 1,
        ///         "placa": "DEF5678",
        ///         "modelo": "Yamaha Fazer 250",
        ///         "numeroChassi": "9C2KC1670GR654321",
        ///         "status": "EM_MANUTENCAO",
        ///         "patioId": 2,
        ///         "clienteId": 2
        ///     }
        /// </remarks>
        /// <returns>A moto atualizada.</returns>
        /// <response code="200">Retorna a moto atualizada.</response>
        /// <response code="400">Se o corpo da requisição for inválido ou IDs não coincidirem.</response>
        /// <response code="404">Moto não encontrada.</response>
        /// <response code="409">Se já existir uma moto com o mesmo número de chassi.</response>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(long id, [FromBody] MotoDTO motoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (moto, error) = _motoService.Update(id, motoDto);

            if (error == "ID do corpo não corresponde ao da URL")
                return BadRequest(new { message = error });

            if (error == "Moto não encontrada")
                return NotFound(new { message = error });

            if (error == "patio id invalido" || error == "cliente id invalido")
                return BadRequest(new { message = error });

            if (error == "já existe uma moto com esse número de chassi")
                return Conflict(new { message = error });

            return Ok(moto);
        }

        /// <summary>
        /// Remove uma moto pelo ID.
        /// </summary>
        /// <param name="id">ID da moto a ser removida.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     DELETE /api/Moto/1
        ///
        /// </remarks>
        /// <response code="204">Moto removida com sucesso.</response>
        /// <response code="404">Moto não encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(long id)
        {
            var deleted = _motoService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}