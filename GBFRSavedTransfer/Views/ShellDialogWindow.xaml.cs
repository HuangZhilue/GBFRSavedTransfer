using GBFRSavedTransfer.Contracts.Views;
using GBFRSavedTransfer.ViewModels;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace GBFRSavedTransfer.Views;

public partial class ShellDialogWindow : MetroWindow, IShellDialogWindow
{
    public ShellDialogWindow(ShellDialogViewModel viewModel)
    {
        InitializeComponent();
        viewModel.SetResult = OnSetResult;
        DataContext = viewModel;
    }

    public Frame GetDialogFrame()
        => dialogFrame;

    private void OnSetResult(bool? result)
    {
        DialogResult = result;
        Close();
    }
}
