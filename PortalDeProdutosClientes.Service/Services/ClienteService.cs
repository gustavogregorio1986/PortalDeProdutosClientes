using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Repository.Interface;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.Repository.Repositorio;


namespace PortalDeProdutosClientes.Service.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Cliente> AdiocinarCliente(Cliente cliente)
        {
            return await _repository.AdiocinarCliente(cliente);
        }

        public async Task<List<Cliente>> ListarTodosClientes()
        {
            return await _repository.ListarTodosClientes();
        }

        public async Task AtualizarClienteAsync(Cliente cliente)
        {
            await _repository.AtualizarAsync(cliente);
        }

        public async Task<bool> DeletarClienteAsync(int id)
        {
            var cliente = await _repository.BuscarPorIdAsync(id);
            if (cliente == null) return false;

            await _repository.DeletarAsync(cliente);
            return true;
        }

        public async Task<Cliente?> BuscarPorIdAsync(int id)
        {
            return await _repository.BuscarPorIdAsync(id);
        }
    }
}
