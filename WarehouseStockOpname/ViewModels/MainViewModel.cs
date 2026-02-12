using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WarehouseStockOpname.Models;
using WarehouseStockOpname.Services;
using WarehouseStockOpname.Views;

namespace WarehouseStockOpname.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<StockOpname> stockOpnames = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private int totalProducts;

    [ObservableProperty]
    private int activeOpnames;

    [ObservableProperty]
    private int completedOpnames;

    public MainViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        try
        {
            IsRefreshing = true;

            var opnames = await _databaseService.GetStockOpnamesAsync();
            StockOpnames.Clear();
            foreach (var opname in opnames)
            {
                StockOpnames.Add(opname);
            }

            var products = await _databaseService.GetProductsAsync();
            TotalProducts = products.Count;

            ActiveOpnames = opnames.Count(o => o.Status == "InProgress");
            CompletedOpnames = opnames.Count(o => o.Status == "Completed");
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
    public async Task CreateNewOpnameAsync()
    {
        try
        {
            var opnameNumber = await _databaseService.GenerateOpnameNumberAsync();
            var newOpname = new StockOpname
            {
                OpnameNumber = opnameNumber,
                OpnameDate = DateTime.Now,
                Status = "Draft",
                PerformedBy = "Admin",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _databaseService.SaveStockOpnameAsync(newOpname);
            await LoadDataAsync();

            await Shell.Current.GoToAsync($"{nameof(StockOpnameDetailPage)}?OpnameId={newOpname.Id}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to create stock opname: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task OpenOpnameAsync(StockOpname opname)
    {
        if (opname == null) return;
        await Shell.Current.GoToAsync($"{nameof(StockOpnameDetailPage)}?OpnameId={opname.Id}");
    }

    [RelayCommand]
    public async Task DeleteOpnameAsync(StockOpname opname)
    {
        if (opname == null) return;

        var confirm = await Shell.Current.DisplayAlert("Confirm Delete", 
            $"Are you sure you want to delete opname {opname.OpnameNumber}?", "Yes", "No");

        if (confirm)
        {
            await _databaseService.DeleteStockOpnameDetailsAsync(opname.Id);
            await _databaseService.DeleteStockOpnameAsync(opname);
            await LoadDataAsync();
        }
    }

    [RelayCommand]
    public async Task GoToProductsAsync()
    {
        await Shell.Current.GoToAsync(nameof(ProductListPage));
    }
}
