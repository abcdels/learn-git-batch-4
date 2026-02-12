# Contributing to Warehouse Stock Opname

Thank you for considering contributing to this project! 🎉

## 🤝 How to Contribute

### Reporting Bugs 🐛

If you find a bug, please create an issue with:
- **Clear title** describing the bug
- **Steps to reproduce** the issue
- **Expected behavior** vs actual behavior
- **Screenshots** if applicable
- **Platform** (Android/iOS/Windows)
- **.NET MAUI version** and device info

### Suggesting Features 💡

We welcome feature suggestions! Please:
- Check existing issues first
- Describe the feature clearly
- Explain the use case
- Consider implementation complexity

### Pull Requests 🔧

1. **Fork the Repository**
   ```bash
   git clone https://github.com/abcdels/learn-git-batch-4.git
   cd learn-git-batch-4
   ```

2. **Create a Branch**
   ```bash
   git checkout -b feature/your-feature-name
   # or
   git checkout -b fix/your-bug-fix
   ```

3. **Make Changes**
   - Follow existing code style
   - Add comments for complex logic
   - Update documentation if needed
   - Test on multiple platforms

4. **Commit Changes**
   ```bash
   git add .
   git commit -m "feat: Add new feature"
   # Use conventional commits:
   # feat: New feature
   # fix: Bug fix
   # docs: Documentation
   # style: Formatting
   # refactor: Code restructuring
   # test: Adding tests
   # chore: Maintenance
   ```

5. **Push and Create PR**
   ```bash
   git push origin feature/your-feature-name
   # Then create PR on GitHub
   ```

## 📝 Code Guidelines

### C# Style
```csharp
// ✅ Good
public async Task<List<Product>> GetProductsAsync()
{
    try
    {
        return await _database.Table<Product>().ToListAsync();
    }
    catch (Exception ex)
    {
        // Handle error
        throw;
    }
}

// ❌ Avoid
public List<Product> GetProducts() // Not async
{
    return _database.Table<Product>().ToList(); // Blocking call
}
```

### XAML Style
```xml
<!-- ✅ Good: Proper indentation and structure -->
<Frame Padding="15" 
       Margin="10" 
       CornerRadius="10"
       BackgroundColor="White">
    <Label Text="Hello" 
           FontSize="16" 
           TextColor="Black"/>
</Frame>

<!-- ❌ Avoid: Poor formatting -->
<Frame Padding="15" Margin="10" CornerRadius="10" BackgroundColor="White"><Label Text="Hello" FontSize="16" TextColor="Black"/></Frame>
```

### MVVM Pattern
```csharp
// ✅ Good: Use ObservableProperty
[ObservableProperty]
private string productName = string.Empty;

[RelayCommand]
public async Task SaveAsync()
{
    // Implementation
}

// ❌ Avoid: Manual INotifyPropertyChanged
private string _productName;
public string ProductName
{
    get => _productName;
    set
    {
        _productName = value;
        OnPropertyChanged();
    }
}
```

## 🧪 Testing

### Before Submitting PR
- [ ] App builds without errors
- [ ] No warnings in build output
- [ ] Tested on Android emulator/device
- [ ] Tested on iOS simulator/device (if applicable)
- [ ] No database errors
- [ ] UI renders correctly
- [ ] Navigation works
- [ ] No crashes

### Manual Test Checklist
- [ ] Create stock opname
- [ ] Add products via scan/manual
- [ ] Edit items
- [ ] Delete items
- [ ] Complete opname
- [ ] Verify system stock update
- [ ] Check statistics update

## 📚 Documentation

If you change functionality:
- [ ] Update README.md
- [ ] Update QUICKSTART.md if user flow changes
- [ ] Update ARCHITECTURE.md if structure changes
- [ ] Add inline code comments
- [ ] Update XML documentation

## 🎨 UI/UX Guidelines

### Colors
Use existing color scheme:
- Primary: #512BD4
- Draft: #FFA500
- InProgress: #0066CC
- Completed: #008000

### Spacing
- Margin: 5, 10, 15
- Padding: 10, 15, 20
- Corner Radius: 8, 10

### Typography
- Title: 20-24
- Body: 14-16
- Caption: 10-12

## 🔐 Security

If you discover a security vulnerability:
- **DO NOT** create a public issue
- Email privately to repository maintainers
- Provide detailed description
- Wait for acknowledgment before disclosure

## 📦 Dependencies

When adding new NuGet packages:
- Use stable versions
- Check compatibility with .NET MAUI 8.0
- Update documentation
- Explain why package is needed

## 🌐 Platform Support

Ensure changes work on:
- ✅ Android (API 21+)
- ✅ iOS (11.0+)
- ✅ Windows (10.0.17763+)
- ✅ macOS (13.1+)

## 🚀 Release Process

### Version Numbering
Follow Semantic Versioning (SemVer):
- MAJOR.MINOR.PATCH (e.g., 1.2.3)
- MAJOR: Breaking changes
- MINOR: New features (backward compatible)
- PATCH: Bug fixes

### Release Checklist
- [ ] All tests pass
- [ ] Documentation updated
- [ ] Version bumped in .csproj
- [ ] CHANGELOG.md updated
- [ ] Tagged in Git
- [ ] Release notes written

## 💬 Communication

### Getting Help
- Check [README.md](README.md) first
- Search existing issues
- Ask in Discussions
- Be respectful and patient

### Code Review
- Be constructive
- Explain reasoning
- Suggest improvements
- Acknowledge good work

## 📋 PR Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Documentation update
- [ ] Performance improvement
- [ ] Code refactoring

## Testing
- [ ] Tested on Android
- [ ] Tested on iOS
- [ ] Tested on Windows
- [ ] Manual testing performed
- [ ] No regressions found

## Screenshots
(If applicable)

## Related Issues
Closes #123
```

## 🏆 Recognition

Contributors will be:
- Listed in GitHub contributors
- Mentioned in release notes
- Credited in documentation

## 📜 Code of Conduct

### Our Standards
- Be respectful and inclusive
- Welcome newcomers
- Accept constructive criticism
- Focus on what's best for the project
- Show empathy

### Unacceptable Behavior
- Harassment or discrimination
- Trolling or insulting comments
- Personal attacks
- Publishing private information
- Unprofessional conduct

## 🎓 Learning Resources

New to .NET MAUI?
- [.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [Microsoft Learn - MAUI](https://learn.microsoft.com/training/paths/build-apps-with-dotnet-maui/)
- [MAUI GitHub](https://github.com/dotnet/maui)

New to MVVM?
- [MVVM Pattern](https://learn.microsoft.com/windows/communitytoolkit/mvvm/)
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet)

## 📞 Contact

- Issues: [GitHub Issues](https://github.com/abcdels/learn-git-batch-4/issues)
- Discussions: [GitHub Discussions](https://github.com/abcdels/learn-git-batch-4/discussions)

## 🙏 Thank You

Every contribution, no matter how small, is valuable!

**Happy Coding! 🚀**

---

*This contributing guide is subject to updates. Please check regularly for changes.*
