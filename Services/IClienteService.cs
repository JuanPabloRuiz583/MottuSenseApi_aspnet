using Sprint.Dtos;
using Sprint.Models;
using System.Collections.Generic;

namespace Sprint.Services
{
    public interface IClienteService
    {
        IEnumerable<Cliente> GetAll();
        Cliente GetById(long id);
        (Cliente cliente, string error) Create(ClienteDTO clienteDto);
        (Cliente cliente, string error) Update(long id, ClienteDTO clienteDto);
        bool Delete(long id);
    }
}