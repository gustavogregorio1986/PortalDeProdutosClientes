using PortalDeProdutosClientes.Dominain.Enum;

namespace PortalDeProdutosClientes.DTOs
{
    public class PagamentoCriacaoDto
    {
        public decimal? Dinheiro { get; set; }

        public decimal? Troco { get; set; }

        public decimal? Preco { get; set; }

        public string? Cartao { get; set; }

        public int? Parcela { get; set; }

        public StatusCartao StatusCartao { get; set; }

        public int ProdutoId { get; set; } // 👈 Apenas o ID é necessário na criação!
    }
}