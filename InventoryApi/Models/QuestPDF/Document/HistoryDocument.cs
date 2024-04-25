using InventoryApi.Models.Clases;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventoryApi.Models.QuestPDF.Document
{
    public class HistoryModel : IDocument
    {
        public ItemPrice model { get; }

        public HistoryModel(ItemPrice model)
        {
            this.model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page => {
                page.Margin(30);
                page.Header().Element(ComposeHeader);


                page.Footer().Text(t => {
                    t.Span("Pagina ");
                    t.CurrentPageNumber();
                });
            });
        }

        private void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(24).SemiBold().FontColor(Colors.Indigo.Lighten1);
            container.Row(row => {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"Historial de Precios").Style(titleStyle);
                });

            });
        }
        private void ComposeContent(IContainer container)
        {

        }

    }
}
