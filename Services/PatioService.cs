using Sprint.Data;
using Sprint.Dtos;
using Sprint.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sprint.Services
{
    public class PatioService : IPatioService
    {
        private readonly AppDbContext _context;

        public PatioService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Patio> GetAll()
        {
            return _context.Patios.ToList();
        }

        public Patio GetById(long id)
        {
            return _context.Patios.Find(id);
        }

        public (Patio patio, string error) Create(PatioDTO patioDto)
        {
            if (string.IsNullOrWhiteSpace(patioDto.Nome))
                return (null, "Nome é obrigatório");

            var patio = new Patio
            {
                Nome = patioDto.Nome,
                Endereco = patioDto.Endereco
            };

            _context.Patios.Add(patio);
            _context.SaveChanges();
            return (patio, null);
        }

        public (Patio patio, string error) Update(long id, PatioDTO patioDto)
        {
            if (id != patioDto.Id)
                return (null, "ID do corpo não corresponde ao da URL");

            var patio = _context.Patios.Find(id);
            if (patio == null)
                return (null, "Pátio não encontrado");

            patio.Nome = patioDto.Nome ?? patio.Nome;
            patio.Endereco = patioDto.Endereco ?? patio.Endereco;

            _context.Patios.Update(patio);
            _context.SaveChanges();
            return (patio, null);
        }

        public bool Delete(long id)
        {
            var patio = _context.Patios.Find(id);
            if (patio == null)
                return false;

            _context.Patios.Remove(patio);
            _context.SaveChanges();
            return true;
        }
    }
}
