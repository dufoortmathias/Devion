namespace Api.Devion.Models;

public class Book
{
    public Item MainPart { get; set; } = new Item();
    public string Project { get; set; } = "";
    public List<string> Boeken { get; set; } = new List<string>();
    public string hoofdartikel { get; set; } = "";
}

public class CompleteBook
{
    public string hoofdartikel { get; set; } = "";
    public string Project { get; set; } = "";
    public int Amount { get; set; } = 0;
}

