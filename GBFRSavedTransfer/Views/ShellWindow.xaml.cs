using GBFRSavedTransfer.Contracts.Views;
using GBFRSavedTransfer.ViewModels;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace GBFRSavedTransfer.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame()
        => shellFrame;

    public Frame GetRightPaneFrame()
        => rightPaneFrame;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();

    public SplitView GetSplitView()
        => splitView;
}
