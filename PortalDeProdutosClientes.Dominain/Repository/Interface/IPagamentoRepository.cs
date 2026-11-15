using PortalDeProdutosClientes.Dominain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Dominain.Repository.Interface
{
    public interface IPagamentoRepository
    {
        Task<Pagamento> AdiocinarPagamento(Pagamento pagamento);

        Task<List<Pagamento>> ListarTodosPagamentos();

        Task<Pagamento?> BuscarPorIdAsync(int id);

        Task AtualizarAsync(Pagamento pagamento);
        Task DeletarAsync(int id);

    }
}
