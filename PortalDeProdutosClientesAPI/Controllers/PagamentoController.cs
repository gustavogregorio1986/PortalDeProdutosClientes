using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.DTOs;

namespace PortalDeProdutosClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] PagamentoCriacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 1. Converte o DTO de Entrada para a Entidade de Domínio (Pagamento)
                var pagamentoEntidade = new Pagamento
                {
                    Dinheiro = dto.Dinheiro,
                    Troco = dto.Troco,
                    Preco = dto.Preco,
                    Cartao = dto.Cartao,
                    Parcela = dto.Parcela,
                    StatusCartao = dto.StatusCartao,
                    ProdutoId = dto.ProdutoId
                };

                // 2. Salva no banco via Service/Repositório (retorna a entidade com o ID preenchido)
                var pagamentoSalvo = await _pagamentoService.AdicionarPagamento(pagamentoEntidade);

                // 3. Converte a Entidade salva de volta para o DTO de Resposta (Usando PagamentoDto para ter o ID)
                var pagamentoDtoRetorno = new PagamentoCriacaoDto
                {
                    Dinheiro = pagamentoSalvo.Dinheiro,
                    Troco = pagamentoSalvo.Troco,
                    Preco = pagamentoSalvo.Preco,
                    Cartao = pagamentoSalvo.Cartao,
                    Parcela = pagamentoSalvo.Parcela,
                    StatusCartao = pagamentoSalvo.StatusCartao,
                    ProdutoId = pagamentoSalvo.ProdutoId
                };

                // 4. Retorna o sucesso encapsulado no Ok() da API
                return Ok(new
                {
                    Mensagem = "Pagamento cadastrado com sucesso!",
                    Dados = pagamentoDtoRetorno
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao cadastrar pagamento.", Erro = ex.Message });
            }
        }
    }
}