using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalDeProdutosClientes.Dominain.Entidades;

namespace PortalDeProdutosClientes.Data.Mappings
{
    public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            // Nome da tabela no banco de dados
            builder.ToTable("Pagamentos");

            // Chave Primária
            builder.HasKey(p => p.Id);

            // Configuração dos campos decimais (precisão monetária)
            builder.Property(p => p.Dinheiro)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Troco)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");

            // Configuração das propriedades de texto e relacionamentos
            builder.Property(p => p.Cartao)
                .HasMaxLength(50);

            builder.Property(p => p.Parcela);

            builder.Property(p => p.StatusCartao)
                .IsRequired();

            // Relacionamento: Muitos Pagamentos pertencem a UM Produto (N para 1)
            builder.HasOne(p => p.Produto)
                .WithMany(prod => prod.Pagamentos)
                .HasForeignKey(p => p.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade); // Se o produto for apagado, os pagamentos vinculados também são
        }
    }
}