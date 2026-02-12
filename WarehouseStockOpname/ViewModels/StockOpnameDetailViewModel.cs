using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WarehouseStockOpname.Models;
using WarehouseStockOpname.Services;
using WarehouseStockOpname.Views;

namespace WarehouseStockOpname.ViewModels;

[QueryProperty(nameof(OpnameId), "OpnameId")]
public partial class StockOpnameDetailViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private int opnameId;

    [ObservableProperty]
    private StockOpname? stockOpname;

    [ObservableProperty]
    private ObservableCollection<StockOpnameDetailViewModel> details = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private int totalItems;

    [ObservableProperty]
    private int totalDifference;

    [ObservableProperty]
    private string statusColor = "#FFA500";

    public StockOpnameDetailViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    partial void OnOpnameIdChanged(int value)
    {
        if (value > 0)
        {
            Task.Run(async () => await LoadDataAsync());
        }
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        try
        {
            IsRefreshing = true;

            StockOpname = await _databaseService.GetStockOpnameByIdAsync(OpnameId);
            
            if (StockOpname != null)
            {
                StatusColor = StockOpname.Status switch
                {
                    "Draft" => "#FFA500",
                    "InProgress" => "#0066CC",
                    "Completed" => "#008000",
                    _ => "#808080"
                };
            }

            var detailList = await _databaseService.GetStockOpnameDetailViewModelsAsync(OpnameId);
            Details.Clear();
            foreach (var detail in detailList)
            {
                Details.Add(detail);
            }

            TotalItems = Details.Count;
            TotalDifference = Details.Sum(d => d.Difference);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    public async Task ScanProductAsync()
    {
        try
        {
            await Shell.Current.GoToAsync($"{nameof(ScannerPage)}?OpnameId={OpnameId}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to open scanner: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task AddManualAsync()
    {
        try
        {
            await Shell.Current.GoToAsync($"{nameof(AddStockPage)}?OpnameId={OpnameId}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to open add page: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task EditDetailAsync(StockOpnameDetailViewModel detail)
    {
        if (detail == null) return;

        string result = await Shell.Current.DisplayPromptAsync(
            "Edit Physical Stock",
            $"Product: {detail.ProductName}\nSystem Stock: {detail.SystemStock}",
            initialValue: detail.PhysicalStock.ToString(),
            keyboard: Keyboard.Numeric);

        if (!string.IsNullOrEmpty(result) && int.TryParse(result, out int physicalStock))
        {
            var detailEntity = await _databaseService.GetStockOpnameDetailByProductAsync(OpnameId, detail.ProductId);
            if (detailEntity != null)
            {
                detailEntity.PhysicalStock = physicalStock;
                detailEntity.Difference = physicalStock - detailEntity.SystemStock;
                await _databaseService.SaveStockOpnameDetailAsync(detailEntity);
                await LoadDataAsync();
            }
        }
    }

    [RelayCommand]
    public async Task DeleteDetailAsync(StockOpnameDetailViewModel detail)
    {
        if (detail == null) return;

        var confirm = await Shell.Current.DisplayAlert("Confirm Delete",
            $"Remove {detail.ProductName} from this opname?", "Yes", "No");

        if (confirm)
        {
            var detailEntity = await _databaseService.GetStockOpnameDetailByProductAsync(OpnameId, detail.ProductId);
            if (detailEntity != null)
            {
                await _databaseService.DeleteStockOpnameDetailAsync(detailEntity);
                await LoadDataAsync();
            }
        }
    }

    [RelayCommand]
    public async Task ChangeStatusAsync()
    {
        if (StockOpname == null) return;

        var action = await Shell.Current.DisplayActionSheet(
            "Change Status",
            "Cancel",
            null,
            "Draft", "In Progress", "Completed");

        if (!string.IsNullOrEmpty(action) && action != "Cancel")
        {
            StockOpname.Status = action.Replace(" ", "");
            await _databaseService.SaveStockOpnameAsync(StockOpname);
            await LoadDataAsync();
        }
    }

    [RelayCommand]
    public async Task CompleteOpnameAsync()
    {
        if (StockOpname == null) return;

        var confirm = await Shell.Current.DisplayAlert("Complete Stock Opname",
            "Mark this stock opname as completed?", "Yes", "No");

        if (confirm)
        {
            StockOpname.Status = "Completed";
            await _databaseService.SaveStockOpnameAsync(StockOpname);
            
            // Update system stock based on physical stock
            var details = await _databaseService.GetStockOpnameDetailsAsync(OpnameId);
            foreach (var detail in details)
            {
                var product = await _databaseService.GetProductByIdAsync(detail.ProductId);
                if (product != null)
                {
                    product.SystemStock = detail.PhysicalStock;
                    await _databaseService.SaveProductAsync(product);
                }
            }

            await Shell.Current.DisplayAlert("Success", "Stock opname completed and system stock updated!", "OK");
            await LoadDataAsync();
        }
    }
}
