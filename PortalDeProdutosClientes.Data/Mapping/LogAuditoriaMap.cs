using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalDeProdutosClientes.Domain.Entidades;

namespace PortalDeProdutosClientes.Data.Mappings
{
    public class LogAuditoriaMap : IEntityTypeConfiguration<LogAuditoria>
    {
        public void Configure(EntityTypeBuilder<LogAuditoria> builder)
        {
            // Nome da tabela no banco de dados
            builder.ToTable("LogsAuditoria");

            // Chave Primária
            builder.HasKey(l => l.Id);

            // Configuração das propriedades
            builder.Property(l => l.Acao)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(l => l.UsuarioId)
                .HasMaxLength(450); // Padrão recomendado para IDs do ASP.NET Core Identity

            builder.Property(l => l.UsuarioEmail)
                .HasMaxLength(256);

            builder.Property(l => l.EntidadeAfetada)
                .HasMaxLength(100);

            builder.Property(l => l.RegistroId);

            // NVARCHAR(MAX) garante espaço suficiente para salvar os dados em JSON (antes/depois)
            builder.Property(l => l.Detalhes)
                .HasColumnType("nvarchar(max)");

            builder.Property(l => l.DataOcorrencia)
                .IsRequired();
        }
    }
}