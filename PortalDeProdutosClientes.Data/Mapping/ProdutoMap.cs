using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalDeProdutosClientes.Dominain.Entidades;

namespace PortalDeProdutosClientes.Data.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            // Nome da tabela no banco de dados
            builder.ToTable("Produtos");

            // Chave Primária
            builder.HasKey(p => p.Id);

            // Configuração das propriedades
            builder.Property(p => p.NomeProduto)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.Quantidade)
                .IsRequired();

            // Configuração de precisão para campos decimais (Preço) no SQL Server/Relacionais
            builder.Property(p => p.Preco)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.UnidadesRestantes)
                .IsRequired();

            builder.Property(p => p.DataFabricacao)
                .IsRequired();

            // Relacionamento: Muitos Produtos pertencem a UM Cliente (N para 1)
            builder.HasOne(p => p.Cliente)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict); // Evita apagar o cliente em cascata sem querer

            // Relacionamento: Um Produto tem MUITOS Pagamentos (1 para N)
            builder.HasMany(p => p.Pagamentos)
                .WithOne(pg => pg.Produto)
                .HasForeignKey(pg => pg.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}