using GBFRSavedTransfer.Contracts.Services;
using System.Reflection;

namespace GBFRSavedTransfer.Services;

public class ApplicationInfoService : IApplicationInfoService
{
    public ApplicationInfoService()
    {
    }

    public Version GetVersion()
    {
        // Set the app version in GBFRSavedTransfer > Properties > Package > PackageVersion
        //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        //string version = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
        //return new Version(version);
        return Assembly.GetExecutingAssembly().GetName().Version;
    }
}
