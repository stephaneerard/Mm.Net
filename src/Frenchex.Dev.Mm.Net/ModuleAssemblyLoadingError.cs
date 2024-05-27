namespace Frenchex.Dev.Mm.Net;

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