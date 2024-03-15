using GBFRSavedTransfer.Models;

namespace GBFRSavedTransfer.Contracts.Services;

public interface IThemeSelectorService
{
    void InitializeTheme();

    void SetTheme(AppTheme theme);

    AppTheme GetCurrentTheme();
}
