using PortalDeProdutosClientes.Domain.Enums; // Ajustado para o namespace correto
using PortalDeProdutosClientes.Dominain.Enum; // Mantenha se estiver usando em outro lugar

namespace PortalDeProdutosClientes.Dominain.Entidades
{
    public class Pagamento
    {
        public int Id { get; set; }

        public decimal? Dinheiro { get; set; }

        public decimal? Troco { get; set; }

        public decimal? Preco { get; set; }

        public string? Cartao { get; set; }

        public int? Parcela { get; set; }

        public DateTime DataPagamento { get; set; } = DateTime.Now;

        public StatusCartao StatusCartao { get; set; }

        public StatusPagamento StatusPagamento { get; set; }

        public int ProdutoId { get; set; }

        public Produto? Produto { get; set; }
    } // <-- Fechamento correto da classe Pagamento
} // <-- Fechamento correto do namespace