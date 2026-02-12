using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WarehouseStockOpname.Models;
using WarehouseStockOpname.Services;

namespace WarehouseStockOpname.ViewModels;

public partial class ProductListViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<Product> products = new();

    [ObservableProperty]
    private ObservableCollection<Product> filteredProducts = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isRefreshing;

    public ProductListViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterProducts();
    }

    [RelayCommand]
    public async Task LoadProductsAsync()
    {
        try
        {
            IsRefreshing = true;

            var productList = await _databaseService.GetProductsAsync();
            Products.Clear();
            FilteredProducts.Clear();
            
            foreach (var product in productList)
            {
                Products.Add(product);
                FilteredProducts.Add(product);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private void FilterProducts()
    {
        FilteredProducts.Clear();

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            foreach (var product in Products)
            {
                FilteredProducts.Add(product);
            }
        }
        else
        {
            var searchLower = SearchText.ToLower();
            foreach (var product in Products)
            {
                if (product.SKU.ToLower().Contains(searchLower) ||
                    product.Name.ToLower().Contains(searchLower) ||
                    product.Category.ToLower().Contains(searchLower))
                {
                    FilteredProducts.Add(product);
                }
            }
        }
    }

    [RelayCommand]
    public async Task ViewProductDetailAsync(Product product)
    {
        if (product == null) return;

        await Shell.Current.DisplayAlert(
            product.Name,
            $"SKU: {product.SKU}\n" +
            $"Category: {product.Category}\n" +
            $"Location: {product.Location}\n" +
            $"Stock: {product.SystemStock} {product.Unit}\n" +
            $"Price: Rp {product.Price:N0}\n" +
            $"Description: {product.Description}",
            "OK");
    }
}
