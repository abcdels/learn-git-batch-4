using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WarehouseStockOpname.Models;
using WarehouseStockOpname.Services;

namespace WarehouseStockOpname.ViewModels;

[QueryProperty(nameof(OpnameId), "OpnameId")]
public partial class ScannerViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private int opnameId;

    [ObservableProperty]
    private bool isScanning = true;

    [ObservableProperty]
    private string lastScannedSKU = string.Empty;

    [ObservableProperty]
    private Product? lastScannedProduct;

    public ScannerViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task ProcessBarcodeAsync(string barcode)
    {
        try
        {
            LastScannedSKU = barcode;

            var product = await _databaseService.GetProductBySKUAsync(barcode);
            
            if (product == null)
            {
                await Shell.Current.DisplayAlert("Not Found", $"Product with SKU {barcode} not found", "OK");
                return;
            }

            LastScannedProduct = product;

            // Check if product already in this opname
            var existingDetail = await _databaseService.GetStockOpnameDetailByProductAsync(OpnameId, product.Id);

            if (existingDetail != null)
            {
                // Increment physical stock
                existingDetail.PhysicalStock++;
                existingDetail.Difference = existingDetail.PhysicalStock - existingDetail.SystemStock;
                await _databaseService.SaveStockOpnameDetailAsync(existingDetail);
            }
            else
            {
                // Create new detail
                var detail = new StockOpnameDetail
                {
                    StockOpnameId = OpnameId,
                    ProductId = product.Id,
                    SystemStock = product.SystemStock,
                    PhysicalStock = 1,
                    Difference = 1 - product.SystemStock,
                    ScannedAt = DateTime.Now,
                    ScannedBy = "Scanner"
                };
                await _databaseService.SaveStockOpnameDetailAsync(detail);
            }

            await Shell.Current.DisplayAlert(
                "Scanned Successfully",
                $"{product.Name}\nSystem: {product.SystemStock}\nPhysical: {(existingDetail?.PhysicalStock ?? 1)}",
                "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to process barcode: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task ManualEntryAsync()
    {
        try
        {
            var sku = await Shell.Current.DisplayPromptAsync("Manual Entry", "Enter Product SKU:");
            
            if (!string.IsNullOrEmpty(sku))
            {
                await ProcessBarcodeAsync(sku);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed manual entry: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
