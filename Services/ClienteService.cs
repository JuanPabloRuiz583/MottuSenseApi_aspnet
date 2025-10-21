using Sprint.Data;
using Sprint.Dtos;
using Sprint.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sprint.Services
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cliente> GetAll()
        {
            return _context.Clientes.ToList();
        }

        public Cliente GetById(long id)
        {
            return _context.Clientes.Find(id);
        }

        public (Cliente cliente, string error) Create(ClienteDTO clienteDto)
        {
            if (string.IsNullOrWhiteSpace(clienteDto.Email))
                return (null, "Email é obrigatório");

            var clienteExistente = _context.Clientes
                .FirstOrDefault(c => c.Email != null && c.Email == clienteDto.Email);

            if (clienteExistente != null)
                return (null, "um cliente com esse email ja existe");

            var cliente = new Cliente
            {
                Nome = clienteDto.Nome,
                Email = clienteDto.Email,
                Senha = clienteDto.Senha
            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return (cliente, null);
        }

        public (Cliente cliente, string error) Update(long id, ClienteDTO clienteDto)
        {
            if (id != clienteDto.Id)
                return (null, "ID do corpo não corresponde ao da URL");

            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
                return (null, "Cliente não encontrado");

            cliente.Nome = clienteDto.Nome ?? cliente.Nome;
            cliente.Email = clienteDto.Email ?? cliente.Email;
            if (!string.IsNullOrEmpty(clienteDto.Senha))
                cliente.Senha = clienteDto.Senha;

            _context.Clientes.Update(cliente);
            _context.SaveChanges();
            return (cliente, null);
        }

        public bool Delete(long id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
            return true;
        }

        public Cliente Authenticate(string email, string senha)
        {
            return _context.Clientes.FirstOrDefault(c => c.Email == email && c.Senha == senha);
        }

    }
}