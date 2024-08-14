namespace Api.Devion.Helpers.General;
public class GeneralKopieerHelper
{
    public void KopieerTekenigen(string BasePath, Kopieer kopieer)
    {
        string filepath = BasePath + @"05_PDF_DXF_STP_Compleet\";
        if (kopieer.Leverancier.ToLower() == "" || kopieer.Leverancier.ToLower() == "-" || kopieer.Leverancier == " " || kopieer.Leverancier == null || kopieer.Leverancier.Length == 0)
        {
            kopieer.Leverancier = "general";
        }
        string savePath = BasePath + @"06_PDF_DXF_STP_Bewerkingen\" + kopieer.Bewerking + @"\" + kopieer.Leverancier + @"\";
        Directory.CreateDirectory(savePath);

        List<string> files = new()
        {
            kopieer.Artikel + ".pdf",
            kopieer.Artikel + ".dxf",
            kopieer.Artikel + ".stp"
        };

        if (kopieer.Bewerking.ToLower() == "3d printen")
        {
            files.Add(kopieer.Artikel + ".stl");
        }

        if (kopieer.Bewerking.ToLower() == "laseren")
        {
            files.Add(kopieer.Artikel + "_FLAT.dxf");
        }

        files.ForEach(file =>
        {
            if (System.IO.File.Exists(filepath + file))
            {
                System.IO.File.Copy(filepath + file, savePath + file, true);
            }
        });
    }

    /*public void KopieerTekeningenNabehandeling(string BasePath, KopieerNa kopieer)
    {
        string filepath = BasePath + @"05_PDF_DXF_STP_Compleet\";
        string savePath = BasePath + @"07_PDF_Nabehandelingen\" + kopieer.Nabehandeling + @"\";
        Directory.CreateDirectory(savePath);

        List<string> files = new()
        {
            kopieer.Artikel + ".pdf",
        };

        files.ForEach(file =>
        {
            if (File.Exists(filepath + file))
            {
                File.Copy(filepath + file, savePath + file, true);
            }
        });
    }*/

    public void KopieerTekeningenNabehandeling(string basePath, KopieerNa kopieer, PdfDocument to)
    {
        if (basePath != null)
        {
            if (System.IO.File.Exists(basePath + @"05_PDF_DXF_STP_Compleet\" + kopieer.Artikel + ".pdf"))
            {
                using (PdfDocument from = PdfReader.Open(basePath + @"05_PDF_DXF_STP_Compleet\" + kopieer.Artikel + ".pdf", PdfDocumentOpenMode.Import))
                {
                    CopyPages(from, to);
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

    public void DeleteFolders(List<string> folders, string BasePath)
    {
        folders.ForEach(folder =>
        {
            if (Directory.Exists(BasePath + folder))
            {
                Directory.Delete(BasePath + folder, true);
            }
        });
    }
}
