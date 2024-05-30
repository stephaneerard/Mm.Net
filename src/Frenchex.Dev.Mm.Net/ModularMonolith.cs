using Frenchex.Dev.Mm.Net.Abstractions.Module;
using Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;
using Frenchex.Dev.Mm.Net.Abstractions.Module.Configuration;
using Frenchex.Dev.Mm.Net.Abstractions.Module.FileProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.ComponentModel;

namespace Frenchex.Dev.Mm.Net;

/// <summary>
/// 
/// </summary>
public class ModularMonolith
{
    private readonly IModuleAssemblyLoader _moduleAssemblyLoader;
    private readonly IFileProviderProvider _fileProviderProvider;
    private readonly ConfigurationManager _configurationManager = new();
    private List<IModule>? _loadedModules;

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<IModule> Modules => _loadedModules
                                             ?? throw new InvalidAsynchronousStateException("No loaded modules.");

    /// <summary>
    /// 
    /// </summary>
    public IConfiguration Configuration => _configurationManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleAssemblyLoader"></param>
    /// <param name="fileProviderProvider"></param>
    public ModularMonolith
    (
        IModuleAssemblyLoader moduleAssemblyLoader,
        IFileProviderProvider fileProviderProvider
    )
    {
        _moduleAssemblyLoader = moduleAssemblyLoader;
        _fileProviderProvider = fileProviderProvider;
    }

    /// <summary>
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">When Module Path is not found.</exception>
    /// <exception cref="InvalidConfigurationException">Thrown when cannot load a specific assembly.</exception>
    /// <exception cref="System.Security.SecurityException">The caller does not have the required permissions.</exception>
    protected async Task<IList<IModule>> InternalInitializeAsync
    (
        IHostEnvironment hostEnvironment,
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

            var moduleLoadingInformation = new ModuleAssemblyLoadingInformation()
            {
                HostEnvironment = new HostEnvironment()
                {
                    ApplicationName = hostEnvironment.ApplicationName,
                    ContentRootFileProvider = hostEnvironment.ContentRootFileProvider,
                    ContentRootPath = Path.GetFullPath(moduleInformation.Path),
                    EnvironmentName = hostEnvironment.EnvironmentName
                },
                ModuleHostPath = Path.GetDirectoryName(moduleInformation.Path)
                                    ?? throw new InvalidOperationException($"{moduleInformation.Path} error")
            };

            var fileProvider = _fileProviderProvider.Provide(Path.GetFullPath(moduleLoadingInformation.ModuleHostPath));

            await success.LoadedModule.ConfigureConfigurationAsync(
                    configurationManager: _configurationManager
                    , moduleAssemblyLoadingInformation: moduleLoadingInformation
                    , configurationSourceBuilder: new ConfigurationSourceBuilder()
                    , fileProvider: fileProvider
                    , cancellationToken: cancellationToken
                );
        }

        _loadedModules = loadedModules;

        return loadedModules;
    }
}