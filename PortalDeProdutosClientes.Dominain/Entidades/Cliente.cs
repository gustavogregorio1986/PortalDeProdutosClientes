using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Dominain.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }

        public string NomeCliente { get; set; }

        public string EmailCliente { get; set; }

        public string Cpf { get; set; }

        public string Sexo { get; set; }

        [JsonIgnore]
        public List<Produto> Produtos { get; set; }
    }
}
