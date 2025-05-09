using Microsoft.EntityFrameworkCore;
using Sprint.Models;

namespace Sprint.Data
{
    public class AppDbContext : DbContext
    {
        // Construtor para passar a configuração de opções para o DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet de cada modelo
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Moto> Motos { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<SensorLocalizacao> Sensores { get; set; }

        // Configuração dos relacionamentos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cliente - Moto (1:N)
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Motos)
                .WithOne(m => m.Cliente)
                .HasForeignKey(m => m.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); // Apaga motos se cliente for apagado

            // Patio - Moto (1:N)
            modelBuilder.Entity<Patio>()
                .HasMany(p => p.Motos)
                .WithOne(m => m.Patio)
                .HasForeignKey(m => m.PatioId)
                .OnDelete(DeleteBehavior.Restrict); // Evita apagar motos junto com pátio

            // Moto - SensorLocalizacao (1:N)
            modelBuilder.Entity<Moto>()
                .HasMany<SensorLocalizacao>()
                .WithOne(s => s.Moto)
                .HasForeignKey(s => s.MotoId)
                .OnDelete(DeleteBehavior.Cascade); // Apaga sensores se moto for apagada
        }
    }
}
