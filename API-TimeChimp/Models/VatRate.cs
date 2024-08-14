namespace Api.Devion.Models
{
    public class VatRate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public float percentage { get; set; }
    }
}
