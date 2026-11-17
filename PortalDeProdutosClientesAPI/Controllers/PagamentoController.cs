using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Domain.Enums;
using PortalDeProdutosClientes.Domain.Interfaces;
using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.DTOs;

namespace PortalDeProdutosClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;
        private readonly ILogAuditoriaService _logAuditoriaService;
        private readonly IRelatorioPdfService _relatorioPdfService;

        public PagamentosController(IPagamentoService pagamentoService, ILogAuditoriaService logAuditoriaService, IRelatorioPdfService relatorioPdfService)
        {
            _pagamentoService = pagamentoService;
            _logAuditoriaService = logAuditoriaService;
            _relatorioPdfService = relatorioPdfService;
        }

        // Método POST único e unificado para registrar pagamentos com detecção de status
        [HttpPost]
        public async Task<IActionResult> RegistrarPagamento([FromBody] PagamentoCriacaoDto pagamentoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var pagamentoEntidade = new Pagamento
                {
                    Dinheiro = pagamentoDto.Dinheiro,
                    Troco = pagamentoDto.Troco,
                    Preco = pagamentoDto.Preco,
                    Cartao = pagamentoDto.Cartao,
                    Parcela = pagamentoDto.Parcela,
                    StatusCartao = pagamentoDto.StatusCartao,
                    ProdutoId = pagamentoDto.ProdutoId
                };

                // 1. Processa a efetivação (detector de status: Aprovado, Recusado ou Pendente)
                var pagamentoCriado = await _pagamentoService.ProcessarEfetivacaoPagamentoAsync(pagamentoEntidade);

                // 2. Registra o log de auditoria
                await _logAuditoriaService.RegistrarLogAsync(
                    acao: "CRIACAO",
                    usuarioId: "sistema-interno",
                    usuarioEmail: "admin@portaldeprodutos.com",
                    entidadeAfetada: "Pagamento",
                    registroId: pagamentoCriado.Id,
                    detalhesJson: $"Pagamento no valor de R$ {pagamentoCriado.Preco} registrado com status {pagamentoCriado.StatusPagamento} para o produto ID {pagamentoCriado.ProdutoId}."
                );

                return Ok(new
                {
                    Mensagem = "Pagamento registrado com sucesso!",
                    StatusDetectado = pagamentoCriado.StatusPagamento.ToString(),
                    Dados = pagamentoCriado
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao registrar pagamento.", Erro = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var pagamentos = await _pagamentoService.ListarTodosPagamentos();
            return Ok(pagamentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var pagamento = await _pagamentoService.BuscarPorIdAsync(id);
            if (pagamento == null) return NotFound(new { Mensagem = "Pagamento não encontrado." });
            return Ok(pagamento);
        }

        [HttpPost("corrigir-status-antigos")]
        public async Task<IActionResult> CorrigirStatusAntigos()
        {
            var pagamentos = await _pagamentoService.ListarTodosPagamentos();
            int corrigidos = 0;

            foreach (var p in pagamentos)
            {
                // Se estiver com status 0 (ou Pendente incorreto), reavalia
                if (p.StatusPagamento == 0)
                {
                    if (p.Dinheiro.HasValue && p.Dinheiro < p.Preco)
                        p.StatusPagamento = StatusPagamento.Recusado;
                    else if ((p.Dinheiro.HasValue && p.Dinheiro >= p.Preco) || (!string.IsNullOrEmpty(p.Cartao) && p.Preco > 0))
                        p.StatusPagamento = StatusPagamento.Aprovado;
                    else
                        p.StatusPagamento = StatusPagamento.Pendente;

                    await _pagamentoService.AtualizarPagamentoAsync(p);
                    corrigidos++;
                }
            }

            return Ok(new { Mensagem = "Status corrigidos com sucesso!", TotalCorrigidos = corrigidos });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Pagamento pagamento)
        {
            if (id != pagamento.Id) return BadRequest(new { Mensagem = "ID divergente." });

            var existente = await _pagamentoService.BuscarPorIdAsync(id);
            if (existente == null) return NotFound(new { Mensagem = "Pagamento não encontrado." });

            await _pagamentoService.AtualizarPagamentoAsync(pagamento);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _pagamentoService.DeletarPagamentoAsync(id);
            if (!sucesso) return NotFound(new { Mensagem = "Pagamento não encontrado." });

            return NoContent();
        }

        [HttpGet("exportar-pdf")]
        public async Task<IActionResult> ExportarPdf()
        {
            var pagamentos = await _pagamentoService.ListarTodosPagamentos(); // ajuste para o método do seu service

            if (!pagamentos.Any())
                return NotFound("Nenhum pagamento encontrado para gerar o relatório.");

            var colunas = new Dictionary<string, Func<Pagamento, string>>
        {
            { "ID", p => p.Id.ToString() },
            { "Produto ID", p => p.ProdutoId.ToString() },
            { "Produto Nome", p => p.Produto != null ? p.Produto.NomeProduto : "-" },
            { "Preço", p => p.Preco.HasValue ? p.Preco.Value.ToString("C2") : "-" },
            { "Dinheiro", p => p.Dinheiro.HasValue ? p.Dinheiro.Value.ToString("C2") : "-" },
            { "Troco", p => p.Troco.HasValue ? p.Troco.Value.ToString("C2") : "-" },
            { "Cartão", p => string.IsNullOrEmpty(p.Cartao) ? "-" : p.Cartao },
            { "Parcelas", p => p.Parcela.HasValue ? p.Parcela.Value.ToString() : "-" },
            { "Status Cartão", p => p.StatusCartao.ToString() },
            { "Status Pagamento", p => p.StatusPagamento.ToString() }
        };

            var pdfBytes = _relatorioPdfService.GerarTabelaPdf(
                "Relatório de Pagamentos - Portal de Produtos e Clientes",
                pagamentos,
                colunas);

            var nomeArquivo = $"relatorio-pagamentos-{DateTime.Now:yyyyMMdd-HHmm}.pdf";
            return File(pdfBytes, "application/pdf", nomeArquivo);
        }

        [HttpGet("exportar-pdf-por-ano")]
        public async Task<IActionResult> ExportarPdfPorAno()
        {
            var pagamentos = await _pagamentoService.ListarTodosPagamentos();

            if (!pagamentos.Any())
                return NotFound("Nenhum pagamento encontrado para gerar o relatório anual.");

            // 1. Agrupa os pagamentos por Ano usando LINQ
            var totaisPorAno = pagamentos
                .GroupBy(p => p.DataPagamento.Year) // Agrupa pelo ano da data do pagamento
                .Select(grupo => new PagamentoTotalAnualDto
                {
                    Ano = grupo.Key,
                    TotalArrecadado = grupo.Where(x => x.Preco.HasValue).Sum(x => x.Preco.Value),
                    QuantidadePagamentos = grupo.Count()
                })
                .OrderBy(x => x.Ano)
                .ToList();

            // 2. Define as colunas para o PDF do resumo anual
            var colunas = new Dictionary<string, Func<PagamentoTotalAnualDto, string>>
    {
        { "Ano", item => item.Ano.ToString() },
        { "Qtd. Pagamentos", item => item.QuantidadePagamentos.ToString() },
        { "Total Arrecadado", item => item.TotalArrecadado.ToString("C2") }
    };

            // 3. Gera o PDF utilizando o seu serviço existente
            var pdfBytes = _relatorioPdfService.GerarTabelaPdf(
                "Relatório de Totais por Ano - Portal de Produtos e Clientes",
                totaisPorAno,
                colunas);

            var nomeArquivo = $"relatorio-pagamentos-por-ano-{DateTime.Now:yyyyMMdd-HHmm}.pdf";
            return File(pdfBytes, "application/pdf", nomeArquivo);
        }
    }
}