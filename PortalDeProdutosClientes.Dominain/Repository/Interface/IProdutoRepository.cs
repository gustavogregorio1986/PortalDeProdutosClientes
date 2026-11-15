using PortalDeProdutosClientes.Dominain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Dominain.Repository.Interface
{
    public interface IProdutoRepository
    {
        Task<Produto> AdiocinarProduto(Produto produto);

        Task<List<Produto>> ListarTodosProdutos();

        Task<Produto?> BuscarPorIdAsync(int id);

        Task AtualizarAsync(Produto produto);
        Task DeletarAsync(int id);

    }
}
