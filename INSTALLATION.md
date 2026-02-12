# Installation Guide - Warehouse Stock Opname

Panduan lengkap untuk setup dan menjalankan aplikasi Warehouse Stock Opname.

## 📋 Prerequisites

### Software Requirements

1. **.NET 8 SDK**
   - Download: https://dotnet.microsoft.com/download/dotnet/8.0
   - Verify: `dotnet --version` (harus 8.0.x)

2. **Visual Studio 2022** (Recommended)
   - Edition: Community, Professional, atau Enterprise
   - Version: 17.8 atau lebih baru
   - Workloads:
     - .NET Multi-platform App UI development
     - Mobile development with .NET

   **ATAU**

3. **Visual Studio Code** (Alternative)
   - Extensions:
     - C# Dev Kit
     - .NET MAUI Extension

4. **Android SDK** (untuk Android development)
   - Android SDK Platform 21 atau higher
   - Android Emulator atau Physical Device

5. **Xcode** (untuk iOS/Mac development - Mac only)
   - Latest stable version
   - Command Line Tools

## 🚀 Installation Steps

### Option 1: Visual Studio 2022

#### Step 1: Install Visual Studio
```bash
# Download dari https://visualstudio.microsoft.com/downloads/
# Pilih workload: .NET Multi-platform App UI development
```

#### Step 2: Clone Repository
```bash
git clone https://github.com/abcdels/learn-git-batch-4.git
cd learn-git-batch-4
```

#### Step 3: Open Solution
```bash
# Double-click file .sln (jika ada)
# ATAU open folder WarehouseStockOpname di VS
```

#### Step 4: Restore NuGet Packages
- Visual Studio akan otomatis restore packages
- Atau manual: Right-click solution → Restore NuGet Packages

#### Step 5: Select Target Platform
- Toolbar dropdown → pilih target (Android/iOS/Windows)
- Select device (Emulator atau Physical device)

#### Step 6: Build & Run
- Press F5 atau Click "Start Debugging"
- Wait for build dan deployment

### Option 2: Command Line

#### Step 1: Clone & Navigate
```bash
git clone https://github.com/abcdels/learn-git-batch-4.git
cd learn-git-batch-4/WarehouseStockOpname
```

#### Step 2: Restore Dependencies
```bash
dotnet restore WarehouseStockOpname.csproj
```

#### Step 3: Build for Android
```bash
dotnet build WarehouseStockOpname.csproj -f net8.0-android -c Debug
```

#### Step 4: Run on Android Emulator
```bash
# Pastikan emulator sudah running
# List devices:
adb devices

# Deploy & run:
dotnet build WarehouseStockOpname.csproj -f net8.0-android -t:Run
```

### Option 3: VS Code

#### Step 1: Install Extensions
```bash
code --install-extension ms-dotnettools.csdevkit
code --install-extension ms-dotnettools.dotnet-maui
```

#### Step 2: Open Project
```bash
cd learn-git-batch-4
code .
```

#### Step 3: Configure Launch
Create `.vscode/launch.json`:
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET MAUI Android",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "dotnet: build",
      "program": "${workspaceFolder}/WarehouseStockOpname/bin/Debug/net8.0-android/com.warehouse.stockopname.dll",
      "cwd": "${workspaceFolder}/WarehouseStockOpname"
    }
  ]
}
```

#### Step 4: Build & Run
```bash
# Terminal di VS Code
cd WarehouseStockOpname
dotnet build -f net8.0-android
dotnet run -f net8.0-android
```

## 📱 Platform-Specific Setup

### Android

#### Prerequisites
1. **Android SDK** installed via Visual Studio Installer
2. **Android Emulator** configured:
   ```bash
   # Create AVD (Android Virtual Device)
   # Via Android Studio → AVD Manager
   # OR Visual Studio → Tools → Android Device Manager
   ```

3. **USB Debugging** enabled (untuk physical device):
   - Settings → About Phone → tap Build Number 7x
   - Settings → Developer Options → USB Debugging → Enable

#### Build Commands
```bash
# Debug build
dotnet build -f net8.0-android -c Debug

# Release build (signed)
dotnet publish -f net8.0-android -c Release
```

#### Deploy to Device
```bash
# List connected devices
adb devices

# Install APK
adb install bin/Debug/net8.0-android/com.warehouse.stockopname-Signed.apk

# Launch app
adb shell am start -n com.warehouse.stockopname/.MainActivity
```

### iOS (Mac only)

#### Prerequisites
1. **Xcode** installed from App Store
2. **Command Line Tools**:
   ```bash
   xcode-select --install
   ```

3. **iOS Simulator** or Physical iPhone
4. **Apple Developer Account** (untuk device deployment)

#### Build Commands
```bash
# Debug build
dotnet build -f net8.0-ios -c Debug

# Release build
dotnet publish -f net8.0-ios -c Release
```

#### Deploy to Simulator
```bash
# List simulators
xcrun simctl list devices

# Boot simulator
xcrun simctl boot "iPhone 14 Pro"

# Deploy app
dotnet build -f net8.0-ios -t:Run
```

### Windows

#### Prerequisites
1. **Windows 10/11** with latest updates
2. **Developer Mode** enabled:
   - Settings → Update & Security → For Developers → Developer Mode

#### Build Commands
```bash
# Debug build
dotnet build -f net8.0-windows10.0.19041.0 -c Debug

