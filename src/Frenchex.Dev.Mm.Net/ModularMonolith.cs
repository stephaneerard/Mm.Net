using System.ComponentModel;
using Frenchex.Dev.Mm.Net.Abstractions.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Frenchex.Dev.Mm.Net;

/// <summary>
/// 
/// </summary>
public class ModularMonolith
{
    private readonly IModuleAssemblyLoader _moduleAssemblyLoader;
    private readonly ConfigurationManager? _configurationManager = new ConfigurationManager();
    private List<IModule>? _loadedModules;

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<IModule> Modules => _loadedModules
                                             ?? throw new InvalidAsynchronousStateException("No loaded modules.");

    /// <summary>
    /// 
    /// </summary>
    public IConfiguration Configuration => _configurationManager
                                           ?? throw new InvalidAsynchronousStateException("Configuration is not set.");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleAssemblyLoader"></param>
    public ModularMonolith
    (
        IModuleAssemblyLoader moduleAssemblyLoader
    )
    {
        _moduleAssemblyLoader = moduleAssemblyLoader;
    }

    /// <summary>
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">When Module Path is not found.</exception>
    /// <exception cref="InvalidConfigurationException">Thrown when cannot load a specific assembly.</exception>
    /// <exception cref="System.Security.SecurityException">The caller does not have the required permissions.</exception>
    protected async Task<IList<IModule>> InternalInitializeAsync
    (
        IOptions<ModularMonolithOptions> options,
        CancellationToken cancellationToken = default
    )
    {
        var loadedModules = new List<IModule>();
        foreach (var moduleInformation in options.Value.Assemblies)
        {
            var assembly = await _moduleAssemblyLoader.LoadAsync(moduleInformation, cancellationToken);
            if (assembly is ModuleAssemblyLoadedSuccessfully success)
                loadedModules.Add(success.LoadedModule);
            else throw new InvalidConfigurationException(moduleInformation);

            var moduleLoadingInformation = new ModuleLoadingInformation()
            {
                HostEnvironment = new HostEnvironment()
                {
                    ApplicationName = "MyAcme.dll",
                    EnvironmentName = "Test",
                    ContentRootPath = Path.GetFullPath(moduleInformation.Path)
                },
                ModuleHostPath = Path.GetDirectoryName(moduleInformation.Path)
                                 ?? throw new InvalidOperationException($"{moduleInformation.Path} error")
            };

            var fileProvider = new PhysicalFileProvider(Path.GetFullPath(moduleLoadingInformation.ModuleHostPath));

            await success.LoadedModule.ConfigureConfigurationAsync
                (
                    configurationManager: _configurationManager
                    , moduleLoadingInformation: moduleLoadingInformation
                    , configurationSourceBuilder: new ConfigurationSourceBuilder()
                    , fileProvider: fileProvider
                    , cancellationToken: cancellationToken
                    );
        }

        _loadedModules = loadedModules;

        return loadedModules;
    }
}

public class HostEnvironment : IHostEnvironment
{
    public string EnvironmentName { get; set; }
    public string ApplicationName { get; set; }
    public string ContentRootPath { get; set; }
    public IFileProvider ContentRootFileProvider { get; set; }
}