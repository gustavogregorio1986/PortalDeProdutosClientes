using PortalDeProdutosClientes.Application.Interfaces;
using PortalDeProdutosClientes.Domain.Entities;
using PortalDeProdutosClientes.Domain.Interfaces;

namespace PortalDeProdutosClientes.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;

        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<IEnumerable<AuditLog>> ListarLogsAsync(int quantidade)
        {
            return await _auditRepository.ObterUltimosLogsAsync(quantidade);
        }
    }
}