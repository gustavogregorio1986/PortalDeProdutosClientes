using Microsoft.AspNetCore.Mvc;
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

        public PagamentosController(IPagamentoService pagamentoService, ILogAuditoriaService logAuditoriaService)
        {
            _pagamentoService = pagamentoService;
            _logAuditoriaService = logAuditoriaService;
        }

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

                var pagamentoCriado = await _pagamentoService.AdicionarPagamento(pagamentoEntidade);

                await _logAuditoriaService.RegistrarLogAsync(
                    acao: "CRIACAO",
                    usuarioId: "sistema-interno",
                    usuarioEmail: "admin@portaldeprodutos.com",
                    entidadeAfetada: "Pagamento",
                    registroId: pagamentoCriado.Id,
                    detalhesJson: $"Pagamento no valor de R$ {pagamentoCriado.Preco} registrado com sucesso para o produto ID {pagamentoCriado.ProdutoId}."
                );

                return Ok(new { Mensagem = "Pagamento registrado com sucesso!", Dados = pagamentoCriado });
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

        [HttpPost("popular-auditoria-antiga")]
        public async Task<IActionResult> PopularAuditoriaAntiga()
        {
            try
            {
                // 1. Busca todos os pagamentos cadastrados
                var pagamentos = await _pagamentoService.ListarTodosPagamentos(); // Ajuste para o nome do método no seu service

                if (pagamentos == null || !pagamentos.Any())
                    return NotFound(new { Mensagem = "Nenhum pagamento encontrado para popular a auditoria." });

                int totalProcessado = 0;

                foreach (var pagamento in pagamentos)
                {
                    // 2. Registra a auditoria para cada pagamento existente
                    await _logAuditoriaService.RegistrarLogAsync(
                        acao: "IMPORTACAO_HISTORICO",
                        usuarioId: "sistema-interno",
                        usuarioEmail: "admin@portaldeprodutos.com",
                        entidadeAfetada: "Pagamento",
                        registroId: pagamento.Id,
                        detalhesJson: $"Pagamento ID {pagamento.Id} no valor de R$ {pagamento.Preco} importado para o histórico de auditoria."
                    );

                    totalProcessado++;
                }

                return Ok(new
                {
                    Mensagem = "Histórico de auditoria populado com sucesso para todos os pagamentos!",
                    TotalImportados = totalProcessado
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao popular auditoria de pagamentos.", Erro = ex.Message });
            }
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
    }
}