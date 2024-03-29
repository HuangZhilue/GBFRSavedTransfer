﻿using GBFRSavedTransfer.Contracts.Services;
using System.Diagnostics;

namespace GBFRSavedTransfer.Services;

public class SystemService : ISystemService
{
    public SystemService()
    {
    }

    public void OpenInWebBrowser(string url)
    {
        // For more info see https://github.com/dotnet/corefx/issues/10361
        ProcessStartInfo psi = new()
        {
            FileName = url,
            UseShellExecute = true
        };
        Process.Start(psi);
    }
}
