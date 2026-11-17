using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Domain.Entidades;
using PortalDeProdutosClientes.Domain.Interfaces;
using PortalDeProdutosClientes.Dominain.Services;

[Route("api/[controller]")]
[ApiController]
public class AuditoriaController : ControllerBase
{
    private readonly ILogAuditoriaService _logService;
    private readonly IRelatorioPdfService _relatorioPdfService;

    public AuditoriaController(
        ILogAuditoriaService logService,
        IRelatorioPdfService relatorioPdfService)
    {
        _logService = logService;
        _relatorioPdfService = relatorioPdfService;
    }

    [HttpGet]
    public async Task<IActionResult> ListarLogs()
    {
        var logs = await _logService.ListarTodosLogsAsync();
        return Ok(logs);
    }

    [HttpGet("exportar-pdf")]
    public async Task<IActionResult> ExportarPdf()
    {
        var logs = await _logService.ListarTodosLogsAsync();

        if (!logs.Any())
            return NotFound("Nenhum log encontrado para gerar o relatório.");

        var colunas = new Dictionary<string, Func<LogAuditoria, string>>
        {
            { "Data/Hora", l => l.DataOcorrencia.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss") },
            { "Usuário", l => string.IsNullOrEmpty(l.UsuarioEmail) ? "Sistema" : l.UsuarioEmail },
            { "Ação", l => string.IsNullOrEmpty(l.Acao) ? "-" : l.Acao },
            { "Entidade", l => l.RegistroId.HasValue ? $"{l.EntidadeAfetada} (#{l.RegistroId.Value})" : (string.IsNullOrEmpty(l.EntidadeAfetada) ? "-" : l.EntidadeAfetada) },
            { "Detalhes", l => string.IsNullOrEmpty(l.Detalhes) ? "-" : l.Detalhes }
        };

        var pdfBytes = _relatorioPdfService.GerarTabelaPdf(
            "Relatório de Auditoria - Portal de Produtos e Clientes",
            logs,
            colunas);

        var nomeArquivo = $"relatorio-auditoria-{DateTime.Now:yyyyMMdd-HHmm}.pdf";
        return File(pdfBytes, "application/pdf", nomeArquivo);
    }
}