using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.DTOs;

namespace PortalDeProdutosClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ProdutoCriacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 1. Converte o DTO de Entrada para a Entidade de Domínio (Produto)
                var produtoEntidade = new Produto
                {
                    NomeProduto = dto.NomeProduto,
                    Quantidade = dto.Quantidade,
                    Preco = dto.Preco,
                    UnidadesRestantes = dto.UnidadesRestantes,
                    DataFabricacao = dto.DataFabricacao,
                    ClienteId = dto.ClienteId
                };

                // 2. Salva no banco via Service/Repositório (retorna a entidade com o ID preenchido)
                var produtoSalvo = await _produtoService.AdiocinarProduto(produtoEntidade);

                // 3. Converte a Entidade salva de volta para o seu ProdutoDto de Resposta
                var produtoDtoRetorno = new ProdutoCriacaoDto
                {
                    Id = produtoSalvo.Id,
                    NomeProduto = produtoSalvo.NomeProduto,
                    Quantidade = produtoSalvo.Quantidade,
                    Preco = produtoSalvo.Preco,
                    UnidadesRestantes = produtoSalvo.UnidadesRestantes,
                    DataFabricacao = produtoSalvo.DataFabricacao,
                    ClienteId = produtoSalvo.ClienteId,
                    Pagamentos = new List<PagamentoCriacaoDto>() // Como acabou de ser criado, começa sem pagamentos
                };

                // 4. Retorna o sucesso encapsulado no Ok() da API
                return Ok(new
                {
                    Mensagem = "Produto cadastrado com sucesso!",
                    Dados = produtoDtoRetorno
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao cadastrar produto.", Erro = ex.Message });
            }
        }
    }
}
