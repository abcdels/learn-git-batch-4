using SQLite;

namespace WarehouseStockOpname.Models;

[Table("products")]
public class Product
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(50), Unique]
    public string SKU { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int SystemStock { get; set; }

    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
