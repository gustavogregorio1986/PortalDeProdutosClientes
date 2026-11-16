using PortalDeProdutosClientes.Dominain.Entidades;

namespace PortalDeProdutosClientes.Dominain.Repository.Interface
{
    public interface IPagamentoRepository
    {
        Task<Pagamento> AdicionarAsync(Pagamento pagamento);
        Task<List<Pagamento>> ListarTodosPagamentos();
        Task<Pagamento?> BuscarPorIdAsync(int id);
        Task AtualizarAsync(Pagamento pagamento);
        Task DeletarAsync(int id);
    }
}