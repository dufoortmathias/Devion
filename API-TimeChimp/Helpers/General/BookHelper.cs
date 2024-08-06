using Api.Devion.Models;
using System.Reflection.Metadata.Ecma335;

namespace Api.Devion.Helpers.General;
public class GeneralBookHelper
{
    public void CreateBook(string b, Item Part, string basePath, string hoofdArtikel, PdfDocument to)
    {
        if (b == "Monteren")
        {
            if (Part != null)
            {
                string path = basePath + @"05_PDF_DXF_STP_Compleet\" + Part.Number + ".pdf";

                Part.Parts.ForEach(part =>
                {
                    if (part != null)
                    {

                        if (part.Bewerking1.ToLower() != "monteren" && part.Bewerking2.ToLower() != "monteren" && part.Bewerking3.ToLower() != "monteren" && part.Bewerking4.ToLower() != "monteren")
                        {
                            path = basePath + @"05_PDF_DXF_STP_Compleet\" + part.Number + ".pdf";
                            if (File.Exists(path))
                            {
                                using (PdfDocument part2 = PdfReader.Open(path, PdfDocumentOpenMode.Import))
                                {
                                    CopyPages(part2, to);
                                }
                            }
                        }
                        if (part.Bewerking1.ToLower() == "monteren" || part.Bewerking2.ToLower() == "monteren" || part.Bewerking3.ToLower() == "monteren" || part.Bewerking4.ToLower() == "monteren")
                        {
                            CreateBook("Monteren", part, basePath, hoofdArtikel, to);
                        }
                    }
                });
            }
        }
        else if (b == "Lassen")
        {
            if (Part != null)
            {
                string path = basePath + @"05_PDF_DXF_STP_Compleet\" + Part.Number + ".pdf";

                Part.Parts.ForEach(part =>
                {
                    if (part != null)
                    {

                        if (part.Bewerking1.ToLower() != "lassen" && part.Bewerking2.ToLower() != "lassen" && part.Bewerking3.ToLower() != "lassen" && part.Bewerking4.ToLower() != "lassen")
                        {
                            path = basePath + @"05_PDF_DXF_STP_Compleet\" + part.Number + ".pdf";
                            if (File.Exists(path))
                            {
                                using (PdfDocument part2 = PdfReader.Open(path, PdfDocumentOpenMode.Import))
                                {
                                    CopyPages(part2, to);
                                }
                            }
                        }
                        if (part.Bewerking1.ToLower() == "lassen" || part.Bewerking2.ToLower() == "lassen" || part.Bewerking3.ToLower() == "lassen" || part.Bewerking4.ToLower() == "lassen")
                        {
                            CreateBook("Lassen", part, basePath, hoofdArtikel, to);
                        }
                    }
                });
            }
        }
    }

    public void CopyPages(PdfDocument from, PdfDocument to)
    {
        for (int i = 0; i < from.PageCount; i++)
        {
            to.AddPage(from.Pages[i]);
        }
    }
}
