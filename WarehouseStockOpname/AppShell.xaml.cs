using WarehouseStockOpname.Views;

namespace WarehouseStockOpname;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(StockOpnameDetailPage), typeof(StockOpnameDetailPage));
        Routing.RegisterRoute(nameof(ProductListPage), typeof(ProductListPage));
        Routing.RegisterRoute(nameof(ScannerPage), typeof(ScannerPage));
        Routing.RegisterRoute(nameof(AddStockPage), typeof(AddStockPage));
    }
}
