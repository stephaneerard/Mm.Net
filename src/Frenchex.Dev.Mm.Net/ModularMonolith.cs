using Frenchex.Dev.Mm.Net.Abstractions.Module;
using Microsoft.Extensions.Options;

namespace Frenchex.Dev.Mm.Net;

/// <summary>
/// 
/// </summary>
public class ModularMonolith
{
    private readonly IModuleAssemblyLoader _moduleAssemblyLoader;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleAssemblyLoader"></param>
    public ModularMonolith(IModuleAssemblyLoader moduleAssemblyLoader)
    {
        _moduleAssemblyLoader = moduleAssemblyLoader;
    }

    /// <summary>
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidConfigurationException">Thrown when cannot load a specific assembly.</exception>
    protected async Task<IList<IModule>> InitializeAsync(IOptions<ModularMonolithOptions> options, CancellationToken cancellationToken = default)
    {
        var loaded = new List<IModule>();
        foreach (var moduleInformation in options.Value.Assemblies)
        {
            var assembly = await _moduleAssemblyLoader.LoadAsync(moduleInformation, cancellationToken);
            if (assembly is ModuleAssemblyLoadedSuccessfully success)
                loaded.Add(success.LoadedModule);
            else throw new InvalidConfigurationException(moduleInformation);
        }

        return loaded;
    }
}