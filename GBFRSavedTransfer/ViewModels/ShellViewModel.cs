using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GBFRSavedTransfer.Contracts.Services;
using System.Windows;

namespace GBFRSavedTransfer.ViewModels;

// You can show pages in different ways (update main view, navigate, right pane, new windows or dialog)
// using the NavigationService, RightPaneService and WindowManagerService.
// Read more about MenuBar project type here:
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WPF/projectTypes/menubar.md
public partial class ShellViewModel(INavigationService navigationService, IRightPaneService rightPaneService) : ObservableObject
{
    [RelayCommand]
    private void OnLoaded()
    {
        navigationService.Navigated += OnNavigated;
    }

    [RelayCommand]
    private void OnUnloaded()
    {
        rightPaneService.CleanUp();
        navigationService.Navigated -= OnNavigated;
    }

    private bool CanGoBack()
        => navigationService.CanGoBack;

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void OnGoBack()
        => navigationService.GoBack();

    private void OnNavigated(object sender, string viewModelName)
    {
        GoBackCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private static void OnMenuFileExit()
        => Application.Current.Shutdown();

    [RelayCommand]
    private void OnMenuViewsMain()
        => navigationService.NavigateTo(typeof(MainViewModel).FullName, null, true);

    [RelayCommand]
    private void OnMenuFileSettings()
        => rightPaneService.OpenInRightPane(typeof(SettingsViewModel).FullName);
}
