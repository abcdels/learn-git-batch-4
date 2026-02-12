using SQLite;

namespace WarehouseStockOpname.Models;

[Table("stock_opname_detail")]
public class StockOpnameDetail
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int StockOpnameId { get; set; }

    [Indexed]
    public int ProductId { get; set; }

    public int SystemStock { get; set; }

    public int PhysicalStock { get; set; }

    public int Difference { get; set; }

    [MaxLength(200)]
    public string Notes { get; set; } = string.Empty;

    public DateTime ScannedAt { get; set; }

    [MaxLength(50)]
    public string ScannedBy { get; set; } = string.Empty;
}
