namespace WarehouseStockOpname.Models;

public class StockOpnameDetailViewModel
{
    public int Id { get; set; }
    public int StockOpnameId { get; set; }
    public int ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int SystemStock { get; set; }
    public int PhysicalStock { get; set; }
    public int Difference { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime ScannedAt { get; set; }

    public string DifferenceDisplay => Difference >= 0 ? $"+{Difference}" : Difference.ToString();
    public Color DifferenceColor => Difference switch
    {
        > 0 => Colors.Green,
        < 0 => Colors.Red,
        _ => Colors.Gray
    };
}
