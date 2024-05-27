using Microsoft.Extensions.Hosting;

namespace Frenchex.Dev.Mm.Net.Abstractions.Module;

/// <summary>
/// 
/// </summary>
public class ModuleLoadingInformation
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