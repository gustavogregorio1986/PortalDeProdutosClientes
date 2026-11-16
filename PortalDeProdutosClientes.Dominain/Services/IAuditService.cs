using PortalDeProdutosClientes.Domain.Entities;

namespace PortalDeProdutosClientes.Application.Interfaces
{
    public interface IAuditService
    {
        Task<IEnumerable<AuditLog>> ListarLogsAsync(int quantidade = 50);
    }
}