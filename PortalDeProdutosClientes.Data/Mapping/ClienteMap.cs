using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalDeProdutosClientes.Dominain.Entidades;

namespace PortalDeProdutosClientes.Data.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            // Nome da tabela no banco de dados
            builder.ToTable("tb_Cliente");

            // Chave Primária
            builder.HasKey(c => c.Id);

            // Configuração das propriedades (colunas)
            builder.Property(c => c.NomeCliente)
                .IsRequired() // NOT NULL
                .HasMaxLength(150);

            builder.Property(c => c.EmailCliente)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Cpf)
                .IsRequired()
                .HasMaxLength(14); // Ex: 000.000.000-00

            builder.Property(c => c.Sexo)
                .HasMaxLength(20);

            // Relacionamento: Um Cliente tem MUITOS Produtos (1 para N)
            builder.HasMany(c => c.Produtos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); // Se apagar o cliente, apaga os produtos (ou Restrict, se preferir bloquear)
        }
    }
}