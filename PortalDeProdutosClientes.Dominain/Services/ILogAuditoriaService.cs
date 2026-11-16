using PortalDeProdutosClientes.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Dominain.Services
{
    public interface ILogAuditoriaService
    {
        Task RegistrarLogAsync(string acao, string usuarioId, string usuarioEmail, string entidadeAfetada, int? registroId, string detalhesJson);
        Task<IEnumerable<LogAuditoria>> ListarTodosLogsAsync();
        Task<IEnumerable<LogAuditoria>> ListarLogsPorRegistroAsync(string entidadeAfetada, int registroId);
    }
}
