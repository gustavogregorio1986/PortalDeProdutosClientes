using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Repository.Interface;
using PortalDeProdutosClientes.Dominain.Services;


namespace PortalDeProdutosClientes.Service.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Produto> AdiocinarProduto(Produto produto)
        {
            return await _repository.AdiocinarProduto(produto);
        }

        public async Task AtualizarProdutoAsync(Produto produto)
        {
            await _repository.AtualizarAsync(produto);
        }

        public async Task<Produto?> BuscarPorIdAsync(int id)
        {
            return await _repository.BuscarPorIdAsync(id);
        }

        public async Task<bool> DeletarProdutoAsync(int id)
        {
            var produto = await _repository.BuscarPorIdAsync(id);
            if (produto == null) return false;

            await _repository.DeletarAsync(id);
            return true;
        }

        public async Task<List<Produto>> ListarTodosProdutos()
        {
            return await _repository.ListarTodosProdutos();
        }
    }
}
