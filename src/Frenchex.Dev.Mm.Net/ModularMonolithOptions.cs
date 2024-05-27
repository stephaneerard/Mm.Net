using System.Diagnostics.CodeAnalysis;

namespace Frenchex.Dev.Mm.Net;

/// <summary>
/// 
/// </summary>
public class ModularMonolithOptions
{
    /// <summary>
    /// 
    /// </summary>
    public List<ModuleAssemblyInformation> Assemblies { get; set; } = new();
}