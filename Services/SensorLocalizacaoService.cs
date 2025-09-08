using Sprint.Data;
using Sprint.Dtos;
using Sprint.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sprint.Services
{
    public class SensorLocalizacaoService : ISensorLocalizacaoService
    {
        private readonly AppDbContext _context;

        public SensorLocalizacaoService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SensorLocalizacao> GetAll()
        {
            return _context.Sensores.ToList();
        }

        public SensorLocalizacao GetById(long id)
        {
            return _context.Sensores.Find(id);
        }

        public (SensorLocalizacao sensor, string error) Create(SensorLocalizacaoDTO sensorDto)
        {
            var moto = _context.Motos.FirstOrDefault(m => m.Id == sensorDto.MotoId);
            if (moto == null)
                return (null, "id invalido. o id da moto nao existe");

            var sensor = new SensorLocalizacao
            {
                Latitude = sensorDto.Latitude,
                Longitude = sensorDto.Longitude,
                TimeDaLocalizacao = sensorDto.TimeDaLocalizacao,
                MotoId = sensorDto.MotoId
            };

            _context.Sensores.Add(sensor);
            _context.SaveChanges();
            return (sensor, null);
        }


        public (SensorLocalizacao sensor, string error) Update(long id, SensorLocalizacaoDTO sensorDto)
        {
            if (id != sensorDto.Id)
                return (null, "ID do corpo não corresponde ao da URL");

            var sensor = _context.Sensores.Find(id);
            if (sensor == null)
                return (null, "Sensor de localização não encontrado");

            sensor.Latitude = sensorDto.Latitude;
            sensor.Longitude = sensorDto.Longitude;
            sensor.TimeDaLocalizacao = sensorDto.TimeDaLocalizacao;

            _context.Sensores.Update(sensor);
            _context.SaveChanges();
            return (sensor, null);
        }

        public bool Delete(long id)
        {
            var sensor = _context.Sensores.Find(id);
            if (sensor == null)
                return false;

            _context.Sensores.Remove(sensor);
            _context.SaveChanges();
            return true;
        }
    }
}
