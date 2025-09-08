using Sprint.Dtos;
using Sprint.Models;
using System.Collections.Generic;

namespace Sprint.Services
{
    public interface IPatioService
    {
        IEnumerable<Patio> GetAll();
        Patio GetById(long id);
        (Patio patio, string error) Create(PatioDTO patioDto);
        (Patio patio, string error) Update(long id, PatioDTO patioDto);
        bool Delete(long id);
    }
}
