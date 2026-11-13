using PortalDeProdutosClientes.Dominain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.DTOs
{
    public class PagamentoDto
    {
        public int Id { get; set; }

        public decimal? Dinheiro { get; set; }

        public decimal? Troco { get; set; }

        public decimal? Preco { get; set; }

        public string? Cartao { get; set; }

        public int? Parcela { get; set; }

        public StatusCartao StatusCartao { get; set; }

        public int ProdutoId { get; set; }

        public ProdutoDto Produto { get; set; }
    }
}
