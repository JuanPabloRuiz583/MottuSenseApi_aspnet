using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint.Models;
using Sprint.Data;
using System;
using Sprint.Dtos;

namespace Sprint.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotoController(AppDbContext context)
        {
            _context = context;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Moto>))]
        public async Task<IActionResult> GetAll()
        {
            var motos = await _context.Motos
                .Include(m => m.Cliente)
                .Include(m => m.Patio)
                .ToListAsync();
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Moto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var moto = await _context.Motos
                .Include(m => m.Cliente)
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);

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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Moto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] MotoDTO motoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exists = await _context.Motos
                .Where(m => m.NumeroChassi == motoDto.NumeroChassi)
                .FirstOrDefaultAsync() != null;

            if (exists)
            {
                return Conflict(new { message = "ja existe uma moto com esse numero de chassi" });
            }

            var patioExists = await _context.Patios
                .Where(p => p.Id == motoDto.PatioId)
                .FirstOrDefaultAsync() != null;

            if (!patioExists)
            {
                return BadRequest(new { message = "patio id invalido" });
            }

            var clienteExists = await _context.Clientes
                .Where(c => c.Id == motoDto.ClienteId)
                .FirstOrDefaultAsync() != null;

            if (!clienteExists)
            {
                return BadRequest(new { message = "cliente id invalido" });
            }

            var moto = new Moto
            {
                Placa = motoDto.Placa,
                Modelo = motoDto.Modelo,
                NumeroChassi = motoDto.NumeroChassi,
                Status = motoDto.Status,
                PatioId = motoDto.PatioId.Value,
                ClienteId = motoDto.ClienteId.Value
            };

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
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
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Moto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] MotoDTO motoDto)
        {
            if (id != motoDto.Id) return BadRequest();

            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();

            moto.Placa = motoDto.Placa ?? moto.Placa;
            moto.Modelo = motoDto.Modelo ?? moto.Modelo;
            moto.NumeroChassi = motoDto.NumeroChassi ?? moto.NumeroChassi;
            moto.Status = motoDto.Status;

            if (motoDto.PatioId.HasValue)
                moto.PatioId = motoDto.PatioId.Value;

            if (motoDto.ClienteId.HasValue)
                moto.ClienteId = motoDto.ClienteId.Value;

            _context.Entry(moto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(long id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
