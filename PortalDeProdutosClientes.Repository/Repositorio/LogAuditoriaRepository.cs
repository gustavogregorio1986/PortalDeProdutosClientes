using Microsoft.EntityFrameworkCore;
using PortalDeProdutosClientes.Data;
using PortalDeProdutosClientes.Data.Data;
using PortalDeProdutosClientes.Domain.Entidades;

namespace PortalDeProdutosClientes.Repository
{
    public class LogAuditoriaRepository : ILogAuditoriaRepository
    {
        private readonly AppDbContext _context;

        public LogAuditoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(LogAuditoria log)
        {
            _context.LogAuditorias.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LogAuditoria>> ObterTodosAsync()
        {
            return await _context.LogAuditorias
                .OrderByDescending(x => x.DataOcorrencia)
                .ToListAsync();
        }

        public async Task<IEnumerable<LogAuditoria>> ObterPorEntidadeAsync(string entidadeAfetada, int registroId)
        {
            return await _context.LogAuditorias
                .Where(x => x.EntidadeAfetada == entidadeAfetada && x.RegistroId == registroId)
                .OrderByDescending(x => x.DataOcorrencia)
                .ToListAsync();
        }
    }
}