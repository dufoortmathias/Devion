namespace Api.Devion.Models
{
    public class Item
    {
        public Item(string number, string description, int quantity)
        {
            Number = number;
            Quantity = quantity;
            Description = description;
        }

        public string Number { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string? MainSupplier { get; set; }
        public List<Item?> Parts { get; set; } = new();
        public Boolean ExistsETS { get; set; } = false;
    }
}
