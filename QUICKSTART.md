# 🚀 Quick Start Guide

Get started with Warehouse Stock Opname in 5 minutes!

## ⚡ Prerequisites Check

```bash
# Check .NET SDK
dotnet --version
# Should show: 8.0.x

# Check MAUI workload
dotnet workload list | grep maui
# Should show: maui installed
```

If not installed:
```bash
# Install .NET 8 SDK from: https://dotnet.microsoft.com/download
# Install MAUI workload:
dotnet workload install maui
```

## 📥 Get the Code

```bash
git clone https://github.com/abcdels/learn-git-batch-4.git
cd learn-git-batch-4/WarehouseStockOpname
```

## 🏃 Run (Choose Your Platform)

### 🤖 Android

```bash
# 1. Start Android Emulator (or connect device)
# 2. Build & Run
dotnet build -f net8.0-android -t:Run

# OR using Visual Studio: Press F5
```

### 🍎 iOS (Mac Only)

```bash
# 1. Open iOS Simulator
# 2. Build & Run
dotnet build -f net8.0-ios -t:Run

# OR using Visual Studio Mac: Press ⌘+Return
```

### 🪟 Windows

```bash
# Build & Run
dotnet run -f net8.0-windows10.0.19041.0

# OR using Visual Studio: Press F5
```

## 🎯 First Steps in App

### 1️⃣ Explore Dashboard
- See 10 sample products
- View empty opname list
- Check statistics cards

### 2️⃣ Create Stock Opname
```
1. Tap "New Stock Opname" button
2. App auto-generates opname number (SO{date}{seq})
3. You're redirected to opname detail page
```

### 3️⃣ Add Products (2 Ways)

#### Option A: Scan Barcode 📷
```
1. Tap "📷 Scan" button
2. Grant camera permission if asked
3. Point camera at barcode
4. Product auto-added to list
```

**Sample Barcodes** (use these SKUs):
- PRD001 - Laptop Dell
- PRD002 - Mouse Logitech
- PRD003 - Keyboard Mechanical
- PRD004 - Monitor LG
- PRD005 - Headset Sony

#### Option B: Manual Entry ➕
```
1. Tap "➕ Add Manual" button
2. Search for product (type "Laptop" or "PRD001")
3. Select product from list
4. Enter physical stock count
5. Tap "Save"
```

### 4️⃣ Review Differences
```
Green number = More stock than system (overage)
Red number = Less stock than system (shortage)
Gray number = Exact match
```

### 5️⃣ Edit Items (Optional)
```
1. Tap ✏️ icon on any item
2. Update physical stock
3. Confirm changes
```

### 6️⃣ Complete Opname
```
1. Review all items
2. Tap "✓ Complete" button
3. Confirm completion
4. System stock auto-updated!
```

## 🎨 UI Quick Tour

### Dashboard (MainPage)
```
┌─────────────────────────────────┐
│   Dashboard                     │
├─────────┬─────────┬─────────────┤
│ Total   │ Active  │ Completed   │
│ Products│ Opnames │ Opnames     │
│   10    │   0     │    0        │
├─────────────────────────────────┤
│ [New Stock Opname] [Products]   │
├─────────────────────────────────┤
│ Opname List (Empty)             │
│ "No stock opname records"       │
│ "Tap New Stock Opname to start" │
└─────────────────────────────────┘
```

### Stock Opname Detail
```
┌─────────────────────────────────┐
│ SO20260212001        [Status]   │
│ Date: 12 Feb 2026               │
│ By: Admin                       │
├─────────┬───────────────────────┤
│ Total   │ Total                 │
│ Items   │ Difference            │
│   5     │   +2                  │
├─────────────────────────────────┤
│ Product List:                   │
│ ┌─────────────────────┬───┬───┐│
│ │ Laptop Dell         │✏️ │🗑️ ││
│ │ SKU: PRD001         │   │   ││
│ │ System:10 Physical:12│   │   ││
│ │ Diff: +2            │   │   ││
│ └─────────────────────┴───┴───┘│
├─────────────────────────────────┤
│ [📷Scan][➕Manual][✓Complete]   │
└─────────────────────────────────┘
```

## 🧪 Test Scenario

Follow this complete flow:

```bash
# Scenario: Stock Take for Electronics Section

1. CREATE OPNAME
   - Tap "New Stock Opname"
   - Note: Opname SO20260212001 created

2. ADD LAPTOP (Manual)
   - Tap "➕ Add Manual"
   - Search: "Laptop"
   - Select: Laptop Dell XPS 13
   - Physical Stock: 12 (system has 10)
   - Save
   - Result: Shows +2 difference (green)

3. ADD MOUSE (Scan)
   - Tap "📷 Scan"
   - Manual Entry: PRD002
   - Result: Mouse added with +1 difference

4. ADD KEYBOARD (Manual)
   - Repeat manual entry
   - Physical: 15 (system has 15)
   - Result: 0 difference (gray)

5. EDIT LAPTOP
   - Tap ✏️ on Laptop
   - Change to: 11
   - Result: +1 difference (green)

6. REVIEW
   - Check total: 3 items
   - Total diff: +2

7. COMPLETE
   - Tap "✓ Complete"
   - Confirm
   - Status: Completed (green)
   - System stocks updated

8. VERIFY
   - Go back to Dashboard
   - Check: Completed = 1
   - View Products
   - Verify: Laptop stock = 11
```

