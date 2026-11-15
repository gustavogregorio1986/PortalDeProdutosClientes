using PortalDeProdutosClientes.Dominain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Dominain.Services
{
    public interface IClienteService
    {
        Task<Cliente> AdiocinarCliente(Cliente cliente);

        Task<List<Cliente>> ListarTodosClientes();

        Task AtualizarClienteAsync(Cliente cliente);
        Task<bool> DeletarClienteAsync(int id);

        Task<Cliente?> BuscarPorIdAsync(int id);
    }
}
