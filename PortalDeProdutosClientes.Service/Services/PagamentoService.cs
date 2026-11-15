using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Repository.Interface;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.Repository.Repositorio;


namespace PortalDeProdutosClientes.Service.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _repository;

        public PagamentoService(IPagamentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Pagamento> AdicionarPagamento(Pagamento pagamento)
        {
            return await _repository.AdiocinarPagamento(pagamento);
        }

        public async Task AtualizarPagamentoAsync(Pagamento pagamento)
        {
            await _repository.AtualizarAsync(pagamento);
        }

        public async Task<Pagamento?> BuscarPorIdAsync(int id)
        {
           return await _repository.BuscarPorIdAsync(id);
        }

        public async Task<bool> DeletarPagamentoAsync(int id)
        {
            var cliente = await _repository.BuscarPorIdAsync(id);
            if (cliente == null) return false;

            await _repository.DeletarAsync(id);
            return true;
        }

        public async Task<List<Pagamento>> ListarTodosPagamentos()
        {
            return await _repository.ListarTodosPagamentos();
        }
    }
}