## 📊 Sample Data Reference

### Products Available
| SKU | Name | Category | Location | Stock |
|-----|------|----------|----------|-------|
| PRD001 | Laptop Dell XPS 13 | Electronics | A-01-01 | 10 |
| PRD002 | Mouse Logitech MX | Electronics | A-01-02 | 25 |
| PRD003 | Keyboard Mechanical | Electronics | A-01-03 | 15 |
| PRD004 | Monitor LG 27" | Electronics | A-02-01 | 8 |
| PRD005 | Headset Sony | Electronics | A-02-02 | 12 |
| PRD006 | Webcam Logitech | Electronics | A-02-03 | 20 |
| PRD007 | USB Hub 7 Port | Accessories | B-01-01 | 30 |
| PRD008 | Cable HDMI 2m | Accessories | B-01-02 | 50 |
| PRD009 | External SSD 1TB | Storage | B-02-01 | 18 |
| PRD010 | Power Bank 20000mAh | Accessories | B-02-02 | 35 |

## 🎓 Key Features Demo

### Feature 1: Auto Number Generation
```
Create multiple opnames → see auto-increment:
- SO20260212001
- SO20260212002
- SO20260212003
```

### Feature 2: Status Colors
```
Draft      → Orange  → Work in progress
InProgress → Blue    → Active counting
Completed  → Green   → Finished & locked
```

### Feature 3: Difference Colors
```
+5  → Green  → Overage (more than system)
-3  → Red    → Shortage (less than system)
0   → Gray   → Exact match
```

### Feature 4: Search Products
```
In Product List:
- Type "Laptop" → finds Laptop Dell
- Type "PRD" → finds all PRD codes
- Type "A-01" → finds by location
```

### Feature 5: Real-time Updates
```
Add item → Dashboard stats update immediately
Complete → System stock reflects changes
Delete → Totals recalculate automatically
```

## ⚡ Pro Tips

### 1. Barcode Testing Without Scanner
Use manual entry with SKU codes (PRD001-PRD010)

### 2. Quick Data Reset
```bash
# Clear app data to reset database
adb shell pm clear com.warehouse.stockopname
```

### 3. View Database
```bash
# Pull database from device
adb pull /data/data/com.warehouse.stockopname/files/warehouse_stockopname.db3
# Open with SQLite browser
```

### 4. Multiple Opnames
Create several opnames to test:
- One Draft (planning phase)
- One InProgress (active counting)
- One Completed (finished)

### 5. Negative Differences
Enter physical stock less than system to see red negative numbers

## 🐛 Quick Troubleshooting

### Issue: Camera Not Working
```bash
✓ Check AndroidManifest.xml has CAMERA permission
✓ Grant permission in device settings
✓ Use physical device (emulator limited)
```

### Issue: No Sample Data
```bash
✓ Clear app data and restart
✓ Check DatabaseService initialization
✓ View logs for errors
```

### Issue: Can't Complete Opname
```bash
✓ Check opname has items
✓ Verify status is not already Completed
✓ Check for database errors
```

## 📱 Platform Differences

### Android
- ✅ Full barcode scanning
- ✅ Camera works on device
- ⚠️ Emulator camera limited

### iOS
- ✅ Full barcode scanning
- ✅ Camera works on simulator & device
- ℹ️ Requires permission prompt

### Windows
- ⚠️ No camera support (use manual entry)
- ✅ All other features work

## 🎯 Success Checklist

After quick start, you should have:
- ✅ App running on device/emulator
- ✅ Seen dashboard with sample data
- ✅ Created at least one stock opname
- ✅ Added products (scan or manual)
- ✅ Viewed differences (colored)
- ✅ Completed an opname
- ✅ Verified system stock update

## 📚 Next Steps

After mastering basics:

1. **Explore More**
   - Create multiple opnames
   - Test all product categories
   - Try different scenarios

2. **Advanced Features**
   - Change opname status
   - Add notes to items
   - Delete and recreate

3. **Learn Architecture**
   - Read [ARCHITECTURE.md](WarehouseStockOpname/ARCHITECTURE.md)
   - Understand MVVM pattern
   - Explore code structure

4. **Customize**
   - Modify sample products
   - Add more categories
   - Adjust colors

## 🚀 Ready to Go!

You're now ready to use Warehouse Stock Opname app!

**Time to Complete**: 5-10 minutes  
**Difficulty**: Beginner-friendly  
**Platform**: Android, iOS, Windows

---

## 🆘 Need Help?

- 📖 Full Docs: [README.md](README.md)
- 🔧 Installation: [INSTALLATION.md](INSTALLATION.md)
- 🏗️ Architecture: [WarehouseStockOpname/ARCHITECTURE.md](WarehouseStockOpname/ARCHITECTURE.md)
- 📊 Summary: [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md)

**Happy Stock Taking! 📦✅**
