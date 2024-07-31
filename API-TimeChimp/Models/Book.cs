namespace Api.Devion.Models;

    public class Book
    {
    public Item MainPart { get; set; } = new Item();
    public string Project { get; set; } = "";
    public List<string> Boeken { get; set; } = new List<string>();
    public string hoofdartikel { get; set; } = "";
    }

