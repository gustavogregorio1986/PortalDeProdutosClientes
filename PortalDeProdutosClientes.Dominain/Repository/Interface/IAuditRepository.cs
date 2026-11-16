using PortalDeProdutosClientes.Domain.Entities;

namespace PortalDeProdutosClientes.Domain.Interfaces
{
    public interface IAuditRepository
    {
        Task AdicionarAsync(AuditLog auditLog);
        Task<IEnumerable<AuditLog>> ObterUltimosLogsAsync(int quantidade);
    }
}