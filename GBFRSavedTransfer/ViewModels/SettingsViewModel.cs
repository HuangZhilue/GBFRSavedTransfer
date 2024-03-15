using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GBFRSavedTransfer.Contracts.Services;
using GBFRSavedTransfer.Contracts.ViewModels;
using GBFRSavedTransfer.Models;
using Microsoft.Extensions.Options;

namespace GBFRSavedTransfer.ViewModels;

public partial class SettingsViewModel(
    IOptions<AppConfig> appConfig,
    IThemeSelectorService themeSelectorService,
    ISystemService systemService,
    IApplicationInfoService applicationInfoService) : ObservableObject, INavigationAware
{
    private readonly AppConfig _appConfig = appConfig.Value;
    [ObservableProperty]
    private AppTheme _theme;
    [ObservableProperty]
    private string _versionDescription;

    public void OnNavigatedTo(object parameter)
    {
        VersionDescription = $"{Properties.Resources.AppDisplayName} - {applicationInfoService.GetVersion()}";
        Theme = themeSelectorService.GetCurrentTheme();
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void OnSetTheme(string themeName)
    {
        AppTheme theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
        themeSelectorService.SetTheme(theme);
    }

    [RelayCommand]
    private void OnPrivacyStatement()
        => systemService.OpenInWebBrowser(_appConfig.MyGithubLink);
}
