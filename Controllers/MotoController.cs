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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var motos = await _context.Motos
                .Include(m => m.Cliente)
                .Include(m => m.Patio)
                .ToListAsync();
            return Ok(motos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var moto = await _context.Motos
                .Include(m => m.Cliente)
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (moto == null) return NotFound();
            return Ok(moto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MotoDTO motoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Verifica se existe moto com o mesmo número de chassi
            var exists = await _context.Motos
                .Where(m => m.NumeroChassi == motoDto.NumeroChassi)
                .FirstOrDefaultAsync() != null;

            if (exists)
            {
                return Conflict(new { message = "ja existe uma moto com esse numero de chassi" });
            }

            // Verifica se o PatioId existe no banco
            var patioExists = await _context.Patios
                .Where(p => p.Id == motoDto.PatioId)
                .FirstOrDefaultAsync() != null;

            if (!patioExists)
            {
                return BadRequest(new { message = "patio id invalido" });
            }

            // Verifica se o ClienteId existe no banco
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] MotoDTO motoDto)
        {
            if (id != motoDto.Id) return BadRequest();

            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();

            // Atualiza apenas os campos fornecidos
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

        [HttpDelete("{id}")]
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
