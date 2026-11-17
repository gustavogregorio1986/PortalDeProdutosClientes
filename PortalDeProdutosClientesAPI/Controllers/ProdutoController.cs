using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Domain.Interfaces;
using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.DTOs;

namespace PortalDeProdutosClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ILogAuditoriaService _logAuditoriaService;
        private readonly IRelatorioPdfService _relatorioPdfService;

        public ProdutosController(IProdutoService produtoService, ILogAuditoriaService logAuditoriaService, IRelatorioPdfService relatorioPdfService)
        {
            _produtoService = produtoService;
            _logAuditoriaService = logAuditoriaService;
            _relatorioPdfService = relatorioPdfService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ProdutoCriacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var produtoEntidade = new Produto
                {
                    NomeProduto = dto.NomeProduto,
                    Quantidade = dto.Quantidade,
                    Preco = dto.Preco,
                    UnidadesRestantes = dto.UnidadesRestantes,
                    DataFabricacao = dto.DataFabricacao,
                    ClienteId = dto.ClienteId
                };

                var produtoSalvo = await _produtoService.AdiocinarProduto(produtoEntidade);

                await _logAuditoriaService.RegistrarLogAsync(
                    acao: "CRIACAO",
                    usuarioId: "sistema-interno",
                    usuarioEmail: "admin@portaldeprodutos.com",
                    entidadeAfetada: "Produto",
                    registroId: produtoSalvo.Id,
                    detalhesJson: $"Produto '{produtoSalvo.NomeProduto}' cadastrado com sucesso (Preço: R$ {produtoSalvo.Preco})."
                );

                return Ok(new { Mensagem = "Produto cadastrado com sucesso!", Dados = produtoSalvo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao cadastrar produto.", Erro = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var produtos = await _produtoService.ListarTodosProdutos();
            return Ok(produtos);
        }

        [HttpPost("popular-auditoria-antiga")]
        public async Task<IActionResult> PopularAuditoriaAntiga()
        {
            try
            {
                // 1. Busca todos os produtos cadastrados
                var produtos = await _produtoService.ListarTodosProdutos(); // Ajuste para o nome do seu método no service

                if (produtos == null || !produtos.Any())
                    return NotFound(new { Mensagem = "Nenhum produto encontrado para popular a auditoria." });

                int totalProcessado = 0;

                foreach (var produto in produtos)
                {
                    // 2. Registra a auditoria para cada produto existente
                    await _logAuditoriaService.RegistrarLogAsync(
                        acao: "IMPORTACAO_HISTORICO",
                        usuarioId: "sistema-interno",
                        usuarioEmail: "admin@portaldeprodutos.com",
                        entidadeAfetada: "Produto",
                        registroId: produto.Id,
                        detalhesJson: $"Produto '{produto.NomeProduto}' importado para o histórico de auditoria."
                    );

                    totalProcessado++;
                }

                return Ok(new
                {
                    Mensagem = "Histórico de auditoria populado com sucesso para todos os produtos!",
                    TotalImportados = totalProcessado
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao popular auditoria de produtos.", Erro = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var produto = await _produtoService.BuscarPorIdAsync(id);
            if (produto == null) return NotFound(new { Mensagem = "Produto não encontrado." });
            return Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id) return BadRequest(new { Mensagem = "ID divergente." });

            var existente = await _produtoService.BuscarPorIdAsync(id);
            if (existente == null) return NotFound(new { Mensagem = "Produto não encontrado." });

            await _produtoService.AtualizarProdutoAsync(produto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _produtoService.DeletarProdutoAsync(id);
            if (!sucesso) return NotFound(new { Mensagem = "Produto não encontrado." });

            return NoContent();
        }

        [HttpGet("exportar-pdf")]
        public async Task<IActionResult> ExportarPdf()
        {
            var produtos = await _produtoService.ListarTodosProdutos(); // ajuste para o método do seu service

            if (!produtos.Any())
                return NotFound("Nenhum produto encontrado para gerar o relatório.");

            var colunas = new Dictionary<string, Func<Produto, string>>
        {
            { "ID", p => p.Id.ToString() },
            { "Nome do Produto", p => string.IsNullOrEmpty(p.NomeProduto) ? "-" : p.NomeProduto },
            { "Quantidade", p => p.Quantidade.ToString() },
            { "Preço", p => p.Preco.ToString("C2") },
            { "Unidades Restantes", p => p.UnidadesRestantes.ToString() },
            { "Data de Fabricação", p => p.DataFabricacao.ToLocalTime().ToString("dd/MM/yyyy") },
            { "Cliente ID", p => p.ClienteId.ToString() }
        };

            var pdfBytes = _relatorioPdfService.GerarTabelaPdf(
                "Relatório de Produtos - Portal de Produtos e Clientes",
                produtos,
                colunas);

            var nomeArquivo = $"relatorio-produtos-{DateTime.Now:yyyyMMdd-HHmm}.pdf";
            return File(pdfBytes, "application/pdf", nomeArquivo);
        }
    }
}