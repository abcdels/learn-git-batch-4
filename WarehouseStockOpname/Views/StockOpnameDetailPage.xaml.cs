using WarehouseStockOpname.ViewModels;

namespace WarehouseStockOpname.Views;

public partial class StockOpnameDetailPage : ContentPage
{
    private readonly StockOpnameDetailViewModel _viewModel;

    public StockOpnameDetailPage(StockOpnameDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}
