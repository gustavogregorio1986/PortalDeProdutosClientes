using PortalDeProdutosClientes.Dominain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Dominain.Services
{
    public interface IPagamentoService
    {
        Task<Pagamento> AdicionarPagamento(Pagamento pagamento);

        Task<List<Pagamento>> ListarTodosPagamentos();

        Task AtualizarPagamentoAsync(Pagamento pagamento);
        Task<bool> DeletarPagamentoAsync(int id);

        Task<Pagamento?> BuscarPorIdAsync(int id);

        Task<Pagamento> ProcessarEfetivacaoPagamentoAsync(Pagamento pagamento);
    }
}
