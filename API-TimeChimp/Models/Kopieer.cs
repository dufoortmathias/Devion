namespace Api.Devion.Models;
public class Kopieer
{
    public string Bewerking { get; set; } = "";
    public string Artikel { get; set; } = "";
    public string Leverancier { get; set; } = "";
    public string Project { get; set; } = "";
}

public class KopieerNa
{
    public string Nabehandeling { get; set; } = "";
    public string Artikel { get; set; } = "";
    public string Project { get; set; } = "";
    public string HoofdArtikel { get; set; } = "";
}

public class KopieerDelete
{
    public List<string> Folders { get; set; } = new List<string>();
    public string BasePath { get; set; } = "";
}
