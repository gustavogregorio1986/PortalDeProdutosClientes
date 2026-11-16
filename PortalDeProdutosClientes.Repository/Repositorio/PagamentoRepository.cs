using Microsoft.EntityFrameworkCore;
using PortalDeProdutosClientes.Data.Data;
using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Repository.Interface;

namespace PortalDeProdutosClientes.Repository.Repositorio
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly AppDbContext _context;

        public PagamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pagamento> AdicionarAsync(Pagamento pagamento)
        {
            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync(); // Dispara o SaveChangesAsync interceptado pela auditoria
            return pagamento;
        }

        public async Task<List<Pagamento>> ListarTodosPagamentos()
        {
            return await _context.Pagamentos
                .Include(p => p.Produto) // Traz os dados do produto junto
                .ToListAsync();
        }

        public async Task<Pagamento?> BuscarPorIdAsync(int id)
        {
            return await _context.Pagamentos
               .Include(p => p.Produto)
               .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AtualizarAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(int id)
        {
            var pagamento = await BuscarPorIdAsync(id);
            if (pagamento != null)
            {
                _context.Pagamentos.Remove(pagamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}