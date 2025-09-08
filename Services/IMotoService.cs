using Sprint.Dtos;
using Sprint.Models;
using System.Collections.Generic;

namespace Sprint.Services
{
    public interface IMotoService
    {
        IEnumerable<Moto> GetAll();
        Moto GetById(long id);
        (Moto moto, string error) Create(MotoDTO motoDto);
        (Moto moto, string error) Update(long id, MotoDTO motoDto);
        bool Delete(long id);
    }
}
