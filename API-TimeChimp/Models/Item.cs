namespace Api.Devion.Models
{
    public class Item
    {
        public Item(string number, string description, int quantity, string lynNumber)
        {
            Number = number;
            Quantity = quantity;
            Description = description;
            LynNumber = lynNumber;
        }

        public Item() { }

        public string? Number { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; } = 1;
        public string? MainSupplier { get; set; }
        public List<Item?> Parts { get; set; } = new();
        public string? LynNumber { get; set; }
        public string? Bewerking1 { get; set; } = "-";
        public string? Bewerking2 { get; set; } = "-";
        public string? Bewerking3 { get; set; } = "-";
        public string? Bewerking4 { get; set; } = "-";
        public string? Merk { get; set; } = "Devion";
        public float Mass { get; set; } = 0;
        public int AankoopPer { get; set; } = 1;
        public string? Aankoopeenh { get; set; } = "ST";
        public string? Verbruikseenh { get; set; } = "ST";
        public int Omrekeningsfactor { get; set; } = 1;
        public string? TypeFactor { get; set; } = "Deelfactor";
    }

    public class Change
    {
        public string? ETSWaarde { get; set; }
        public string? NewWaarde { get; set; }

        public override string ToString()
        {
            return $"ETSWaarde: {ETSWaarde}; NewWaarde: {NewWaarde}";
        }
    }

    public class ItemChange
    {
        public string? ArticleNumber { get; set; }
        public Change? Change { get; set; }
        public string? Key { get; set; }

        //tostring override
        public override string ToString()
        {
            return $"ArticleNumber: {ArticleNumber}, Change: {Change}, Key: {Key}";
        }
    }
}
