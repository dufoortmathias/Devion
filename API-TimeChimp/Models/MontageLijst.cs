namespace Api.Devion.Models;
public class Lijst
{
    public List<Data> data { get; set; } = new List<Data>();
    public string Project { get; set; } = "";
    public string hoofdartikel { get; set; } = "";
}

public class Data
{
    public string partNumber { get; set; } = "";
    public int Aantal { get; set; } = 0;
    public string Done { get; set; } = "";
}

