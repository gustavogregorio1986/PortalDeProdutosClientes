using PortalDeProdutosClientes.Domain.Entidades;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.Repository;

namespace PortalDeProdutosClientes.Services
{
    public class LogAuditoriaService : ILogAuditoriaService
    {
        private readonly ILogAuditoriaRepository _logRepository;

        public LogAuditoriaService(ILogAuditoriaRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task RegistrarLogAsync(string acao, string usuarioId, string usuarioEmail, string entidadeAfetada, int? registroId, string detalhesJson)
        {
            var log = new LogAuditoria
            {
                Acao = acao,
                UsuarioId = usuarioId,
                UsuarioEmail = usuarioEmail,
                EntidadeAfetada = entidadeAfetada,
                RegistroId = registroId,
                Detalhes = detalhesJson,
                DataOcorrencia = DateTime.UtcNow
            };

            await _logRepository.AdicionarAsync(log);
        }

        public async Task<IEnumerable<LogAuditoria>> ListarTodosLogsAsync()
        {
            return await _logRepository.ObterTodosAsync();
        }

        public async Task<IEnumerable<LogAuditoria>> ListarLogsPorRegistroAsync(string entidadeAfetada, int registroId)
        {
            return await _logRepository.ObterPorEntidadeAsync(entidadeAfetada, registroId);
        }
    }
}