# Release build
dotnet publish -f net8.0-windows10.0.19041.0 -c Release
```

#### Run
```bash
dotnet run -f net8.0-windows10.0.19041.0
```

## 🔧 Troubleshooting

### Common Issues

#### Issue 1: NuGet Package Restore Failed
```bash
# Solution 1: Clear NuGet cache
dotnet nuget locals all --clear
dotnet restore

# Solution 2: Update packages
dotnet restore --force
```

#### Issue 2: Android SDK Not Found
```bash
# Set ANDROID_HOME environment variable
# Windows:
setx ANDROID_HOME "C:\Program Files (x86)\Android\android-sdk"

# Mac/Linux:
export ANDROID_HOME=~/Library/Android/sdk
export PATH=$PATH:$ANDROID_HOME/tools:$ANDROID_HOME/platform-tools
```

#### Issue 3: Camera Permission Denied
- Check `AndroidManifest.xml` contains:
  ```xml
  <uses-permission android:name="android.permission.CAMERA" />
  ```
- Grant permission manually di device settings

#### Issue 4: Database Not Created
- Check app has storage permission
- Verify path: `FileSystem.AppDataDirectory`
- Clear app data and restart

#### Issue 5: Build Error "Target not found"
```bash
# Check installed workloads
dotnet workload list

# Install MAUI workload
dotnet workload install maui

# Update workloads
dotnet workload update
```

#### Issue 6: ZXing Scanner Not Working
- Ensure camera permission granted
- Test on physical device (emulator camera limited)
- Check `UseBarcodeReader()` called in MauiProgram

### Debug Tips

1. **Enable Debug Output**
   ```csharp
   // In MauiProgram.cs
   #if DEBUG
   builder.Logging.AddDebug();
   builder.Logging.SetMinimumLevel(LogLevel.Debug);
   #endif
   ```

2. **Check Device Logs**
   ```bash
   # Android
   adb logcat | grep -i "WarehouseStockOpname"
   
   # iOS
   xcrun simctl spawn booted log stream --predicate 'process == "WarehouseStockOpname"'
   ```

3. **SQLite Database Viewer**
   - Pull database from device:
     ```bash
     adb pull /data/data/com.warehouse.stockopname/files/warehouse_stockopname.db3
     ```
   - Open with DB Browser for SQLite

## ✅ Verification

### Check Installation Success

1. **App Launches**
   - Dashboard appears
   - No crash on startup

2. **Database Works**
   - Sample products loaded (10 items)
   - Can create new opname

3. **Navigation Works**
   - Can navigate between pages
   - Back button works

4. **Scanner Works** (on physical device)
   - Camera preview appears
   - Can scan barcode

5. **CRUD Operations**
   - Can create stock opname
   - Can add/edit/delete items
   - Can complete opname

### Test Flow
```bash
1. Launch app → See dashboard with 10 products
2. Tap "New Stock Opname" → Opname created
3. Tap "Scan" → Camera opens
4. Scan barcode → Product added
5. Tap "Complete" → Status updated
```

## 📦 First Run

### Sample Data
App otomatis load 10 sample products:
1. Laptop Dell XPS 13
2. Mouse Logitech
3. Keyboard Mechanical
4. Monitor LG
5. Headset Sony
6. Webcam Logitech
7. USB Hub
8. HDMI Cable
9. External SSD
10. Power Bank

### Initial Setup
No configuration needed! App ready to use immediately.

## 🔄 Update Application

### Pull Latest Changes
```bash
cd learn-git-batch-4
git pull origin main
cd WarehouseStockOpname
dotnet restore
dotnet build
```

### Clear App Data (if needed)
```bash
# Android
adb shell pm clear com.warehouse.stockopname

# iOS
xcrun simctl uninstall booted com.warehouse.stockopname
xcrun simctl install booted <path-to-app>
```

## 🛠️ Development Setup

### Hot Reload
Enable for faster development:
```bash
# XAML Hot Reload enabled by default in Debug mode
# C# Hot Reload: 
dotnet watch run -f net8.0-android
```

### IDE Settings

#### Visual Studio
- Tools → Options → MAUI → Enable XAML Hot Reload
- Tools → Options → Debugging → Enable Hot Reload

#### VS Code
- Install .NET MAUI Extension
- Configure tasks.json for build tasks

## 📞 Support

### Get Help
1. Check [README.md](README.md) for features
2. Review [ARCHITECTURE.md](WarehouseStockOpname/ARCHITECTURE.md) for technical details
3. Check [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md) for overview

### Report Issues
- Create issue di GitHub repository
- Include error logs
- Specify platform (Android/iOS/Windows)
- Provide steps to reproduce

## 🎓 Learning Resources

- [.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [SQLite-net Tutorial](https://github.com/praeclarum/sqlite-net)
- [MVVM Toolkit](https://learn.microsoft.com/windows/communitytoolkit/mvvm/)
- [ZXing.Net.Maui](https://github.com/Redth/ZXing.Net.Maui)

---

**Installation Complete! 🎉**

Selamat menggunakan Warehouse Stock Opname App!
