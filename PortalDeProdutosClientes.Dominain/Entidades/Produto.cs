using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Dominain.Entidades
{
    public class Produto
    {
        public int Id { get; set; }

        public string NomeProduto { get; set; }

        public int Quantidade { get; set; }

        public decimal Preco { get; set; }

        public int UnidadesRestantes { get; set; }

        public DateTime DataFabricacao { get; set; }

        public Cliente Cliente { get; set; }

        public int ClienteId { get; set; }

        [JsonIgnore]
        public List<Pagamento> Pagamentos { get; set; }
    }
}
