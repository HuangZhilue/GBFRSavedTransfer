using GBFRSavedTransfer.Contracts.Services;
using GBFRSavedTransfer.Contracts.ViewModels;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace GBFRSavedTransfer.Services;

public class RightPaneService(IPageService pageService) : IRightPaneService
{
    private Frame _frame;
    private object _lastParameterUsed;
    private SplitView _splitView;

    public event EventHandler PaneOpened;

    public event EventHandler PaneClosed;

    public void Initialize(Frame rightPaneFrame, SplitView splitView)
    {
        _frame = rightPaneFrame;
        _splitView = splitView;
        _frame.Navigated += OnNavigated;
        _splitView.PaneClosed += OnPaneClosed;
    }

    public void CleanUp()
    {
        _frame.Navigated -= OnNavigated;
        _splitView.PaneClosed -= OnPaneClosed;
    }

    public void OpenInRightPane(string pageKey, object parameter = null)
    {
        Type pageType = pageService.GetPageType(pageKey);
        if (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed)))
        {
            Page page = pageService.GetPage(pageKey);
            bool navigated = _frame.Navigate(page, parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
                var dataContext = _frame.GetDataContext();
                if (dataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }
        }

        _splitView.IsPaneOpen = true;
        PaneOpened?.Invoke(_splitView, EventArgs.Empty);
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            frame.CleanNavigation();
            var dataContext = frame.GetDataContext();
            if (dataContext is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.ExtraData);
            }
        }
    }

    private void OnPaneClosed(object sender, EventArgs e)
        => PaneClosed?.Invoke(sender, e);
}
