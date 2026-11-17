using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Domain.Interfaces;
using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.DTOs;

namespace PortalDeProdutosClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IRelatorioPdfService _relatorioPdfService;
        private readonly ILogAuditoriaService _logAuditoriaService;

        public ClientesController(IClienteService clienteService, ILogAuditoriaService logAuditoriaService, IRelatorioPdfService relatorioPdfService)
        {
            _clienteService = clienteService;
            _logAuditoriaService = logAuditoriaService;
            _relatorioPdfService = relatorioPdfService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ClienteCriacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var clienteEntidade = new Cliente
                {
                    NomeCliente = dto.NomeCliente,
                    EmailCliente = dto.EmailCliente,
                    Cpf = dto.Cpf,
                    Sexo = dto.Sexo
                };

                var clienteSalvo = await _clienteService.AdiocinarCliente(clienteEntidade);

                await _logAuditoriaService.RegistrarLogAsync(
                    acao: "CRIACAO",
                    usuarioId: "sistema-interno",
                    usuarioEmail: "admin@portaldeprodutos.com",
                    entidadeAfetada: "Cliente",
                    registroId: clienteSalvo.Id,
                    detalhesJson: $"Cliente '{clienteSalvo.NomeCliente}' cadastrado com sucesso."
                );

                return Ok(new { Mensagem = "Cliente cadastrado com sucesso!", Dados = clienteSalvo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao cadastrar cliente.", Erro = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> ListarTodos()
        {
            var clientes = await _clienteService.ListarTodosClientes();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> ObterPorId(int id)
        {
            var cliente = await _clienteService.BuscarPorIdAsync(id);
            if (cliente == null) return NotFound(new { Mensagem = "Cliente não encontrado." });
            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest(new { Mensagem = "O ID da URL diverge do ID do objeto." });

            var existente = await _clienteService.BuscarPorIdAsync(id);
            if (existente == null) return NotFound(new { Mensagem = "Cliente não encontrado." });

            await _clienteService.AtualizarClienteAsync(cliente);
            return NoContent();
        }

        [HttpPost("popular-auditoria-antiga")]
        public async Task<IActionResult> PopularAuditoriaAntiga()
        {
            try
            {
                // 1. Busca todos os clientes cadastrados
                var clientes = await _clienteService.ListarTodosClientes();

                if (clientes == null || !clientes.Any())
                    return NotFound(new { Mensagem = "Nenhum cliente encontrado para popular a auditoria." });

                int totalProcessado = 0;

                foreach (var cliente in clientes)
                {
                    // 2. Registra a auditoria para cada cliente existente
                    await _logAuditoriaService.RegistrarLogAsync(
                        acao: "IMPORTACAO_HISTORICO",
                        usuarioId: "sistema-interno",
                        usuarioEmail: "admin@portaldeprodutos.com",
                        entidadeAfetada: "Cliente",
                        registroId: cliente.Id,
                        detalhesJson: $"Cliente '{cliente.NomeCliente}' importado para o histórico de auditoria."
                    );

                    totalProcessado++;
                }

                return Ok(new
                {
                    Mensagem = "Histórico de auditoria populado com sucesso para todos os clientes!",
                    TotalImportados = totalProcessado
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao popular auditoria.", Erro = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _clienteService.DeletarClienteAsync(id);
            if (!sucesso) return NotFound(new { Mensagem = "Cliente não encontrado para exclusão." });

            return NoContent();
        }

        [HttpGet("exportar-pdf")]
        public async Task<IActionResult> ExportarPdf()
        {
            var clientes = await _clienteService.ListarTodosClientes(); // ajuste para o método da sua service

            if (!clientes.Any())
                return NotFound("Nenhum cliente encontrado para gerar o relatório.");

            var colunas = new Dictionary<string, Func<Cliente, string>>
        {
            { "ID", c => c.Id.ToString() },
            { "Nome", c => c.NomeCliente },
            { "E-mail", c => c.EmailCliente },
            { "CPF", c => c.Cpf }, // ajuste o nome da propriedade se for Cpf, CpfCnpj, etc.
        };

            var pdfBytes = _relatorioPdfService.GerarTabelaPdf(
                "Relatório de Clientes - Portal de Produtos e Clientes",
                clientes,
                colunas);

            var nomeArquivo = $"relatorio-clientes-{DateTime.Now:yyyyMMdd-HHmm}.pdf";
            return File(pdfBytes, "application/pdf", nomeArquivo);
        }

    }
}