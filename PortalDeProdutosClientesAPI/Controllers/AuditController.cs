using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Application.Interfaces;

namespace PortalDeProdutosClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int quantidade = 50)
        {
            var logs = await _auditService.ListarLogsAsync(quantidade);
            return Ok(logs);
        }
    }
}