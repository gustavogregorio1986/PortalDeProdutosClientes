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
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> AdiocinarCliente(Cliente cliente)
        {
            await _context.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;

        }

        public async Task<List<Cliente>> ListarTodosClientes()
        {
            return await _context.Clientes.Include(c => c.Produtos).ToListAsync();
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> BuscarPorIdAsync(int id)
        {
            return await _context.Clientes
            .Include(c => c.Produtos)
            .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
