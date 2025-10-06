using Microsoft.EntityFrameworkCore;
using BoleriaAPI.Domain.Entity;
using BoleriaAPI.Infrastructure.Mapping;

namespace BoleriaAPI.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Bolo> Bolos => Set<Bolo>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BoloMapping());
            modelBuilder.ApplyConfiguration(new PedidoMapping());
        }
    }
}
