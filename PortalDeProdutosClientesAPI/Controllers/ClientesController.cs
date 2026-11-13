using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.DTOs;
using PortalDeProdutosClientes.Service.Services; // Ou o namespace onde está a sua IClienteService

namespace PortalDeProdutosClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService; // 👈 1. Usando a Interface aqui

        public ClientesController(IClienteService clienteService) // 👈 2. O .NET vai resolver pelo Program.cs
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ClienteCriacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 1. Converte o DTO de Entrada para a Entidade de Domínio
                var clienteEntidade = new Cliente
                {
                    NomeCliente = dto.NomeCliente,
                    EmailCliente = dto.EmailCliente,
                    Cpf = dto.Cpf,
                    Sexo = dto.Sexo
                };

                // 2. Salva no banco via Service (retorna a entidade com o ID preenchido)
                var clienteSalvo = await _clienteService.AdiocinarCliente(clienteEntidade);

                // 3. Converte a Entidade salva de volta para o DTO de Resposta
                var clienteDtoRetorno = new ClienteCriacaoDto
                {
                    NomeCliente = clienteSalvo.NomeCliente,
                    EmailCliente = clienteSalvo.EmailCliente,
                    Cpf = clienteSalvo.Cpf,
                    Sexo = clienteSalvo.Sexo
                };

                // 4. Retorna o DTO encapsulado no Ok() da API
                return Ok(new
                {
                    Mensagem = "Cliente cadastrado com sucesso!",
                    Dados = clienteDtoRetorno
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = "Erro ao cadastrar cliente.", Erro = ex.Message });
            }
        }
    }
}