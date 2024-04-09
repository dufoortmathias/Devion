using Microsoft.VisualBasic;

namespace Api.Devion.Models;
public class Patch
{
    public string? Op { get; set; }
    public string? Path { get; set; }
    public string? Value { get; set; }
    public Patch(string op,string path,string value)
    {
        Op = op;
        Path = path;
        Value = value;
    }
}