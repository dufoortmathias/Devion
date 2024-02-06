namespace Api.Devion.Models;

public class PriceSettings
{
    public Settings? Devion {  get; set; }
    public Settings? Metabil {  get; set; }
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