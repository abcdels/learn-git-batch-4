using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WarehouseStockOpname.Models;
using WarehouseStockOpname.Services;

namespace WarehouseStockOpname.ViewModels;

[QueryProperty(nameof(OpnameId), "OpnameId")]
public partial class AddStockViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private int opnameId;

    [ObservableProperty]
    private ObservableCollection<Product> products = new();

    [ObservableProperty]
    private Product? selectedProduct;

    [ObservableProperty]
    private int physicalStock;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private string searchText = string.Empty;

    public AddStockViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    partial void OnOpnameIdChanged(int value)
    {
        if (value > 0)
        {
            Task.Run(async () => await LoadProductsAsync());
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        Task.Run(async () => await SearchProductsAsync());
    }

    [RelayCommand]
    public async Task LoadProductsAsync()
    {
        try
        {
            var productList = await _databaseService.GetProductsAsync();
            Products.Clear();
            foreach (var product in productList.Take(20))
            {
                Products.Add(product);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task SearchProductsAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadProductsAsync();
                return;
            }

            var productList = await _databaseService.SearchProductsAsync(SearchText);
            Products.Clear();
            foreach (var product in productList)
            {
                Products.Add(product);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to search products: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            if (SelectedProduct == null)
            {
                await Shell.Current.DisplayAlert("Validation", "Please select a product", "OK");
                return;
            }

            if (PhysicalStock < 0)
            {
                await Shell.Current.DisplayAlert("Validation", "Physical stock cannot be negative", "OK");
                return;
            }

            // Check if product already in this opname
            var existingDetail = await _databaseService.GetStockOpnameDetailByProductAsync(OpnameId, SelectedProduct.Id);

            if (existingDetail != null)
            {
                existingDetail.PhysicalStock = PhysicalStock;
                existingDetail.Difference = PhysicalStock - existingDetail.SystemStock;
                existingDetail.Notes = Notes;
                await _databaseService.SaveStockOpnameDetailAsync(existingDetail);
            }
            else
            {
                var detail = new StockOpnameDetail
                {
                    StockOpnameId = OpnameId,
                    ProductId = SelectedProduct.Id,
                    SystemStock = SelectedProduct.SystemStock,
                    PhysicalStock = PhysicalStock,
                    Difference = PhysicalStock - SelectedProduct.SystemStock,
                    Notes = Notes,
                    ScannedAt = DateTime.Now,
                    ScannedBy = "Manual"
                };
                await _databaseService.SaveStockOpnameDetailAsync(detail);
            }

            await Shell.Current.DisplayAlert("Success", "Stock data saved successfully", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public void SelectProduct(Product product)
    {
        SelectedProduct = product;
        PhysicalStock = product.SystemStock;
    }
}
