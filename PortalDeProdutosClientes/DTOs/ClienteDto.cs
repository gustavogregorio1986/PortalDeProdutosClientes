using PortalDeProdutosClientes.Dominain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.DTOs
{
    public class ClienteDto
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
