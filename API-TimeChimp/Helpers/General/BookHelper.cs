using Api.Devion.Models;
using System.Reflection.Metadata.Ecma335;

namespace Api.Devion.Helpers.General;
public class GeneralBookHelper
{
    public void CreateBook(string b, Item Part, string basePath, string hoofdArtikel)
    {
        if (b == "Monteren")
        {
            if (Part != null)
            {
                string path = basePath + @"05_PDF_DXF_STP_Compleet\" + Part.Number + ".pdf";

                PdfDocument to = new PdfDocument();
                // check if file exists
                if (File.Exists(path))
                {
                    using (PdfDocument part = PdfReader.Open(path, PdfDocumentOpenMode.Import))
                    {
                        CopyPages(part, to);
                    }
                }

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
                    }
                });
                // save pdf
                //check if folder exists
                if (Directory.Exists(basePath + @"03_Montage_boek") == false)
                {
                    Directory.CreateDirectory(basePath + @"03_Montage_boek");
                }

                if (Directory.Exists(basePath + @"03_Montage_boek\" + hoofdArtikel + "_" + DateTime.Now.ToString("yyyy-MM-dd")) == false)
                {
                    Directory.CreateDirectory(basePath + @"03_Montage_boek\" + hoofdArtikel + "_" + DateTime.Now.ToString("yyyy-MM-dd"));
                }

                string savePath = basePath + @"03_Montage_boek\" + hoofdArtikel + "_" + DateTime.Now.ToString("yyyy-MM-dd") +@"\MOB_"+ Part.Number+ "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".pdf";
                if (to.PageCount > 0)
                {
                    to.Save(savePath);
                }
            }
        }
        else if (b == "Lassen")
        {
            if (Part != null)
            {
                string path = basePath + @"05_PDF_DXF_STP_Compleet\" + Part.Number + ".pdf";

                PdfDocument to = new PdfDocument();
                // check if file exists
                if (File.Exists(path))
                {
                    using (PdfDocument part = PdfReader.Open(path, PdfDocumentOpenMode.Import))
                    {
                        CopyPages(part, to);
                    }
                }

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
                    }
                });
                // save pdf
                //check if folder exists
                if (Directory.Exists(basePath + @"04_Las_boek") == false)
                {
                    Directory.CreateDirectory(basePath + @"04_Las_boek");
                }

                if (Directory.Exists(basePath + @"04_Las_boek\" + hoofdArtikel + "_" + DateTime.Now.ToString("yyyy-MM-dd")) == false)
                {
                    Directory.CreateDirectory(basePath + @"04_Las_boek\" + hoofdArtikel + "_" + DateTime.Now.ToString("yyyy-MM-dd"));
                }

                string savePath = basePath + @"04_Las_boek\" + hoofdArtikel + "_" + DateTime.Now.ToString("yyyy-MM-dd") + @"\WEB_" + Part.Number + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".pdf";
                if (to.PageCount > 0)
                {
                    to.Save(savePath);
                }
            }
        }
    }

    void CopyPages(PdfDocument from, PdfDocument to)
    {
        for (int i = 0; i < from.PageCount; i++)
        {
            to.AddPage(from.Pages[i]);
        }
    }
}
