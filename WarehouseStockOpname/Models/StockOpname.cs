using SQLite;

namespace WarehouseStockOpname.Models;

[Table("stock_opname")]
public class StockOpname
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(50)]
    public string OpnameNumber { get; set; } = string.Empty;

    public DateTime OpnameDate { get; set; }

    [MaxLength(100)]
    public string PerformedBy { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty; // Draft, InProgress, Completed

    [MaxLength(500)]
    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
