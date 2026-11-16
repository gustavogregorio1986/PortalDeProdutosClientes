using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PortalDeProdutosClientes.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Tabela { get; set; }
        public string Acao { get; set; } // Added, Modified, Deleted
        public string Usuario { get; set; }
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public string? ValoresNovos { get; set; }
    }
}
namespace PortalDeProdutosClientes.Dominain.Repository.Interface
{
    internal class AuditLog
    {
    }
}
