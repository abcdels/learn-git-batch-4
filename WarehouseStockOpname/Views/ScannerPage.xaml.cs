using WarehouseStockOpname.ViewModels;
using ZXing.Net.Maui;

namespace WarehouseStockOpname.Views;

public partial class ScannerPage : ContentPage
{
    private readonly ScannerViewModel _viewModel;

    public ScannerPage(ScannerViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first is not null)
        {
            Dispatcher.Dispatch(async () =>
            {
                cameraBarcodeReaderView.IsDetecting = false;
                await _viewModel.ProcessBarcodeAsync(first.Value);
                await Task.Delay(2000); // Delay before scanning again
                cameraBarcodeReaderView.IsDetecting = true;
            });
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        cameraBarcodeReaderView.IsDetecting = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        cameraBarcodeReaderView.IsDetecting = false;
    }
}
