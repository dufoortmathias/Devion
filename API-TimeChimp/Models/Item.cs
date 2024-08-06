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
        public string Bewerking1 { get; set; } = "-";
        public string Bewerking2 { get; set; } = "-";
        public string Bewerking3 { get; set; } = "-";
        public string Bewerking4 { get; set; } = "-";
        public string? Merk { get; set; } = "Devion";
        public float Mass { get; set; } = 0;
        public string? AankoopPer { get; set; } = "1";
        public string? Aankoopeenh { get; set; } = "ST";
        public string? Verbruikseenh { get; set; } = "ST";
        public string Omrekeningsfactor { get; set; } = "1";
        public string? TypeFactor { get; set; } = "Deelfactor";
        public string? Nabehandeling1 { get; set; } = "-";
        public string? Nabehandeling2 { get; set; } = "-";
        public ItemFile Files { get; set; } = new ItemFile();
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

    public class NewItem
    {
        public string ArtikelNr { get; set; }
        public string Reflev { get; set; }
        public string Omschrijving { get; set; }
        public string Tarief { get; set; }
        public string Aankoop { get; set; }
        public string StdKorting { get; set; }
        public string Muntcode { get; set; }
        public string Verkoop { get; set; }
        public string Winstpercentage { get; set; }
        public string Rekver { get; set; }
        public string Aaneh { get; set; }
        public string Vereh { get; set; }
        public string Btwcode { get; set; }
        public string Omrekfac { get; set; }
        public string Typfac { get; set; }
        public string Merk { get; set; }
        public string Familie { get; set; }
        public string Subfamilie { get; set; }
        public string Lengte { get; set; }
        public string Breedte { get; set; }
        public string Hoogte { get; set; }
        public string Hoofdleverancier { get; set; }
        public string Minaan { get; set; }
        public string Mass { get; set; }
    }

    public class ItemFile
    {
        public string? dxf { get; set; } = "N/A";
        public string? pdf { get; set; } = "N/A";
        public string? stp { get; set; } = "N/A";
        public string? stl { get; set; } = "N/A";
        public string? flatDxf { get; set; } = "N/A";
    }
}