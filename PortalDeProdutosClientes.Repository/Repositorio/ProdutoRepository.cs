using PortalDeProdutosClientes.Data.Data;
using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Repository.Repositorio
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Produto> AdiocinarCliente(Produto produto)
        {
            await _context.AddAsync(produto);
            await _context.SaveChangesAsync();
            return produto;
        }
    }
}
