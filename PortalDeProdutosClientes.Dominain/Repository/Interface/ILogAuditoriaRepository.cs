using PortalDeProdutosClientes.Domain.Entidades;

namespace PortalDeProdutosClientes.Repository
{
    public interface ILogAuditoriaRepository
    {
        Task AdicionarAsync(LogAuditoria log);
        Task<IEnumerable<LogAuditoria>> ObterTodosAsync();
        Task<IEnumerable<LogAuditoria>> ObterPorEntidadeAsync(string entidadeAfetada, int registroId);
    }
}