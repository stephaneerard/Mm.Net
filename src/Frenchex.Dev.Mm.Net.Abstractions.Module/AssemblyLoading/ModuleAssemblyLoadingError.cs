namespace Frenchex.Dev.Mm.Net.Abstractions.Module.AssemblyLoading;

/// <summary>
/// 
/// </summary>
public class ModuleAssemblyLoadingError : IModuleAssemblyLoadingResult
{
    /// <summary>
    /// 
    /// </summary>
    public required ModuleAssemblyInformation ModuleAssemblyInformation { get; init; }
}