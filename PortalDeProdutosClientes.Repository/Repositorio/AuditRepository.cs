using Microsoft.EntityFrameworkCore;
using PortalDeProdutosClientes.Data.Data;
using PortalDeProdutosClientes.Domain.Entities;
using PortalDeProdutosClientes.Domain.Interfaces;

namespace PortalDeProdutosClientes.Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _context;

        public AuditRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            // Salva a alteração imediatamente no banco de dados
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> ObterUltimosLogsAsync(int quantidade)
        {
            return await _context.AuditLogs
                .OrderByDescending(l => l.DataHora)
                .Take(quantidade)
                .ToListAsync();
        }
    }
}