using WarehouseStockOpname.ViewModels;

namespace WarehouseStockOpname.Views;

public partial class AddStockPage : ContentPage
{
    public AddStockPage(AddStockViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
