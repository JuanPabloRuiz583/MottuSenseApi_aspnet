using Microsoft.AspNetCore.Mvc;
using System;
using Sprint.Data;
using Sprint.Models;
using Sprint.Dtos;
namespace Sprint.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var clientes = _context.Clientes.ToList();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ClienteDTO clienteDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exists = _context.Clientes.Where(c => c.Email == clienteDto.Email).FirstOrDefault() != null;
            if (exists)
            {
                return Conflict(new { message = "um cliente com esse email ja existe" });
            }

            var cliente = new Cliente
            {
                Nome = clienteDto.Nome,
                Email = clienteDto.Email,
                Senha = clienteDto.Senha
            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ClienteDTO clienteDto)
        {
            if (id != clienteDto.Id) return BadRequest();

            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();

            cliente.Nome = clienteDto.Nome ?? cliente.Nome;
            cliente.Email = clienteDto.Email ?? cliente.Email;
            if (!string.IsNullOrEmpty(clienteDto.Senha))
                cliente.Senha = clienteDto.Senha;

            _context.Clientes.Update(cliente);
            _context.SaveChanges();
            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
