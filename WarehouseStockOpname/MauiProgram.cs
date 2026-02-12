using Microsoft.Extensions.Logging;
using WarehouseStockOpname.Services;
using WarehouseStockOpname.ViewModels;
using WarehouseStockOpname.Views;
using ZXing.Net.Maui.Controls;

namespace WarehouseStockOpname;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Services
        builder.Services.AddSingleton<DatabaseService>();

        // Register ViewModels
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<StockOpnameDetailViewModel>();
        builder.Services.AddTransient<ProductListViewModel>();
        builder.Services.AddTransient<ScannerViewModel>();
        builder.Services.AddTransient<AddStockViewModel>();

        // Register Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<StockOpnameDetailPage>();
        builder.Services.AddTransient<ProductListPage>();
        builder.Services.AddTransient<ScannerPage>();
        builder.Services.AddTransient<AddStockPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
