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
    [Authorize]
    public class SensorLocalizacaoController : ControllerBase
    {
        private readonly ISensorLocalizacaoService _sensorService;

        public SensorLocalizacaoController(ISensorLocalizacaoService sensorService)
        {
            _sensorService = sensorService;
        }

        /// <summary>
        /// Obtém uma lista de todos os sensores de localização cadastrados.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/SensorLocalizacao
        ///
        /// </remarks>
        /// <returns>Uma lista de sensores de localização.</returns>
        /// <response code="200">Retorna a lista completa de sensores de localização.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SensorLocalizacao>))]
        public IActionResult GetAll()
        {
            var sensores = _sensorService.GetAll();
            return Ok(sensores);
        }

        /// <summary>
        /// Retorna um sensor de localização específico por ID.
        /// </summary>
        /// <param name="id">ID do sensor de localização.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/SensorLocalizacao/1
        ///
        /// </remarks>
        /// <returns>O sensor de localização encontrado ou NotFound.</returns>
        /// <response code="200">Retorna o sensor de localização.</response>
        /// <response code="404">Sensor de localização não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorLocalizacao))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(long id)
        {
            var sensor = _sensorService.GetById(id);
            if (sensor == null) return NotFound();
            return Ok(sensor);
        }

        /// <summary>
        /// Cria um novo sensor de localização.
        /// </summary>
        /// <param name="sensorDto">Objeto do sensor de localização a ser criado.</param>
        /// <remarks>
        /// O ID do sensor é gerado automaticamente.
        /// Exemplo de requisição:
        ///
        ///     POST /api/SensorLocalizacao
        ///     {
        ///         "latitude": -23.55052,
        ///         "longitude": -46.633308,
        ///         "timeDaLocalizacao": "2024-09-05T14:30:00",
        ///         "motoId": 1
        ///     }
        /// </remarks>
        /// <returns>O sensor de localização recém-criado, incluindo o ID.</returns>
        /// <response code="201">Retorna o sensor recém-criado.</response>
        /// <response code="400">Se o sensor for nulo, inválido ou o MotoId não existir.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SensorLocalizacao))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] SensorLocalizacaoDTO sensorDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (sensor, error) = _sensorService.Create(sensorDto);
            if (sensor == null)
                return BadRequest(new { message = error });

            return CreatedAtAction(nameof(GetById), new { id = sensor.Id }, sensor);
        }

        /// <summary>
        /// Atualiza um sensor de localização existente.
        /// </summary>
        /// <param name="id">ID do sensor de localização a ser atualizado.</param>
        /// <param name="sensorDto">Objeto sensor de localização com os dados atualizados.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     PUT /api/SensorLocalizacao/1
        ///     {
        ///         "id": 1,
        ///         "latitude": -23.55052,
        ///         "longitude": -46.633308,
        ///         "timeDaLocalizacao": "2024-09-05T15:00:00",
        ///         "motoId": 1
        ///     }
        /// </remarks>
        /// <returns>O sensor de localização atualizado.</returns>
        /// <response code="200">Retorna o sensor de localização atualizado.</response>
        /// <response code="400">Se o corpo da requisição for inválido ou IDs não coincidirem.</response>
        /// <response code="404">Sensor de localização não encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorLocalizacao))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(long id, [FromBody] SensorLocalizacaoDTO sensorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (sensor, error) = _sensorService.Update(id, sensorDto);

            if (error == "ID do corpo não corresponde ao da URL")
                return BadRequest(new { message = error });

            if (error == "Sensor de localização não encontrado")
                return NotFound(new { message = error });

            if (error == "id invalido. o id da moto nao existe")
                return BadRequest(new { message = error });

            return Ok(sensor);
        }

        /// <summary>
        /// Remove um sensor de localização pelo ID.
        /// </summary>
        /// <param name="id">ID do sensor de localização a ser removido.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     DELETE /api/SensorLocalizacao/1
        ///
        /// </remarks>
        /// <response code="204">Sensor de localização removido com sucesso.</response>
        /// <response code="404">Sensor de localização não encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(long id)
        {
            var deleted = _sensorService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
