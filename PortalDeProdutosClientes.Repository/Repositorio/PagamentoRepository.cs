using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Repository.Repositorio
{
    public class PagamentoRepository : IPagamentoRepository
    {
        public Task<Pagamento> AdiocinarCliente(Pagamento pagamento)
        {
            throw new NotImplementedException();
        }
    }
}
