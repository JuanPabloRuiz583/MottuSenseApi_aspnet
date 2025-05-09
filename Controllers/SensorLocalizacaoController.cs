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
    public class SensorLocalizacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SensorLocalizacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var sensores = _context.Sensores.ToList();
            return Ok(sensores);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var sensor = _context.Sensores.Find(id);
            if (sensor == null) return NotFound();
            return Ok(sensor);
        }

        [HttpPost]
        public IActionResult Create([FromBody] SensorLocalizacaoDTO sensorDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Verifica se o MotoId existe no banco
            var moto = _context.Motos.FirstOrDefault(m => m.Id == sensorDto.MotoId);
            if (moto == null)
            {
                return BadRequest(new { message = "id invalido. o id da moto nao existe" });
            }

            var sensor = new SensorLocalizacao
            {
                Latitude = sensorDto.Latitude,
                Longitude = sensorDto.Longitude,
                TimeDaLocalizacao = sensorDto.TimeDaLocalizacao,
                MotoId = sensorDto.MotoId
            };

            _context.Sensores.Add(sensor);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = sensor.Id }, sensor);
        
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] SensorLocalizacaoDTO sensorDto)
        {
            if (id != sensorDto.Id) return BadRequest();

            var sensor = _context.Sensores.Find(id);
            if (sensor == null) return NotFound();

            sensor.Latitude = sensorDto.Latitude;
            sensor.Longitude = sensorDto.Longitude;
            sensor.TimeDaLocalizacao = sensorDto.TimeDaLocalizacao;

            _context.Sensores.Update(sensor);
            _context.SaveChanges();
            return Ok(sensor);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var sensor = _context.Sensores.Find(id);
            if (sensor == null) return NotFound();

            _context.Sensores.Remove(sensor);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
