using Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;
using Frenchex.Dev.Mm.Net.Abstractions.Module.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Frenchex.Dev.Mm.Net.Abstractions.Module;

/// <summary>
/// /
/// </summary>
public interface IModule
{
    /// <summary>
    /// 
    /// </summary>
    Assembly Assembly { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configurationManager"></param>
    /// <param name="moduleAssemblyLoadingInformation"></param>
    /// <param name="configurationSourceBuilder"></param>
    /// <param name="fileProvider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ConfigureConfigurationAsync
    (
        IConfigurationManager configurationManager,
        ModuleAssemblyLoadingInformation moduleAssemblyLoadingInformation, 
        ConfigurationSourceBuilder configurationSourceBuilder,
        IFileProvider fileProvider, 
        CancellationToken cancellationToken = default
    );
}