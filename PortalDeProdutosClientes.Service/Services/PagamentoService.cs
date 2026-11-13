using PortalDeProdutosClientes.Dominain.Entidades;
using PortalDeProdutosClientes.Dominain.Repository.Interface;
using PortalDeProdutosClientes.Dominain.Services;


namespace PortalDeProdutosClientes.Service.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _repository;

        public PagamentoService(IPagamentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Pagamento> AdicionarPagamento(Pagamento pagamento)
        {
            return await _repository.AdiocinarPagamento(pagamento);
        }
    }
}
