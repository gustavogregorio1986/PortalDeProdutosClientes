using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Dominain.Services;

[Route("api/[controller]")]
[ApiController]
public class AuditoriaController : ControllerBase
{
    private readonly ILogAuditoriaService _logService;

    public AuditoriaController(ILogAuditoriaService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<IActionResult> ListarLogs()
    {
        var logs = await _logService.ListarTodosLogsAsync(); // Ajuste conforme o método do seu service
        return Ok(logs);
    }
}