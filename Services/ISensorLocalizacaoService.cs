using Sprint.Dtos;
using Sprint.Models;
using System.Collections.Generic;

namespace Sprint.Services
{
    public interface ISensorLocalizacaoService
    {
        IEnumerable<SensorLocalizacao> GetAll();
        SensorLocalizacao GetById(long id);
        (SensorLocalizacao sensor, string error) Create(SensorLocalizacaoDTO sensorDto);
        (SensorLocalizacao sensor, string error) Update(long id, SensorLocalizacaoDTO sensorDto);
        bool Delete(long id);
    }
}
