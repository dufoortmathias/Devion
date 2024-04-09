namespace Api.Devion.Models;

public class PriceSettings
{
    public Settings? Devion { get; set; }
    public Settings? Metabil { get; set; }
}

public class BewerkingenSettings
{
    public Settings2? Devion { get; set; }
    public Settings2? Metabil { get; set; }
}

public class Settings
{
    public float? Frezen { get; set; }
    public float? Lassen { get; set; }
    public float? Draaien { get; set; }
    public float? Printen { get; set; }
    public float? Laseren { get; set; }
    public float? Buislaseren { get; set; }
}

public class Settings2
{
    public string? Frezen { get; set; }
    public string? Lassen { get; set; }
    public string? Draaien { get; set; }
    public string? Printen { get; set; }
    public string? Laseren { get; set; }
    public string? Buislaseren { get; set; }
}