
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace InventoryApi.Models.Clases
{
    public static class Custom
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void WritePDF(string path)
        {
            //QuestPDF.Settings.License = LicenseType.Community;

            //Document.Create(container =>
            //{

            //}).ShowInPreviewer();

        }


        //private static Table crearTable()
        //{
        //    Table table = new Table(2, false);
        //    Cell cell11 = new Cell(1, 1)
        //       .SetBackgroundColor(ColorConstants.GRAY)
        //       .SetTextAlignment(TextAlignment.CENTER)
        //       .Add(new Paragraph("State"));
        //    Cell cell12 = new Cell(1, 1)
        //       .SetBackgroundColor(ColorConstants.GRAY)
        //       .SetTextAlignment(TextAlignment.CENTER)
        //       .Add(new Paragraph("Capital"));

        //    Cell cell21 = new Cell(1, 1)
        //       .SetTextAlignment(TextAlignment.CENTER)
        //       .Add(new Paragraph("New York"));
        //    Cell cell22 = new Cell(1, 1)
        //       .SetTextAlignment(TextAlignment.CENTER)
        //       .Add(new Paragraph("Albany"));

        //    Cell cell31 = new Cell(1, 1)
        //       .SetTextAlignment(TextAlignment.CENTER)
        //       .Add(new Paragraph("New Jersey"));
        //    Cell cell32 = new Cell(1, 1)
        //       .SetTextAlignment(TextAlignment.CENTER)
        //       .Add(new Paragraph("Trenton"));

        //    Cell cell41 = new Cell(1, 1)
        //       .SetTextAlignment(TextAlignment.CENTER)
        //       .Add(new Paragraph("California"));
        //    Cell cell42 = new Cell(1, 1)
        //       .SetTextAlignment(TextAlignment.CENTER)
        //       .Add(new Paragraph("Sacramento"));

        //    table.AddCell(cell11);
        //    table.AddCell(cell12);
        //    table.AddCell(cell21);
        //    table.AddCell(cell22);
        //    table.AddCell(cell31);
        //    table.AddCell(cell32);
        //    table.AddCell(cell41);
        //    table.AddCell(cell42);
        //    return table;
        //}
        //public static void WritePDF(string path)
        //{
        //    PdfWriter writer = new PdfWriter(path + ".pdf");
        //    PdfDocument pdf = new PdfDocument(writer);
        //    Document document = new Document(pdf, PageSize.A4);




        //    Paragraph header = new Paragraph("HEADER")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetFontSize(20);

        //    Paragraph subHeader = new Paragraph("SubHeader")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetFontSize(14);

        //    LineSeparator ls = new LineSeparator(new SolidLine());

        //    Image img = new Image(ImageDataFactory.Create("C:\\Users\\Gashe\\source\\repos\\InventoryApi\\InventoryApi\\bin\\Debug\\net6.0\\Images\\11YUWVHPON.png"))
        //        .SetTextAlignment(TextAlignment.CENTER);

        //    Link link = new Link("click here",
        //        PdfAction.CreateURI("https://www.google.com"));
        //    Paragraph hyperlink = new Paragraph("Please ")
        //        .Add(link.SetBold().SetUnderline()
        //        .SetItalic().SetFontColor(ColorConstants.BLUE))
        //        .Add(" to go www.google.com");

        //    pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, new PageEventHandler(document));



        //    document.Add(header);
        //    document.Add(subHeader);
        //    document.Add(ls);
        //    document.Add(crearTable());

        //    document.Add(hyperlink);



        //    document.Add(img);
        //    //No funciona el paginador este



        //    int n = pdf.GetNumberOfPages();
        //    for (int i = 1; i <= n; i++)
        //    {


        //        document.Add(new Paragraph("page " + i + " of " + n)
        //            .SetTextAlignment(TextAlignment.RIGHT)
        //            .SetVerticalAlignment(VerticalAlignment.TOP)
        //        );
        //    }



        //    document.Close();
        //}




        //private class PageEventHandler : IEventHandler
        //{
        //    Document doc;
        //    public PageEventHandler(Document doc)
        //    {
        //        this.doc = doc;
        //    }

        //    public void HandleEvent(Event @event)
        //    {
        //        PdfDocumentEvent docEvent = @event as PdfDocumentEvent;
        //        if (docEvent != null)
        //        {
        //            float right = 10;
        //            Paragraph yes = new Paragraph("yes")
        //                .SetRelativePosition(50, 0, 0, 0);
        //            this.doc.Add(yes);
        //        }
        //    }
        //}


    }
}
