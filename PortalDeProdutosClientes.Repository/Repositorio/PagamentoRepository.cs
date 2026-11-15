using Microsoft.EntityFrameworkCore;
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
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly AppDbContext _context;

        public PagamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pagamento> AdiocinarPagamento(Pagamento pagamento)
        {
            await _context.AddAsync(pagamento);
            await _context.SaveChangesAsync();
            return pagamento;
        }

        public async Task<List<Pagamento>> ListarTodosPagamentos()
        {
            return await _context.Pagamentos.ToListAsync();
        }

        public async Task AtualizarAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();
        }

      
        public async Task DeletarAsync(int id)
        {
            await _context.Pagamentos
           .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Pagamento?> BuscarPorIdAsync(int id)
        {
            return await _context.Pagamentos
               .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
