using GBFRSavedTransfer.ViewModels;
using System.Windows.Controls;

namespace GBFRSavedTransfer.Views;

public partial class MainPage : Page
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
