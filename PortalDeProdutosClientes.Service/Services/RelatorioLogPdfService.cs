using PortalDeProdutosClientes.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PortalDeProdutosClientes.Infrastructure.Services;

public class RelatorioPdfService : IRelatorioPdfService
{
    public byte[] GerarTabelaPdf<T>(
        string titulo,
        IEnumerable<T> dados,
        Dictionary<string, Func<T, string>> colunas,
        string filtroAplicado = "Todos os registros")
    {
        var lista = dados.ToList();

        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.Lato));

                // Cabeçalho Padrão
                page.Header().Column(col =>
                {
                    col.Item().Text(titulo)
                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Darken3);

                    col.Item().Text($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss} | Filtro: {filtroAplicado} | Total: {lista.Count} registros")
                        .FontSize(9).FontColor(Colors.Grey.Darken1);

                    col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                });

                // Tabela Dinâmica
                page.Content().PaddingVertical(10).Table(table =>
                {
                    // Cria colunas com larguras iguais proporcionalmente
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var _ in colunas)
                            columns.RelativeColumn();
                    });

                    // Cabeçalho das Colunas
                    table.Header(header =>
                    {
                        foreach (var nomeColuna in colunas.Keys)
                        {
                            header.Cell().Element(EstiloCabecalho).Text(nomeColuna);
                        }

                        static IContainer EstiloCabecalho(IContainer c) =>
                            c.Background(Colors.Blue.Darken3)
                             .Padding(5)
                             .DefaultTextStyle(x => x.SemiBold().FontColor(Colors.White));
                    });

                    // Linhas de Dados
                    for (int i = 0; i < lista.Count; i++)
                    {
                        var item = lista[i];
                        var corFundo = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                        foreach (var extratorValor in colunas.Values)
                        {
                            var texto = extratorValor(item) ?? "-";
                            table.Cell().Element(c => EstiloCelula(c, corFundo)).Text(texto);
                        }
                    }

                    static IContainer EstiloCelula(IContainer c, string bg) =>
                        c.Background(bg)
                         .BorderBottom(0.5f)
                         .BorderColor(Colors.Grey.Lighten2)
                         .Padding(5);
                });

                // Rodapé Padrão
                page.Footer().AlignRight().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        });

        return documento.GeneratePdf();
    }
}