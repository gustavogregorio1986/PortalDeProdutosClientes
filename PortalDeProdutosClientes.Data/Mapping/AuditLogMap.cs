using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalDeProdutosClientes.Domain.Entities;

namespace PortalDeProdutosClientes.Infrastructure.Data.Mappings
{
    public class AuditLogMap : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Tabela)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Acao)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Usuario)
                .HasMaxLength(150);

            builder.Property(x => x.DataHora)
                .IsRequired();

            builder.Property(x => x.ValoresNovos)
                .HasColumnType("TEXT"); // Ou VARCHAR(MAX) dependendo do seu SQL Server/PostgreSQL
        }
    }
}