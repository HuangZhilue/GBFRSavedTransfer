using GBFRSavedTransfer.ViewModels;
using System.Windows.Controls;

namespace GBFRSavedTransfer.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
