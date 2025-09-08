using Sprint.Data;
using Sprint.Dtos;
using Sprint.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sprint.Services
{
    public class MotoService : IMotoService
    {
        private readonly AppDbContext _context;

        public MotoService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Moto> GetAll()
        {
            return _context.Motos
                .Include(m => m.Cliente)
                .Include(m => m.Patio)
                .ToList();
        }

        public Moto GetById(long id)
        {
            return _context.Motos
                .Include(m => m.Cliente)
                .Include(m => m.Patio)
                .FirstOrDefault(m => m.Id == id);
        }

        public (Moto moto, string error) Create(MotoDTO motoDto)
        {
            if (string.IsNullOrWhiteSpace(motoDto.NumeroChassi))
                return (null, "Número do chassi é obrigatório");

            var motoExistente = _context.Motos
                .FirstOrDefault(m => m.NumeroChassi == motoDto.NumeroChassi);

            if (motoExistente != null)
                return (null, "ja existe uma moto com esse numero de chassi");

            var patio = _context.Patios.FirstOrDefault(p => p.Id == motoDto.PatioId);
            if (patio == null)
                return (null, "patio id invalido");

            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == motoDto.ClienteId);
            if (cliente == null)
                return (null, "cliente id invalido");

            var moto = new Moto
            {
                Placa = motoDto.Placa,
                Modelo = motoDto.Modelo,
                NumeroChassi = motoDto.NumeroChassi,
                Status = motoDto.Status,
                PatioId = motoDto.PatioId.Value,
                ClienteId = motoDto.ClienteId.Value
            };

            _context.Motos.Add(moto);
            _context.SaveChanges();
            return (moto, null);
        }
        public (Moto moto, string error) Update(long id, MotoDTO motoDto)
        {
            if (id != motoDto.Id)
                return (null, "ID do corpo não corresponde ao da URL");

            var moto = _context.Motos.Find(id);
            if (moto == null)
                return (null, "Moto não encontrada");

            // Atualiza apenas se vier valor novo
            if (!string.IsNullOrWhiteSpace(motoDto.Placa))
                moto.Placa = motoDto.Placa;

            if (!string.IsNullOrWhiteSpace(motoDto.Modelo))
                moto.Modelo = motoDto.Modelo;

            if (!string.IsNullOrWhiteSpace(motoDto.NumeroChassi))
                moto.NumeroChassi = motoDto.NumeroChassi;

            moto.Status = motoDto.Status;

            if (motoDto.PatioId.HasValue)
            {
                var patio = _context.Patios.Find(motoDto.PatioId.Value);
                if (patio == null)
                    return (null, "patio id invalido");
                moto.PatioId = motoDto.PatioId.Value;
            }

            if (motoDto.ClienteId.HasValue)
            {
                var cliente = _context.Clientes.Find(motoDto.ClienteId.Value);
                if (cliente == null)
                    return (null, "cliente id invalido");
                moto.ClienteId = motoDto.ClienteId.Value;
            }

            _context.Motos.Update(moto);
            _context.SaveChanges();
            return (moto, null);
        }



        public bool Delete(long id)
        {
            var moto = _context.Motos.Find(id);
            if (moto == null)
                return false;

            _context.Motos.Remove(moto);
            _context.SaveChanges();
            return true;
        }
    }
}
