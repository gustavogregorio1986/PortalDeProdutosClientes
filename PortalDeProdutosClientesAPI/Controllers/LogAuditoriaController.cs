using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.Services;

namespace PortalDeProdutosClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogAuditoriaController : ControllerBase
    {
        private readonly ILogAuditoriaService _logService;

        public LogAuditoriaController(ILogAuditoriaService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var logs = await _logService.ListarTodosLogsAsync();
            return Ok(logs);
        }

        [HttpGet("filtro")]
        public async Task<IActionResult> ObterPorRegistro([FromQuery] string entidade, [FromQuery] int registroId)
        {
            var logs = await _logService.ListarLogsPorRegistroAsync(entidade, registroId);
            return Ok(logs);
        }
    }
}