using SQLite;
using WarehouseStockOpname.Models;

namespace WarehouseStockOpname.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "warehouse_stockopname.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        
        InitializeAsync().Wait();
    }

    private async Task InitializeAsync()
    {
        await _database.CreateTableAsync<Product>();
        await _database.CreateTableAsync<StockOpname>();
        await _database.CreateTableAsync<StockOpnameDetail>();
        
        // Seed sample data
        await SeedSampleDataAsync();
    }

    private async Task SeedSampleDataAsync()
    {
        var count = await _database.Table<Product>().CountAsync();
        if (count == 0)
        {
            var products = new List<Product>
            {
                new Product { SKU = "PRD001", Name = "Laptop Dell XPS 13", Category = "Electronics", Unit = "Unit", Price = 15000000, SystemStock = 10, Location = "A-01-01", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Intel Core i7, 16GB RAM, 512GB SSD" },
                new Product { SKU = "PRD002", Name = "Mouse Logitech MX Master", Category = "Electronics", Unit = "Unit", Price = 1200000, SystemStock = 25, Location = "A-01-02", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Wireless, Rechargeable" },
                new Product { SKU = "PRD003", Name = "Keyboard Mechanical", Category = "Electronics", Unit = "Unit", Price = 800000, SystemStock = 15, Location = "A-01-03", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "RGB, Cherry MX Blue" },
                new Product { SKU = "PRD004", Name = "Monitor LG 27 inch", Category = "Electronics", Unit = "Unit", Price = 3500000, SystemStock = 8, Location = "A-02-01", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "4K, IPS Panel" },
                new Product { SKU = "PRD005", Name = "Headset Sony WH-1000XM4", Category = "Electronics", Unit = "Unit", Price = 4500000, SystemStock = 12, Location = "A-02-02", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Noise Cancelling, Wireless" },
                new Product { SKU = "PRD006", Name = "Webcam Logitech C920", Category = "Electronics", Unit = "Unit", Price = 1500000, SystemStock = 20, Location = "A-02-03", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "1080p HD" },
                new Product { SKU = "PRD007", Name = "USB Hub 7 Port", Category = "Accessories", Unit = "Unit", Price = 250000, SystemStock = 30, Location = "B-01-01", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "USB 3.0" },
                new Product { SKU = "PRD008", Name = "Cable HDMI 2m", Category = "Accessories", Unit = "Piece", Price = 100000, SystemStock = 50, Location = "B-01-02", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "4K Support" },
                new Product { SKU = "PRD009", Name = "External SSD 1TB", Category = "Storage", Unit = "Unit", Price = 2000000, SystemStock = 18, Location = "B-02-01", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Samsung, USB-C" },
                new Product { SKU = "PRD010", Name = "Power Bank 20000mAh", Category = "Accessories", Unit = "Unit", Price = 500000, SystemStock = 35, Location = "B-02-02", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Fast Charging" }
            };

            await _database.InsertAllAsync(products);
        }
    }

    // Product operations
    public Task<List<Product>> GetProductsAsync() => _database.Table<Product>().ToListAsync();

    public Task<Product?> GetProductByIdAsync(int id) => _database.Table<Product>().Where(p => p.Id == id).FirstOrDefaultAsync();

    public Task<Product?> GetProductBySKUAsync(string sku) => _database.Table<Product>().Where(p => p.SKU == sku).FirstOrDefaultAsync();

    public Task<List<Product>> SearchProductsAsync(string searchText)
    {
        return _database.Table<Product>()
            .Where(p => p.SKU.Contains(searchText) || p.Name.Contains(searchText))
            .ToListAsync();
    }

    public Task<int> SaveProductAsync(Product product)
    {
        product.UpdatedAt = DateTime.Now;
        if (product.Id != 0)
        {
            return _database.UpdateAsync(product);
        }
        else
        {
            product.CreatedAt = DateTime.Now;
            return _database.InsertAsync(product);
        }
    }

    public Task<int> DeleteProductAsync(Product product) => _database.DeleteAsync(product);

    // StockOpname operations
    public Task<List<StockOpname>> GetStockOpnamesAsync() => 
        _database.Table<StockOpname>().OrderByDescending(s => s.OpnameDate).ToListAsync();

    public Task<StockOpname?> GetStockOpnameByIdAsync(int id) => 
        _database.Table<StockOpname>().Where(s => s.Id == id).FirstOrDefaultAsync();

    public async Task<string> GenerateOpnameNumberAsync()
    {
        var date = DateTime.Now;
        var prefix = $"SO{date:yyyyMMdd}";
        
        var count = await _database.Table<StockOpname>()
            .Where(s => s.OpnameNumber.StartsWith(prefix))
            .CountAsync();
        
        return $"{prefix}{(count + 1):D4}";
    }

    public Task<int> SaveStockOpnameAsync(StockOpname stockOpname)
    {
        stockOpname.UpdatedAt = DateTime.Now;
        if (stockOpname.Id != 0)
        {
            return _database.UpdateAsync(stockOpname);
        }
        else
        {
            stockOpname.CreatedAt = DateTime.Now;
            return _database.InsertAsync(stockOpname);
        }
    }

    public Task<int> DeleteStockOpnameAsync(StockOpname stockOpname) => _database.DeleteAsync(stockOpname);

    // StockOpnameDetail operations
    public Task<List<StockOpnameDetail>> GetStockOpnameDetailsAsync(int stockOpnameId) => 
        _database.Table<StockOpnameDetail>().Where(d => d.StockOpnameId == stockOpnameId).ToListAsync();

    public async Task<List<StockOpnameDetailViewModel>> GetStockOpnameDetailViewModelsAsync(int stockOpnameId)
    {
        var details = await _database.Table<StockOpnameDetail>()
            .Where(d => d.StockOpnameId == stockOpnameId)
            .ToListAsync();

        var viewModels = new List<StockOpnameDetailViewModel>();

        foreach (var detail in details)
        {
            var product = await GetProductByIdAsync(detail.ProductId);
            if (product != null)
            {
                viewModels.Add(new StockOpnameDetailViewModel
                {
                    Id = detail.Id,
                    StockOpnameId = detail.StockOpnameId,
                    ProductId = detail.ProductId,
                    SKU = product.SKU,
                    ProductName = product.Name,
                    Location = product.Location,
                    SystemStock = detail.SystemStock,
                    PhysicalStock = detail.PhysicalStock,
                    Difference = detail.Difference,
                    Notes = detail.Notes,
                    ScannedAt = detail.ScannedAt
                });
            }
        }

        return viewModels;
    }

    public Task<StockOpnameDetail?> GetStockOpnameDetailByProductAsync(int stockOpnameId, int productId) =>
        _database.Table<StockOpnameDetail>()
            .Where(d => d.StockOpnameId == stockOpnameId && d.ProductId == productId)
            .FirstOrDefaultAsync();

    public Task<int> SaveStockOpnameDetailAsync(StockOpnameDetail detail)
    {
        if (detail.Id != 0)
        {
            return _database.UpdateAsync(detail);
        }
        else
        {
            return _database.InsertAsync(detail);
        }
    }

    public Task<int> DeleteStockOpnameDetailAsync(StockOpnameDetail detail) => _database.DeleteAsync(detail);

    public async Task DeleteStockOpnameDetailsAsync(int stockOpnameId)
    {
        var details = await GetStockOpnameDetailsAsync(stockOpnameId);
        foreach (var detail in details)
        {
            await DeleteStockOpnameDetailAsync(detail);
        }
    }
}
