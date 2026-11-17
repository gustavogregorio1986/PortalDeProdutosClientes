namespace PortalDeProdutosClientes.Domain.Interfaces;

public interface IRelatorioPdfService
{
    byte[] GerarTabelaPdf<T>(
        string titulo,
        IEnumerable<T> dados,
        Dictionary<string, Func<T, string>> colunas,
        string filtroAplicado = "Todos os registros");
}