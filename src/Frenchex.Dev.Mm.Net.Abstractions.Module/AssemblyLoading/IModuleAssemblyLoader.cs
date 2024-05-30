namespace Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;

/// <summary>
/// 
/// </summary>
public interface IModuleAssemblyLoader
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleAssemblyInformation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IModuleAssemblyLoadingResult> LoadAsync(ModuleAssemblyInformation moduleAssemblyInformation, CancellationToken cancellationToken = default);
}