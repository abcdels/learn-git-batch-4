# Architecture Documentation

## Overview

Warehouse Stock Opname adalah aplikasi mobile cross-platform yang dibangun menggunakan .NET MAUI 8.0 dengan arsitektur MVVM (Model-View-ViewModel).

## Design Patterns

### MVVM (Model-View-ViewModel)
- **Model**: Data entities dan business objects
- **View**: XAML UI pages
- **ViewModel**: Business logic dan state management menggunakan CommunityToolkit.MVVM

### Repository Pattern
- `DatabaseService` sebagai single source of truth untuk data access
- Abstraksi database operations dari business logic

### Dependency Injection
- Built-in .NET MAUI DI container
- Services dan ViewModels diregister di `MauiProgram.cs`

## Layer Architecture

```
┌─────────────────────────────────────────┐
│           Presentation Layer            │
│  (Views + ViewModels + Converters)      │
├─────────────────────────────────────────┤
│          Business Logic Layer           │
│        (ViewModels + Services)          │
├─────────────────────────────────────────┤
│           Data Access Layer             │
│         (DatabaseService)               │
├─────────────────────────────────────────┤
│            Data Layer                   │
│        (SQLite Database)                │
└─────────────────────────────────────────┘
```

## Key Components

### Models
Represent data structures dan entities:
- `Product`: Informasi produk
- `StockOpname`: Header stock opname
- `StockOpnameDetail`: Detail items per opname
- `StockOpnameDetailViewModel`: Rich model untuk UI display

### ViewModels
Handle business logic dan state management:
- `MainViewModel`: Dashboard dan list opnames
- `StockOpnameDetailViewModel`: Detail opname management
- `ProductListViewModel`: Product listing dan search
- `ScannerViewModel`: Barcode scanning logic
- `AddStockViewModel`: Manual stock entry

Menggunakan `[ObservableProperty]` dan `[RelayCommand]` dari CommunityToolkit.MVVM untuk:
- Property change notification
- Command binding
- Async operations

### Services
Provide core functionalities:
- `DatabaseService`: 
  - SQLite operations
  - CRUD operations untuk semua entities
  - Data seeding
  - Query optimization

### Views
XAML pages untuk UI:
- `MainPage`: Dashboard
- `StockOpnameDetailPage`: Opname detail
- `ProductListPage`: Product list
- `ScannerPage`: Barcode scanner
- `AddStockPage`: Manual entry form

### Converters
XAML value converters:
- `StatusToColorConverter`: Status ke color mapping
- `IsNotNullConverter`: Null checking
- `IsNotNullOrEmptyConverter`: String validation
- `InvertedBoolConverter`: Boolean inversion

## Data Flow

### Create Stock Opname Flow
```
User Action (MainPage)
    ↓
MainViewModel.CreateNewOpnameCommand
    ↓
DatabaseService.GenerateOpnameNumberAsync()
DatabaseService.SaveStockOpnameAsync()
    ↓
Navigate to StockOpnameDetailPage
    ↓
Load Data via StockOpnameDetailViewModel
```

### Scan Product Flow
```
User Action (StockOpnameDetailPage)
    ↓
Navigate to ScannerPage
    ↓
Camera detects barcode
    ↓
ScannerViewModel.ProcessBarcodeAsync()
    ↓
DatabaseService.GetProductBySKUAsync()
DatabaseService.SaveStockOpnameDetailAsync()
    ↓
Navigate back with refresh
```

### Complete Opname Flow
```
User Action (Complete button)
    ↓
StockOpnameDetailViewModel.CompleteOpnameAsync()
    ↓
Update StockOpname.Status = "Completed"
    ↓
Loop through details:
    - Get Product
    - Update Product.SystemStock = PhysicalStock
    - Save Product
    ↓
Display success message
```

## Database Design

### Schema
```sql
-- Products Table
CREATE TABLE products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SKU TEXT UNIQUE NOT NULL,
    Name TEXT NOT NULL,
    Description TEXT,
    Category TEXT,
    Unit TEXT,
    Price REAL,
    SystemStock INTEGER,
    Location TEXT,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);

-- StockOpname Table
CREATE TABLE stock_opname (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    OpnameNumber TEXT NOT NULL,
    OpnameDate DATETIME,
    PerformedBy TEXT,
    Status TEXT,
    Notes TEXT,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);

-- StockOpnameDetail Table
CREATE TABLE stock_opname_detail (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StockOpnameId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    SystemStock INTEGER,
    PhysicalStock INTEGER,
    Difference INTEGER,
    Notes TEXT,
    ScannedAt DATETIME,
    ScannedBy TEXT,
    FOREIGN KEY (StockOpnameId) REFERENCES stock_opname(Id),
    FOREIGN KEY (ProductId) REFERENCES products(Id)
);
```

