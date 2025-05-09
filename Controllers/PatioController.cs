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
    public class PatioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var patios = _context.Patios.ToList();
            return Ok(patios);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var patio = _context.Patios.Find(id);
            if (patio == null) return NotFound();
            return Ok(patio);
        }

        [HttpPost]
        public IActionResult Create([FromBody] PatioDTO patioDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var patio = new Patio
            {
                Nome = patioDto.Nome,
                Endereco = patioDto.Endereco
            };

            _context.Patios.Add(patio);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = patio.Id }, patio);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] PatioDTO patioDto)
        {
            if (id != patioDto.Id) return BadRequest();

            var patio = _context.Patios.Find(id);
            if (patio == null) return NotFound();

            patio.Nome = patioDto.Nome ?? patio.Nome;
            patio.Endereco = patioDto.Endereco ?? patio.Endereco;

            _context.Patios.Update(patio);
            _context.SaveChanges();
            return Ok(patio);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var patio = _context.Patios.Find(id);
            if (patio == null) return NotFound();

            _context.Patios.Remove(patio);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
