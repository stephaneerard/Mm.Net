using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

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
    /// <param name="moduleLoadingInformation"></param>
    /// <param name="configurationSourceBuilder"></param>
    /// <param name="fileProvider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ConfigureConfigurationAsync
    (
        IConfigurationManager configurationManager,
        ModuleLoadingInformation moduleLoadingInformation, 
        ConfigurationSourceBuilder configurationSourceBuilder,
        IFileProvider fileProvider, 
        CancellationToken cancellationToken = default
    );
}