### Indexes
- `stock_opname_detail.StockOpnameId` - For fast detail queries
- `stock_opname_detail.ProductId` - For product lookup

## Navigation

### Shell Navigation
Menggunakan .NET MAUI Shell untuk navigation:
```csharp
// Register routes
Routing.RegisterRoute(nameof(StockOpnameDetailPage), typeof(StockOpnameDetailPage));
Routing.RegisterRoute(nameof(ProductListPage), typeof(ProductListPage));
Routing.RegisterRoute(nameof(ScannerPage), typeof(ScannerPage));
Routing.RegisterRoute(nameof(AddStockPage), typeof(AddStockPage));

// Navigate with parameters
await Shell.Current.GoToAsync($"{nameof(StockOpnameDetailPage)}?OpnameId={id}");

// Navigate back
await Shell.Current.GoToAsync("..");
```

### Query Parameters
Menggunakan `[QueryProperty]` untuk parameter passing:
```csharp
[QueryProperty(nameof(OpnameId), "OpnameId")]
public partial class StockOpnameDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private int opnameId;
    
    partial void OnOpnameIdChanged(int value)
    {
        // Load data when parameter changes
    }
}
```

## State Management

### ObservableProperty
Properties yang trigger UI updates:
```csharp
[ObservableProperty]
private ObservableCollection<Product> products = new();

[ObservableProperty]
private bool isRefreshing;
```

### RelayCommand
Commands untuk user actions:
```csharp
[RelayCommand]
public async Task LoadDataAsync()
{
    // Command implementation
}

[RelayCommand]
public async Task DeleteAsync(Product product)
{
    // Command with parameter
}
```

## Error Handling

### Try-Catch Pattern
Semua async operations wrapped dalam try-catch:
```csharp
try
{
    // Operations
}
catch (Exception ex)
{
    await Shell.Current.DisplayAlert("Error", $"Message: {ex.Message}", "OK");
}
finally
{
    IsRefreshing = false;
}
```

## Performance Optimizations

1. **Lazy Loading**: ViewModels hanya load data saat OnAppearing
2. **Async Operations**: Semua database operations async
3. **ObservableCollection**: Efficient UI updates
4. **SQLite Indexes**: Fast queries pada foreign keys
5. **Local Database**: No network latency

## Security Considerations

1. **Local Storage**: SQLite database di app sandbox
2. **No Authentication**: Currently single-user app
3. **Camera Permissions**: Handled via AndroidManifest.xml
4. **Data Validation**: Input validation di ViewModels

## Testing Strategy

### Unit Tests (Future)
- ViewModel logic testing
- DatabaseService operations
- Business rule validation

### Integration Tests (Future)
- Database CRUD operations
- Navigation flows
- Data integrity

### UI Tests (Future)
- User interaction flows
- Barcode scanning
- Form validation

## Deployment

### Android
```bash
dotnet publish -f net8.0-android -c Release
```

### iOS
```bash
dotnet publish -f net8.0-ios -c Release
```

## Scalability Considerations

### Current Limitations
- Single device, no sync
- Local database only
- No user authentication
- Limited to device storage

### Future Enhancements
- Cloud sync capability
- Multi-user support
- Real-time collaboration
- Server-side API integration
- Offline-first architecture with sync

## Code Quality

### Best Practices
- ✅ MVVM pattern
- ✅ Dependency Injection
- ✅ Async/await pattern
- ✅ Observable pattern
- ✅ Repository pattern
- ✅ Separation of concerns
- ✅ SOLID principles

### Code Organization
- Clear folder structure
- Descriptive naming
- Single responsibility
- Minimal coupling
- High cohesion

## Performance Metrics

### Target Benchmarks
- App startup: < 2 seconds
- Page navigation: < 500ms
- Barcode scan: < 1 second
- Database query: < 100ms
- UI rendering: 60 FPS

## Dependencies

### NuGet Packages
- `Microsoft.Maui.Controls` (8.0.3) - MAUI framework
- `CommunityToolkit.Mvvm` (8.2.2) - MVVM helpers
- `sqlite-net-pcl` (1.8.116) - SQLite ORM
- `SQLitePCLRaw.bundle_green` (2.1.6) - SQLite runtime
- `ZXing.Net.Maui.Controls` (0.4.0) - Barcode scanning

## Troubleshooting

### Common Issues
1. **Camera not working**: Check AndroidManifest.xml permissions
2. **Database not created**: Check file system permissions
3. **Navigation fails**: Verify route registration
4. **Binding not working**: Check DataContext and BindingContext

## References

- [.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/windows/communitytoolkit/mvvm/)
- [SQLite-net](https://github.com/praeclarum/sqlite-net)
- [ZXing.Net.Maui](https://github.com/Redth/ZXing.Net.Maui)
