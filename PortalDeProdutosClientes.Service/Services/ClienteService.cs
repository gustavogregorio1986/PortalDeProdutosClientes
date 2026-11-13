using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Repository.Interface;
using PortalDeProdutosClientes.Dominain.Services;


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
    }
}
