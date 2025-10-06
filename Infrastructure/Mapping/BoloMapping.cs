using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoleriaAPI.Domain.Entity;

namespace BoleriaAPI.Infrastructure.Mapping
{
    public class BoloMapping : IEntityTypeConfiguration<Bolo>
    {
        public void Configure(EntityTypeBuilder<Bolo> builder)
        {
            builder.ToTable("Bolo");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Nome).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Sabor).HasMaxLength(50).IsRequired();
            builder.Property(b => b.Preco).HasColumnType("decimal(10,2)").IsRequired();

            builder.HasMany(b => b.Pedidos)
                   .WithOne(p => p.Bolo!)
                   .HasForeignKey(p => p.BoloId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
