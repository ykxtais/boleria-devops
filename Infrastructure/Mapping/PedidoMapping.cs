using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoleriaAPI.Domain.Entity;

namespace BoleriaAPI.Infrastructure.Mapping
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.NomeCliente).HasMaxLength(100);

            builder.HasOne(p => p.Bolo)
                   .WithMany(b => b.Pedidos)
                   .HasForeignKey(p => p.BoloId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
