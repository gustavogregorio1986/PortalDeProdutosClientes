using PortalDeProdutosClientes.Dominain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.DTOs
{
     // DTO Principal do Produto(para cadastros, edições ou detalhes completos)
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public int UnidadesRestantes { get; set; }
        public DateTime DataFabricacao { get; set; }
        public int ClienteId { get; set; }

        // Relacionamento: Lista de pagamentos associados a este produto (usando DTO e não a entidade)
        public List<PagamentoResumoDto> Pagamentos { get; set; }
    }
}
