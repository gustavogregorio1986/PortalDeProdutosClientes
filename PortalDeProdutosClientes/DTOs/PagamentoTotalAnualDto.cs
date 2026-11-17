namespace PortalDeProdutosClientes.DTOs
{
    public class PagamentoTotalAnualDto
    {
        public int Ano { get; set; }
        public decimal TotalArrecadado { get; set; }
        public int QuantidadePagamentos { get; set; }
    }
}