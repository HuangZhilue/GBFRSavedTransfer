using GBFRSavedTransfer.Contracts.Services;
using GBFRSavedTransfer.Contracts.ViewModels;
using GBFRSavedTransfer.Contracts.Views;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace GBFRSavedTransfer.Services;

public class WindowManagerService(IServiceProvider serviceProvider, IPageService pageService) : IWindowManagerService
{
    public Window MainWindow
        => Application.Current.MainWindow;

    public void OpenInNewWindow(string key, object parameter = null)
    {
        Window window = GetWindow(key);
        if (window != null)
        {
            window.Activate();
        }
        else
        {
            window = new MetroWindow()
            {
                Title = "GBFRSavedTransfer",
                Style = Application.Current.FindResource("CustomMetroWindow") as Style
            };
            Frame frame = new()
            {
                Focusable = false,
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };

            window.Content = frame;
            Page page = pageService.GetPage(key);
            window.Closed += OnWindowClosed;
            window.Show();
            frame.Navigated += OnNavigated;
            bool navigated = frame.Navigate(page, parameter);
        }
    }

    public bool? OpenInDialog(string key, object parameter = null)
    {
        Window shellWindow = serviceProvider.GetService(typeof(IShellDialogWindow)) as Window;
        Frame frame = ((IShellDialogWindow)shellWindow).GetDialogFrame();
        frame.Navigated += OnNavigated;
        shellWindow.Closed += OnWindowClosed;
        Page page = pageService.GetPage(key);
        bool navigated = frame.Navigate(page, parameter);
        return shellWindow.ShowDialog();
    }

    public Window GetWindow(string key)
    {
        foreach (Window window in Application.Current.Windows)
        {
            var dataContext = window.GetDataContext();
            if (dataContext?.GetType().FullName == key)
            {
                return window;
            }
        }

        return null;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var dataContext = frame.GetDataContext();
            if (dataContext is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.ExtraData);
            }
        }
    }

    private void OnWindowClosed(object sender, EventArgs e)
    {
        if (sender is Window window)
        {
            if (window.Content is Frame frame)
            {
                frame.Navigated -= OnNavigated;
            }

            window.Closed -= OnWindowClosed;
        }
    }
}
