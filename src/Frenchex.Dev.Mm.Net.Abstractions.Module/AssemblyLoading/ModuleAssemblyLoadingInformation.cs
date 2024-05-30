using Microsoft.Extensions.Hosting;

namespace Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;

/// <summary>
/// 
/// </summary>
public class ModuleAssemblyLoadingInformation
{
    /// <summary>
    /// 
    /// </summary>
    public required IHostEnvironment HostEnvironment { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string ModuleHostPath { get; init; }
}